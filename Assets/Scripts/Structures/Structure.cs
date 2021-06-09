﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Structure : MonoBehaviour {
    [SerializeField] private StructureSO _profile;
    [SerializeField] private string _id;

    [SerializeField] private StringToStructureStatDictionary _stats = new StringToStructureStatDictionary ();
    [SerializeField] private float _hull;
    [SerializeField] private Faction _faction;
    [SerializeField] private FactionSO _initialFaction;

    [SerializeField] private List<EquipmentSlot> _equipmentSlots = new List<EquipmentSlot> ();
    [SerializeField] private ItemSOToIntDictionary _inventory = new ItemSOToIntDictionary ();

    [SerializeReference] private AI _ai;
    [SerializeField] private AISO _initialAI;
    [SerializeField] private bool _aiEnabled;

    [SerializeField] private Structure _selected;
    [SerializeField] private Dictionary<Structure, float> _locks = new Dictionary<Structure, float> ();

    [SerializeField] private Sector _sector;
    [SerializeField] private List<Structure> _detected;

    [SerializeField] private List<Structure> _docked;
    [SerializeField] private List<string> _dockedIds;

    [SerializeField] private bool _initialized;

    private Rigidbody _rb;

    public StructureSO Profile { get => _profile; }
    public string Id { get => _id; }
    public float Hull {

        get => _hull;
        set {

            _hull = value;
            if (_hull <= 0) {

                if (Faction != null) Faction.RemoveProperty (this);
                if (Sector != null) Sector.Exited (this);
                StructureManager.Instance.OnShipDestroyed (this);

            }

        }

    }
    public Faction Faction { get => _faction; set => _faction = value; }
    public List<EquipmentSlot> Equipment { get => _equipmentSlots; }
    public ItemSOToIntDictionary Inventory { get => _inventory; }
    public AI AI { get => _ai; set => _ai = value; }
    public Structure Selected {

        get => _selected;
        set {
            _selected = value;
            if (PlayerController.GetInstance ().GetPlayer () == this) PlayerController.GetInstance ().SelectedChangedChannel.OnEventRaised ();

        }

    }
    public Dictionary<Structure, float> Locks { get => _locks; set => _locks = value; }
    public Sector Sector { get => _sector; set => _sector = value; }
    public List<Structure> Detected { get { if (_detected == null) _detected = StructureManager.Instance.GetDetected (this); return _detected; } set => _detected = value; }

    private void Awake () {

        if (transform.parent != null) {

            _sector = GetComponentInParent<Sector> ();
            if (_sector != null) _sector.Entered (this);

        }

        _rb = GetComponent<Rigidbody> ();

    }

    private void Start () {

        if (!_initialized) Initialize ();

    }

    public void Initialize () {

        _initialized = true;

        StructureManager.Instance.AddStructure (this);

        if (string.IsNullOrEmpty (_id)) _id = Guid.NewGuid ().ToString ();

        EnsureStats ();

        if (_initialFaction != null) _faction = FactionManager.Instance.GetFaction (_initialFaction.Id);
        if (_initialAI != null) _ai = _initialAI.GetAI (this);

        if (_faction != null) _faction.AddProperty (this);

    }

    public float GetStatBaseValue (string name) {
        if (!_stats.ContainsKey (name)) return 0;
        return _stats[name].BaseValue;
    }

    public float GetStatAppliedValue (string name) {
        if (!_stats.ContainsKey (name)) return 0;
        return _stats[name].GetAppliedValue ();
    }

    public void AddStatModifier (StructureStatModifier modifier) {
        if (!_stats.ContainsKey (modifier.Target)) return;
        _stats[modifier.Target].AddModifier (modifier);
    }

    public void RemoveStatModifier (StructureStatModifier modifier) {
        if (!_stats.ContainsKey (modifier.Target)) return;
        _stats[modifier.Target].RemoveModifier (modifier);
    }

    public void RemoveStatModifier (string stat, string id) {
        if (!_stats.ContainsKey (stat)) return;
        _stats[stat].RemoveModifier (id);
    }

    public List<T> GetEquipmentData<T> () where T : EquipmentSlotData {
        List<T> data = new List<T> ();
        _equipmentSlots.ForEach (slot => {
            if (slot.Data is T) data.Add (slot.Data as T);
        });
        return data;
    }

    public int GetInventoryCount (ItemSO item) {

        if (item == null) return 0;
        return _inventory.ContainsKey (item) ? _inventory[item] : 0;

    }

    public void SetInventoryCount (ItemSO item, int count) {

        if (item == null) return;
        _inventory[item] = count;

    }

    public void ChangeInventoryCount (ItemSO item, int delta) {

        if (item == null) return;
        SetInventoryCount (item, GetInventoryCount (item) + delta);

    }

    public bool HasInventoryCount (ItemSO item, int condition) {

        if (item == null) return false;
        return GetInventoryCount (item) >= condition;

    }

    public float GetTotalInventorySize () { return _profile.InventorySize; }

    public float GetUsedInventorySize () {

        float used = 0;
        foreach (ItemSO item in _inventory.Keys) used += item.Volume * _inventory[item];
        return used;

    }

    public float GetFreeInventorySize () { return GetTotalInventorySize () - GetUsedInventorySize (); }

    public bool CanAddInventoryItem (ItemSO item, int count) { return GetFreeInventorySize () >= item.Volume * count; }

    public List<Structure> GetDocked () { return _docked; }

    public bool AddDocker (Structure docker) {

        if (!CanAddDocker (docker)) return false;

        // Add as child
        _docked.Add (docker);
        docker.transform.parent = transform;

        // Disable renderers
        Renderer[] renderers = docker.GetComponentsInChildren<Renderer> ();
        foreach (Renderer r in renderers) r.enabled = false;

        // Disable all equipment
        //foreach (NewEquipmentSlot slot in docker.Equipment) slot.TargetState = false;

        if (docker == PlayerController.GetInstance ().GetPlayer ()) {

            // Send notification
            NotificationUI.GetInstance ().AddNotification ("Docked at " + gameObject.name);

            // Update UI
            UIStateManager.GetInstance ().AddState (UIState.Docked);

            // Update camera anchor
            CameraController.GetInstance ().SetAnchor (new Location (transform));

        }

        return true;

    }

    public void RemoveDocker (Structure docker) {

        if (_docked.Remove (docker)) {

            // Remove as child
            docker.transform.parent = transform.parent;

            // Enable renderers
            Renderer[] renderers = docker.GetComponentsInChildren<Renderer> (true);
            foreach (Renderer r in renderers) r.enabled = true;

            if (docker == PlayerController.GetInstance ().GetPlayer ()) {

                // Send notification
                NotificationUI.GetInstance ().AddNotification ("Undocked from " + gameObject.name);

                // Update UI
                UIStateManager.GetInstance ().RemoveState ();

                // Update camera anchor
                CameraController.GetInstance ().RemoveAnchor ();

            }

        }

    }

    public bool CanAddDocker (Structure docker) {

        // In the same sector or already docked?
        if (docker.transform.parent != transform.parent) return false;
        // Good relations?
        if (_faction.IsEnemy (docker.Faction)) return false;
        // Within range?
        if (NavigationManager.GetInstance ().GetLocalDistance (this, docker) > 50) return false;
        // Already children?
        if (_docked.Contains (docker)) return false;
        // Enough space?
        float size = 0;
        foreach (Structure c in _docked) size += c.Profile.ApparentSize;
        if (docker.Profile.ApparentSize > _stats[StructureStatNames.DockingBaySize].GetAppliedValue () - size) return false;

        return true;

    }

    public bool IsDocked () { return GetDockedAt () != null; }

    public Structure GetDockedAt () { return transform.parent.GetComponent<Structure> (); }

    public bool CanDockTarget () { return _selected != null && _selected.CanAddDocker (this); }

    public bool CanUndock () { return IsDocked (); }

    public void DockTarget () { _selected.AddDocker (this); }

    public bool Undock () {

        if (!CanUndock ()) return false;

        GetDockedAt ().RemoveDocker (this);
        return true;

    }

    public void TakeDamage (Damage damage, Vector3 from) {
        ShieldSlotData closest = null;
        GetEquipmentData<ShieldSlotData> ().ForEach (shield => {
            if (closest == null) closest = shield;
            else {
                float disA = (from - closest.Slot.transform.position).sqrMagnitude;
                float disB = (shield.Slot.transform.position - closest.Slot.transform.position).sqrMagnitude;
                if (disB < disA) closest = shield;
            }
        });
        float ps = 0;
        if (closest != null) ps = Mathf.Clamp01 (closest.Strength / damage.ShieldDamage) * (1 - damage.ShieldPenetration);
        float ph = 1 - ps;
        if (closest != null) closest.Strength -= damage.ShieldDamage * ps;
        Hull -= damage.HullDamage * ph;
        // TODO equipment damage damage.EquipmentDamage * ph
    }

    public float GetAngleTo (Vector3 to) {

        Vector3 heading = to - transform.localPosition;
        return Vector3.SignedAngle (transform.forward, heading, transform.up);

    }

    public float GetElevationTo (Vector3 to) {

        Vector3 heading = to - transform.localPosition;
        return Vector3.SignedAngle (transform.forward, heading, -transform.right);

    }

    public void ManageSelectedAndLocks () {

        if (Selected != null && !Detected.Contains (Selected)) Selected = null;
        bool lockChanged = false;
        foreach (Structure target in Locks.Keys.ToArray ()) {
            if (target == null || !Detected.Contains (target) || Locks.Keys.Count > GetStatAppliedValue (StructureStatNames.MaxLocks)) {
                Locks.Remove (target);
                lockChanged = true;
            }
        }
        if (lockChanged) {
            PlayerController pc = PlayerController.GetInstance ();
            if (pc.GetPlayer () == this) pc.LocksChangedChannel.OnEventRaised.Invoke ();
        }

    }

    public bool Lock (Structure target) {

        if (target == null) return false;
        if (!Detected.Contains (target)) return false;
        if (Locks.ContainsKey (target)) return false;
        if (Locks.Keys.Count >= GetStatAppliedValue (StructureStatNames.MaxLocks)) return false;
        Locks[target] = 0;
        PlayerController pc = PlayerController.GetInstance ();
        if (pc.GetPlayer () == this) pc.LocksChangedChannel.OnEventRaised.Invoke ();
        return true;

    }

    public bool Unlock (Structure target) {

        if (target == null) return false;
        if (!Locks.ContainsKey (target)) return false;
        Locks.Remove (target);
        PlayerController pc = PlayerController.GetInstance ();
        if (pc.GetPlayer () == this) pc.LocksChangedChannel.OnEventRaised.Invoke ();
        return true;

    }

    public void TickLocks () {

        foreach (Structure target in Locks.Keys.ToArray ()) {

            float progress = GetStatAppliedValue (StructureStatNames.ScannerStrength) * target.GetStatAppliedValue (StructureStatNames.SignatureSize) * Time.deltaTime;
            Locks[target] = Mathf.Min (Locks[target] + progress, 100);

        }

    }

    public void Tick () {

        ManageSelectedAndLocks ();
        TickLocks ();

        if (_ai == null) _ai = new AI (this);
        if (_aiEnabled) _ai.Tick ();

        foreach (EquipmentSlot slot in _equipmentSlots) slot.Tick ();

    }

    public void FixedTick () {

        foreach (EquipmentSlot slot in _equipmentSlots) slot.FixedTick ();

        if (_profile.SnapToPlane) {

            // Set position and rotation
            transform.LeanSetLocalPosY (0);
            transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, _rb.angularVelocity.y * -25);

        }

    }

    private void EnsureStats () {
        if (!_stats.ContainsKey (StructureStatNames.SensorStrength)) _stats[StructureStatNames.SensorStrength] = new StructureStat { Name = StructureStatNames.SensorStrength, BaseValue = _profile.SensorStrength };
        if (!_stats.ContainsKey (StructureStatNames.ScannerStrength)) _stats[StructureStatNames.ScannerStrength] = new StructureStat { Name = StructureStatNames.ScannerStrength, BaseValue = _profile.ScannerStrength };
        if (!_stats.ContainsKey (StructureStatNames.MaxLocks)) _stats[StructureStatNames.MaxLocks] = new StructureStat { Name = StructureStatNames.MaxLocks, BaseValue = _profile.MaxLocks };
        if (!_stats.ContainsKey (StructureStatNames.Detectability)) _stats[StructureStatNames.Detectability] = new StructureStat { Name = StructureStatNames.Detectability, BaseValue = _profile.Detectability };
        if (!_stats.ContainsKey (StructureStatNames.SignatureSize)) _stats[StructureStatNames.SignatureSize] = new StructureStat { Name = StructureStatNames.SignatureSize, BaseValue = _profile.SignatureSize };
        if (!_stats.ContainsKey (StructureStatNames.DockingBaySize)) _stats[StructureStatNames.DockingBaySize] = new StructureStat { Name = StructureStatNames.DockingBaySize, BaseValue = _profile.DockingBaySize };
        if (!_stats.ContainsKey (StructureStatNames.DamageMultiplier)) _stats[StructureStatNames.DamageMultiplier] = new StructureStat { Name = StructureStatNames.DamageMultiplier, BaseValue = 1 };
        if (!_stats.ContainsKey (StructureStatNames.RechargeMultiplier)) _stats[StructureStatNames.RechargeMultiplier] = new StructureStat { Name = StructureStatNames.RechargeMultiplier, BaseValue = 1 };
        if (!_stats.ContainsKey (StructureStatNames.LinearSpeedMultiplier)) _stats[StructureStatNames.LinearSpeedMultiplier] = new StructureStat { Name = StructureStatNames.LinearSpeedMultiplier, BaseValue = 1 };
        if (!_stats.ContainsKey (StructureStatNames.AngularSpeedMultiplier)) _stats[StructureStatNames.AngularSpeedMultiplier] = new StructureStat { Name = StructureStatNames.AngularSpeedMultiplier, BaseValue = 1 };
    }

    public StructureSaveData GetSaveData () {
        StructureSaveData data = new StructureSaveData {
            Name = gameObject.name,
            Position = new float[] { transform.localPosition.x, transform.localPosition.y, transform.localPosition.z },
            Rotation = new float[] { transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w },
            Hull = _hull,
            InventoryIds = _inventory.Keys.Select (item => item.Id).ToList (),
            InventoryCounts = _inventory.Values.ToList (),
            Stats = _stats,
        };
        if (_profile != null) data.ProfileId = _profile.Id;
        if (_faction != null) data.FactionId = _faction.GetId ();
        _equipmentSlots.ForEach (slot => { data.Equipment.Add (slot.Data.Save ()); });
        if (_sector != null) data.SectorId = _sector.GetId ();
        data.AIEnabled = _aiEnabled;
        if (PlayerController.GetInstance ().GetPlayer () == this) data.IsPlayer = true;
        return data;
    }

    public void SetSaveData (StructureSaveData saveData) {
        gameObject.name = saveData.Name;
        transform.localPosition = new Vector3 (saveData.Position[0], saveData.Position[1], saveData.Position[2]);
        transform.localRotation = new Quaternion (saveData.Rotation[0], saveData.Rotation[1], saveData.Rotation[2], saveData.Rotation[3]);
        _hull = saveData.Hull;
        _faction = FactionManager.Instance.GetFaction (saveData.FactionId);
        for (int i = 0; i < _equipmentSlots.Count; i++) {
            _equipmentSlots[i].Data = saveData.Equipment[i].Load ();
            _equipmentSlots[i].Data.Slot = _equipmentSlots[i];
        }
        ItemManager im = ItemManager.GetInstance ();
        for (int i = 0; i < saveData.InventoryIds.Count; i++) {
            _inventory[im.GetItem (saveData.InventoryIds[i])] = saveData.InventoryCounts[i];
        }
        _stats = saveData.Stats;
        if (_stats == null) _stats = new StringToStructureStatDictionary ();
        EnsureStats ();
        _sector = SectorManager.Instance.GetSector (saveData.SectorId);
        _sector.Entered (this);
        _aiEnabled = saveData.AIEnabled;
    }
}

[Serializable]
public class StructureSaveData {
    public string Name;
    public float[] Position;
    public float[] Rotation;
    public string ProfileId;
    public string Id;
    public float Hull;
    public string FactionId;
    public List<EquipmentSlotSaveData> Equipment = new List<EquipmentSlotSaveData> ();
    public List<string> InventoryIds = new List<string> ();
    public List<int> InventoryCounts = new List<int> ();
    public StringToStructureStatDictionary Stats = new StringToStructureStatDictionary ();
    public string SectorId;
    public bool AIEnabled;
    public bool IsPlayer;
}
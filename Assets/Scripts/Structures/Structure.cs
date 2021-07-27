﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Structure : MonoBehaviour {
    public StructureSO Profile { get => _profile; }
    [SerializeField] private StructureSO _profile;

    public string Id { get => _id; }
    [SerializeField] private string _id;

    public StatList Stats { get => stats; }
    [SerializeField] private StatList stats = new StatList ();

    public float Hull {
        get => hull;
        set {
            hull = Mathf.Min (value, stats.GetBaseValue (StatNames.MaxHull, 1));
            if (hull <= 0) {
                if (Faction != null) Faction.RemoveProperty (this);
                if (Sector != null) Sector.Exited (this);
                StructureManager.Instance.DestroyStructure (this);
            }
        }
    }
    [SerializeField] private float hull;

    public Faction Faction {
        get => _faction;
        set => _faction = value;
    }
    [SerializeReference, Expandable] private Faction _faction;

    public List<EquipmentSlot> Equipment { get => _equipmentSlots; }
    [SerializeField] private List<EquipmentSlot> _equipmentSlots = new List<EquipmentSlot> ();

    public Inventory Inventory { get => _inventory; }
    [SerializeReference] private Inventory _inventory;

    public AI AI {
        get => _ai;
        set => _ai = value;
    }
    [SerializeReference, Expandable] private AI _ai;
    [SerializeField] private bool _aiEnabled;

    public Structure Selected {
        get => _selected;
        set {
            _selected = value;
            if (PlayerController.Instance.Player == this) PlayerController.Instance.OnSelectedChanged?.Invoke (this, EventArgs.Empty);
        }
    }
    [SerializeField] private Structure _selected;

    public Dictionary<Structure, float> Locks {
        get => _locks;
        set => _locks = value;
    }
    [SerializeField] private Dictionary<Structure, float> _locks = new Dictionary<Structure, float> ();

    public Sector Sector {
        get => _sector;
        set => _sector = value;
    }
    [SerializeField] private Sector _sector;

    public HashSet<Structure> Detected { get => detected ?? (detected = StructureManager.Instance.GetDetected (this)); }
    [SerializeField] private HashSet<Structure> detected;

    public DockingBayList DockingBays { get => dockingBays; }
    [SerializeReference] private DockingBayList dockingBays;

    public Structure Dockee { get => dockee; }
    [SerializeField] private Structure dockee;
    public bool IsDocked { get => dockee != null; }

    [SerializeField] private bool _initialized;

    private Rigidbody _rb;

    private void Start () {
        Initialize ();
    }

    public void Initialize () {
        if (_initialized) return;

        _initialized = true;

        if (transform.parent != null) {
            _sector = GetComponentInParent<Sector> ();
            if (_sector != null) _sector.Entered (this);
        }
        _rb = GetComponent<Rigidbody> ();

        StructureManager.Instance.AddStructure (this);

        if (string.IsNullOrEmpty (_id)) _id = Guid.NewGuid ().ToString ();

        EnsureStats ();

        if (_ai == null) _ai = ScriptableObject.CreateInstance<AI> ();
        else _ai = _ai.Copy ();

        if (_faction != null) {
            FactionManager.Instance.AddFaction (_faction);
            _faction.AddProperty (this);
        }

        if (_inventory == null) _inventory = new Inventory (stats.GetAppliedValue (StatNames.InventoryVolume, 0), 1);
        SynchronizeInventoryVolume (this, EventArgs.Empty);

        if (dockingBays == null) dockingBays = _profile.DockingBays.Copy (this);

        StructureInventoryAdder adder = GetComponent<StructureInventoryAdder> ();
        if (adder != null) adder.Run (this);
    }

    private void OnEnable () {
        stats.GetStat (StatNames.InventoryVolume, 0).OnValueChanged += SynchronizeInventoryVolume;
    }

    private void OnDisable () {
        stats.GetStat (StatNames.InventoryVolume, 0).OnValueChanged -= SynchronizeInventoryVolume;
    }

    private void SynchronizeInventoryVolume (object sender, EventArgs args) => _inventory.Volume = stats.GetAppliedValue (StatNames.InventoryVolume, 0);

    public float GetProfileValue (string name, float baseValue) {
        if (_profile == null || _profile.Stats == null) return baseValue;
        return _profile.Stats.GetBaseValue (name, baseValue);
    }

    public List<T> GetEquipmentData<T> () where T : EquipmentSlotData {
        List<T> data = new List<T> ();
        _equipmentSlots.ForEach (slot => {
            if (slot.Data is T) data.Add (slot.Data as T);
        });
        return data;
    }

    public void OnDocked (Structure dockee) {
        // Cache dockee
        this.dockee = dockee;
        // Add as child
        transform.parent = dockee.transform;
        // Disable renderers
        Renderer[] renderers = GetComponentsInChildren<Renderer> ();
        foreach (Renderer renderer in renderers) renderer.enabled = false;
        // Disable colliders
        Collider[] colliders = GetComponentsInChildren<Collider> ();
        foreach (Collider collider in colliders) collider.enabled = false;
        // TODO Disable all equipment
        //foreach (NewEquipmentSlot slot in docker.Equipment) slot.TargetState = false;
        if (this == PlayerController.Instance.Player) {
            // Send notification
            NotificationUI.GetInstance ().AddNotification ("Docked at " + dockee.name);
            // TODO Update UI
            //UIStateManager.Instance.AddState (UIState.Docked);
            // Update camera anchor
            CameraController.GetInstance ().SetAnchor (new Location (dockee.transform));
        }
    }

    public void OnUndocked (Structure dockee) {
        // Remove dockee
        this.dockee = null;
        // Remove as child
        transform.parent = dockee.transform.parent;
        // Enable renderers
        Renderer[] renderers = GetComponentsInChildren<Renderer> (true);
        foreach (Renderer renderer in renderers) renderer.enabled = true;
        // Enable colliders
        Collider[] colliders = GetComponentsInChildren<Collider> ();
        foreach (Collider collider in colliders) collider.enabled = true;
        if (this == PlayerController.Instance.Player) {
            // Send notification
            NotificationUI.GetInstance ().AddNotification ("Undocked from " + dockee.name);
            // TODO Update UI
            //UIStateManager.Instance.RemoveState ();
            // Update camera anchor
            CameraController.GetInstance ().RemoveAnchor ();
        }
        // Set position
        transform.localPosition = dockee.transform.localPosition + dockee.transform.localRotation * Vector3.forward * 100;
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
            if (target == null || !Detected.Contains (target) || Locks.Keys.Count > stats.GetAppliedValue (StatNames.MaxTargetLocks, 0)) {
                Locks.Remove (target);
                lockChanged = true;
            }
        }
        if (lockChanged) {
            PlayerController pc = PlayerController.Instance;
            if (pc.Player == this) pc.OnLocksChanged.Invoke (this, EventArgs.Empty);
        }
    }

    public bool Lock (Structure target) {
        if (target == null) return false;
        if (!Detected.Contains (target)) return false;
        if (Locks.ContainsKey (target)) return false;
        if (Locks.Keys.Count >= stats.GetAppliedValue (StatNames.MaxTargetLocks, 0)) return false;
        Locks[target] = 0;
        PlayerController pc = PlayerController.Instance;
        if (pc.Player == this) pc.OnLocksChanged.Invoke (this, EventArgs.Empty);
        return true;
    }

    public bool Unlock (Structure target) {
        if (target == null) return false;
        if (!Locks.ContainsKey (target)) return false;
        Locks.Remove (target);
        PlayerController pc = PlayerController.Instance;
        if (pc.Player == this) pc.OnLocksChanged.Invoke (this, EventArgs.Empty);
        return true;
    }

    public void TickLocks () {
        foreach (Structure target in Locks.Keys.ToArray ()) {
            float progress = stats.GetAppliedValue (StatNames.ScannerStrength, 0) * target.Stats.GetAppliedValue (StatNames.SignatureSize, 0) * Time.deltaTime;
            Locks[target] = Mathf.Min (Locks[target] + progress, 100);
        }
    }

    public void Tick () {
        ManageSelectedAndLocks ();
        TickLocks ();

        foreach (EquipmentSlot slot in _equipmentSlots) slot.Tick ();

        stats.Tick ();

        _inventory.Volume = stats.GetAppliedValue (StatNames.InventoryVolume, 0);
    }

    public void FixedTick () {
        foreach (EquipmentSlot slot in _equipmentSlots) slot.FixedTick ();

        if (_profile.SnapToPlane) {
            // Set position and rotation
            transform.LeanSetLocalPosY (0);
            transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, _rb.angularVelocity.y * -25);
        }
    }

    public void ExpensiveTick () {
        detected = null;

        if (_aiEnabled) _ai.Tick (this);
    }

    private void EnsureStats () {
        if (_profile != null && _profile.Stats != null) {
            foreach (Stat stat in _profile.Stats.Stats) {
                stats.AddStat (stat.Copy ());
            }
        }
    }

    public StructureSaveData GetSaveData () {
        StructureSaveData data = new StructureSaveData {
            Name = gameObject.name,
            Position = new float[] { transform.localPosition.x, transform.localPosition.y, transform.localPosition.z },
            Rotation = new float[] { transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w },
            Hull = hull,
            Inventory = _inventory.Save (),
            Stats = stats.Save (),
        };
        if (_profile != null) data.ProfileId = _profile.Id;
        if (_faction != null) data.FactionId = _faction.Id;
        _equipmentSlots.ForEach (slot => { data.Equipment.Add (slot.Data.Save ()); });
        if (_sector != null) data.SectorId = _sector.GetId ();
        data.AIEnabled = _aiEnabled;
        if (PlayerController.Instance.Player == this) data.IsPlayer = true;
        return data;
    }

    public void SetSaveData (StructureSaveData saveData) {
        gameObject.name = saveData.Name;
        transform.localPosition = new Vector3 (saveData.Position[0], saveData.Position[1], saveData.Position[2]);
        transform.localRotation = new Quaternion (saveData.Rotation[0], saveData.Rotation[1], saveData.Rotation[2], saveData.Rotation[3]);
        hull = saveData.Hull;
        _faction = FactionManager.Instance.GetFaction (saveData.FactionId);
        for (int i = 0; i < _equipmentSlots.Count; i++) {
            _equipmentSlots[i].Data = saveData.Equipment[i].Load ();
            _equipmentSlots[i].Data.Slot = _equipmentSlots[i];
        }
        _inventory = saveData.Inventory.Load ();
        stats = saveData.Stats.Load ();
        if (stats == null) stats = new StatList ();
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
    public List<EquipmentSlotSaveData> Equipment;
    public InventorySaveData Inventory;
    public StatListSaveData Stats;
    public string SectorId;
    public bool AIEnabled;
    public bool IsPlayer;
}
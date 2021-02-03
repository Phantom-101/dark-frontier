using System;
using System.Collections.Generic;
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
    [SerializeField] private Structure _target;

    [SerializeField] private Sector _sector;

    [SerializeField] private List<Structure> _docked;
    [SerializeField] private List<string> _dockedIds;

    private Rigidbody _rb;

    private void Awake () {

        if (transform.parent != null) {

            _sector = transform.parent.GetComponent<Sector> ();
            if (_sector != null) _sector.Entered (this);

        }

        _rb = GetComponent<Rigidbody> ();

    }

    private void Start () {

        if (StructureManager.GetInstance () != null) StructureManager.GetInstance ().AddStructure (this);

        if (string.IsNullOrEmpty (_id)) _id = Guid.NewGuid ().ToString ();

        if (_stats.Keys.Count == 0) {

            // Initialize structure stats

            _stats.Add ("sensor_strength", new StructureStat ("sensor_strength", _profile.SensorStrength));
            _stats.Add ("detectability", new StructureStat ("detectability", _profile.Detectability));
            _stats.Add ("docking_bay_size", new StructureStat ("docking_bay_size", _profile.DockingBaySize));
            _stats.Add ("damage_multiplier", new StructureStat ("damage_multiplier", 1));
            _stats.Add ("recharge_multiplier", new StructureStat ("recharge_multiplier", 1));
            _stats.Add ("speed_multiplier", new StructureStat ("speed_multiplier", 1));
            _stats.Add ("agility_multiplier", new StructureStat ("agility_multiplier", 1));

        }

        if (_initialFaction != null) _faction = FactionManager.GetInstance ().GetFaction (_initialFaction.Id);
        if (_initialAI != null) _ai = _initialAI.GetAI (this);

        if (_faction != null) _faction.AddProperty (this);

    }

    public StructureSO GetProfile () { return _profile; }

    public string GetId () { return _id; }

    public void SetId (string id) { _id = id; }

    public float GetHull () { return _hull; }

    public Faction GetFaction () { return _faction; }

    public void SetFaction (Faction faction) { _faction = faction; }

    public void ChangeHull (float amount) {

        _hull += amount;

        if (_hull <= 0) {

            if (_faction != null) _faction.RemoveProperty (this);
            _profile.OnDestroyedChannel.RaiseEvent (this);

        }

    }

    public void SetHull (float target) {

        _hull = target;

        if (_hull <= 0) {

            if (_faction != null) _faction.RemoveProperty (this);
            _profile.OnDestroyedChannel.RaiseEvent (this);

        }

    }

    public List<EquipmentSlot> GetEquipment () { return _equipmentSlots; }

    public List<T> GetEquipment<T> () where T : EquipmentSlot {

        List<T> equipment = new List<T> ();

        _equipmentSlots.ForEach (slot => {

            if (slot is T) equipment.Add (slot as T);

        });

        return equipment;

    }

    public ItemSOToIntDictionary GetInventory () { return _inventory; }

    public void SetInventory (ItemSOToIntDictionary inventory) { _inventory = inventory ?? new ItemSOToIntDictionary (); }

    public int GetInventoryCount (ItemSO item) { return _inventory.ContainsKey (item) ? _inventory[item] : 0; }

    public void SetInventoryCount (ItemSO item, int count) { _inventory[item] = count; }

    public void ChangeInventoryCount (ItemSO item, int delta) { SetInventoryCount (item, GetInventoryCount (item) + delta); }

    public bool HasInventoryCount (ItemSO item, int condition) { return GetInventoryCount (item) >= condition; }

    public float GetTotalInventorySize () { return _profile.InventorySize; }

    public float GetUsedInventorySize () {

        float used = 0;
        foreach (ItemSO item in _inventory.Keys) used += item.Size * _inventory[item];
        return used;

    }

    public float GetFreeInventorySize () { return GetTotalInventorySize () - GetUsedInventorySize (); }

    public bool CanAddInventoryItem (ItemSO item, int count) { return GetFreeInventorySize () >= item.Size * count; }

    public AI GetAI () { return _ai; }

    public void SetAI (AI controller) { _ai = controller; }

    public Structure GetTarget () { return _target; }

    public void SetTarget (Structure target) {

        _target = target;

        if (PlayerController.GetInstance ().GetPlayer () == this) PlayerController.GetInstance ().TargetChangedChannel.OnEventRaised ();

    }

    public Sector GetSector () { return _sector; }

    public void SetSector (Sector sector) { _sector = sector; }

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
        foreach (EquipmentSlot slot in docker.GetEquipment ()) slot.Deactivate ();

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
        if (_faction.IsEnemy (docker.GetFaction ())) return false;
        // Within range?
        if (NavigationManager.GetInstance ().GetLocalDistance (this, docker) > 50) return false;
        // Already children?
        if (_docked.Contains (docker)) return false;
        // Enough space?
        float size = 0;
        foreach (Structure c in _docked) size += c.GetProfile ().ApparentSize;
        if (docker.GetProfile ().ApparentSize > _stats["docking_bay_size"].GetAppliedValue () - size) return false;

        return true;

    }

    public bool IsDocked () { return GetDockedAt () != null; }

    public Structure GetDockedAt () { return transform.parent.GetComponent<Structure> (); }

    public bool CanDockTarget () { return _target != null && _target.CanAddDocker (this); }

    public bool CanUndock () { return IsDocked (); }

    public void DockTarget () { _target.AddDocker (this); }

    public bool Undock () {

        if (!CanUndock ()) return false;

        GetDockedAt ().RemoveDocker (this);
        return true;

    }

    public void Tick () {

        if (_ai == null) _ai = new AI (this);
        if (_aiEnabled) _ai.Tick ();

        foreach (EquipmentSlot slot in _equipmentSlots) slot.Tick ();

    }

    public void FixedTick () {

        foreach (EquipmentSlot slot in _equipmentSlots) slot.FixedTick ();

        // Set position and rotation
        transform.LeanSetLocalPosY (0);
        transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, _rb.angularVelocity.y * -25);

    }

    public void TakeDamage (float amount, Vector3 from) {

        ShieldSlot shield = GetEquipment<ShieldSlot> ()[0];
        float leftOver = amount;
        if (shield != null) {
            int sector = shield.GetStrengths ().GetSectorTo (from);
            float strength = shield.GetStrengths ().GetSectorStrength (sector);
            float shieldDmg = Mathf.Min (amount, strength);
            leftOver -= shieldDmg;
            shield.GetStrengths ().ChangeSectorStrength (sector, -shieldDmg);
        }
        ChangeHull (-leftOver);

    }

    public float GetAngleTo (Vector3 to) {

        Vector3 heading = to - transform.localPosition;
        return Vector3.SignedAngle (transform.forward, heading, transform.up);

    }

    public float GetElevationTo (Vector3 to) {

        Vector3 heading = to - transform.localPosition;
        return Vector3.SignedAngle (transform.forward, heading, -transform.right);

    }

    public StructureSaveData GetSaveData () {

        StructureSaveData data = new StructureSaveData {

            Name = gameObject.name,
            Position = transform.localPosition,
            Rotation = transform.localRotation,
            Hull = _hull

        };
        if (_profile != null) data.ProfileId = _profile.Id;
        if (_faction != null) data.FactionId = _faction.GetId ();
        _equipmentSlots.ForEach (slot => { data.Equipment.Add (slot.GetSaveData ()); });
        if (_sector != null) data.SectorId = _sector.GetId ();
        return data;

    }

    public void SetSaveData (StructureSaveData saveData) {

        gameObject.name = saveData.Name;
        transform.localPosition = saveData.Position;
        transform.localRotation = saveData.Rotation;
        _hull = saveData.Hull;
        _faction = FactionManager.GetInstance ().GetFaction (saveData.FactionId);
        for (int i = 0; i < _equipmentSlots.Count; i++) _equipmentSlots[i].LoadSaveData (saveData.Equipment[i]);
        _sector = SectorManager.GetInstance ().GetSector (saveData.SectorId);
        _sector.Entered (this);

    }

}

[Serializable]
public class StructureSaveData {

    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public string ProfileId;
    public string Id;
    public float Hull;
    public string FactionId;
    public List<EquipmentSlotSaveData> Equipment = new List<EquipmentSlotSaveData> ();
    public string SectorId;

}
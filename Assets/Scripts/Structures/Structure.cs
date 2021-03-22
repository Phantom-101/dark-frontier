using System;
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
    [SerializeField] private Structure _target;

    [SerializeField] private Sector _sector;

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
                Profile.OnDestroyedChannel.RaiseEvent (this);

            }

        }

    }
    public Faction Faction { get => _faction; set => _faction = value; }
    public List<EquipmentSlot> Equipment { get => _equipmentSlots; }
    public ItemSOToIntDictionary Inventory { get => _inventory; }
    public AI AI { get => _ai; set => _ai = value; }
    public Structure Target {

        get => _target;
        set {
            _target = value;
            if (PlayerController.GetInstance ().GetPlayer () == this) PlayerController.GetInstance ().TargetChangedChannel.OnEventRaised ();

        }

    }
    public Sector Sector { get => _sector; set => _sector = value; }

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

        if (StructureManager.GetInstance () != null) StructureManager.GetInstance ().AddStructure (this);

        if (string.IsNullOrEmpty (_id)) _id = Guid.NewGuid ().ToString ();

        if (_stats.Keys.Count == 0) {

            // Initialize structure stats

            _stats.Add ("sensor_strength", new StructureStat ("sensor_strength", _profile.SensorStrength));
            _stats.Add ("detectability", new StructureStat ("detectability", _profile.Detectability));
            _stats.Add ("docking_bay_size", new StructureStat ("docking_bay_size", _profile.DockingBaySize));
            _stats.Add ("damage_multiplier", new StructureStat ("damage_multiplier", 1));
            _stats.Add ("recharge_multiplier", new StructureStat ("recharge_multiplier", 1));
            _stats.Add ("linear_speed_multiplier", new StructureStat ("linear_speed_multiplier", 1));
            _stats.Add ("angular_speed_multiplier", new StructureStat ("angular_speed_multiplier", 1));

        }

        if (_initialFaction != null) _faction = FactionManager.GetInstance ().GetFaction (_initialFaction.Id);
        if (_initialAI != null) _ai = _initialAI.GetAI (this);

        if (_faction != null) _faction.AddProperty (this);

    }

    public float GetStatBaseValue (string name) {

        return _stats[name].GetBaseValue ();

    }

    public float GetStatAppliedValue (string name) {

        return _stats[name].GetAppliedValue ();

    }

    public void AddStatModifier (StructureStatModifier modifier) {

        _stats[modifier.GetTarget ()].AddModifier (modifier);

    }

    public void RemoveStatModifier (StructureStatModifier modifier) {

        _stats[modifier.GetTarget ()].GetModifiers ().RemoveAll (m => m.GetName () == modifier.GetName ());

    }

    public List<T> GetEquipment<T> () where T : EquipmentSlot {

        List<T> equipment = new List<T> ();

        _equipmentSlots.ForEach (slot => {

            if (slot is T) equipment.Add (slot as T);

        });

        return equipment;

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
        foreach (EquipmentSlot slot in docker.Equipment) slot.TargetState = false;

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
        if (docker.Profile.ApparentSize > _stats["docking_bay_size"].GetAppliedValue () - size) return false;

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

        if (_profile.SnapToPlane) {

            // Set position and rotation
            transform.LeanSetLocalPosY (0);
            transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, _rb.angularVelocity.y * -25);

        }

    }

    public void TakeDamage (DamageProfile damage, Vector3 from) {

        bool good = false;
        Vector3 point = from;
        RaycastHit hit;

        do {

            Physics.Raycast (point, transform.position - point, out hit);
            if (hit.transform == transform) good = true;
            point = hit.point;

        } while (!good);

        List<ShieldSlot> shields = GetEquipment<ShieldSlot> ();
        ShieldSlot damaged = null;
        float maxInf = 0;
        foreach (ShieldSlot shield in shields) {

            if (shield.Shield != null) {

                float inf = 1 - (Vector3.Distance (shield.transform.position, point) / shield.Shield.Radius);
                if (inf >= maxInf) {

                    maxInf = inf;
                    damaged = shield;

                }

            }

        }
        if (damaged == null) Hull -= damage.DamageAmount * damage.HullEffectiveness;
        else {

            float strength = damaged.Strength;
            float maxStrength = damaged.Shield.MaxStrength;
            if (strength <= damage.ShieldBypass * maxStrength) Hull -= damage.DamageAmount * damage.HullEffectiveness;
            else {

                Hull -= damage.DamageAmount * damage.ShieldPenetration * damage.HullEffectiveness;
                float toShield = damage.DamageAmount * (1 - damage.ShieldPenetration);
                float remain = toShield - (strength / damage.ShieldEffectiveness);
                if (remain < 0) damaged.Strength -= toShield * damage.ShieldEffectiveness;
                else {

                    damaged.Strength = 0;
                    Hull -= remain;

                }

            }

        }

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
            Hull = _hull,
            InventoryIds = _inventory.Keys.Select (item => item.Id).ToList (),
            InventoryCounts = _inventory.Values.ToList ()

        };
        if (_profile != null) data.ProfileId = _profile.Id;
        if (_faction != null) data.FactionId = _faction.GetId ();
        _equipmentSlots.ForEach (slot => { data.Equipment.Add (slot.GetSaveData ()); });
        if (_sector != null) data.SectorId = _sector.GetId ();
        data.AIEnabled = _aiEnabled;
        if (PlayerController.GetInstance ().GetPlayer () == this) data.IsPlayer = true;
        return data;

    }

    public void SetSaveData (StructureSaveData saveData) {

        gameObject.name = saveData.Name;
        transform.localPosition = saveData.Position;
        transform.localRotation = saveData.Rotation;
        _hull = saveData.Hull;
        _faction = FactionManager.GetInstance ().GetFaction (saveData.FactionId);
        for (int i = 0; i < _equipmentSlots.Count; i++) _equipmentSlots[i].LoadSaveData (saveData.Equipment[i]);
        ItemManager im = ItemManager.GetInstance ();
        for (int i = 0; i < saveData.InventoryIds.Count; i++) {

            _inventory[im.GetItem (saveData.InventoryIds[i])] = saveData.InventoryCounts[i];

        }
        _sector = SectorManager.GetInstance ().GetSector (saveData.SectorId);
        _sector.Entered (this);
        _aiEnabled = saveData.AIEnabled;

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
    public List<string> InventoryIds = new List<string> ();
    public List<int> InventoryCounts = new List<int> ();
    public string SectorId;
    public bool AIEnabled;
    public bool IsPlayer;

}
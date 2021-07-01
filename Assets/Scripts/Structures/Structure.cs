using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Structure : MonoBehaviour {
    [SerializeField] private StructureSO _profile;
    [SerializeField] private string _id;

    [SerializeField] private StatTypeToStatDictionary _stats = new StatTypeToStatDictionary ();
    [SerializeField] private float _hull;
    [SerializeReference, Expandable] private Faction _faction;

    [SerializeField] private List<EquipmentSlot> _equipmentSlots = new List<EquipmentSlot> ();
    [SerializeField] private Inventory _inventory;

    [SerializeReference, Expandable] private AI _ai;
    [SerializeField] private bool _aiEnabled;

    [SerializeField] private Structure _selected;
    [SerializeField] private Dictionary<Structure, float> _locks = new Dictionary<Structure, float> ();

    [SerializeField] private Sector _sector;
    [SerializeField] private List<Structure> _detected;

    [SerializeField] private List<Structure> _docked;

    [SerializeField] private bool _initialized;

    private Rigidbody _rb;

    public StructureSO Profile { get => _profile; }
    public string Id { get => _id; }
    public float Hull {
        get => _hull;
        set {
            _hull = Mathf.Min (value, GetProfileValue (StatType.MaxHull, 1));
            if (_hull <= 0) {
                if (Faction != null) Faction.RemoveProperty (this);
                if (Sector != null) Sector.Exited (this);
                StructureManager.Instance.OnStructureDestroyed (this);
            }
        }
    }
    public Faction Faction { get => _faction; set => _faction = value; }
    public List<EquipmentSlot> Equipment { get => _equipmentSlots; }
    public Inventory Inventory { get => _inventory; }
    public AI AI { get => _ai; set => _ai = value; }
    public Structure Selected {
        get => _selected;
        set {
            _selected = value;
            if (PlayerController.Instance.Player == this) PlayerController.Instance.SelectedChanged?.Invoke (this, EventArgs.Empty);
        }
    }
    public Dictionary<Structure, float> Locks { get => _locks; set => _locks = value; }
    public Sector Sector { get => _sector; set => _sector = value; }
    public List<Structure> Detected { get { if (_detected == null) _detected = StructureManager.Instance.GetDetected (this); return _detected; } set => _detected = value; }

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
        else _ai = _ai.GetAI ();

        if (_faction != null) {
            FactionManager.Instance.AddFaction (_faction);
            _faction.AddProperty (this);
        }

        if (_inventory == null) _inventory = new Inventory (GetStat (StatType.InventoryVolume, 0), 1);
    }

    public float GetProfileValue (StatType type, float value) {
        if (_profile == null || _profile.Stats == null) return value;
        if (_profile.Stats.ContainsKey (type)) return _profile.Stats[type].BaseValue;
        return value;
    }

    public Stat GetStat (StatType type, float value) {
        _stats.TryAdd (type, new Stat { Name = type.ToString (), BaseValue = value });
        return _stats[type];
    }

    public float GetStatBaseValue (StatType type, float value) {
        _stats.TryAdd (type, new Stat { Name = type.ToString (), BaseValue = value });
        return _stats[type].BaseValue;
    }

    public float GetStatAppliedValue (StatType type, float value) {
        _stats.TryAdd (type, new Stat { Name = type.ToString (), BaseValue = value });
        return _stats[type].AppliedValue;
    }

    public void AddStatModifier (StatType type, StatModifier modifier) {
        _stats.TryGet (type, null)?.AddModifier (modifier);
    }

    public void RemoveStatModifier (StatType type, StatModifier modifier) {
        _stats.TryGet (type, null)?.RemoveModifier (modifier);
    }

    public void RemoveStatModifier (StatType type, string id) {
        _stats.TryGet (type, null)?.RemoveModifier (id);
    }

    public List<T> GetEquipmentData<T> () where T : EquipmentSlotData {
        List<T> data = new List<T> ();
        _equipmentSlots.ForEach (slot => {
            if (slot.Data is T) data.Add (slot.Data as T);
        });
        return data;
    }
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

        if (docker == PlayerController.Instance.Player) {
            // Send notification
            NotificationUI.GetInstance ().AddNotification ("Docked at " + gameObject.name);

            // Update UI
            //UIStateManager.Instance.AddState (UIState.Docked);

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

            if (docker == PlayerController.Instance.Player) {
                // Send notification
                NotificationUI.GetInstance ().AddNotification ("Undocked from " + gameObject.name);

                // Update UI
                UIStateManager.Instance.RemoveState ();

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
        if (NavigationManager.Instance.GetLocalDistance (this, docker) > 50) return false;
        // Already children?
        if (_docked.Contains (docker)) return false;
        // Enough space?
        float size = 0;
        foreach (Structure c in _docked) size += c.Profile.ApparentSize;
        if (docker.Profile.ApparentSize > GetStatAppliedValue (StatType.DockingBaySize, 0) - size) return false;

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
            if (target == null || !Detected.Contains (target) || Locks.Keys.Count > GetStatAppliedValue (StatType.MaxLocks, 0)) {
                Locks.Remove (target);
                lockChanged = true;
            }
        }
        if (lockChanged) {
            PlayerController pc = PlayerController.Instance;
            if (pc.Player == this) pc.LocksChanged.Invoke (this, EventArgs.Empty);
        }
    }

    public bool Lock (Structure target) {
        if (target == null) return false;
        if (!Detected.Contains (target)) return false;
        if (Locks.ContainsKey (target)) return false;
        if (Locks.Keys.Count >= GetStatAppliedValue (StatType.MaxLocks, 0)) return false;
        Locks[target] = 0;
        PlayerController pc = PlayerController.Instance;
        if (pc.Player == this) pc.LocksChanged.Invoke (this, EventArgs.Empty);
        return true;
    }

    public bool Unlock (Structure target) {
        if (target == null) return false;
        if (!Locks.ContainsKey (target)) return false;
        Locks.Remove (target);
        PlayerController pc = PlayerController.Instance;
        if (pc.Player == this) pc.LocksChanged.Invoke (this, EventArgs.Empty);
        return true;
    }

    public void TickLocks () {
        foreach (Structure target in Locks.Keys.ToArray ()) {
            float progress = GetStatAppliedValue (StatType.ScannerStrength, 0) * target.GetStatAppliedValue (StatType.SignatureSize, 0) * Time.deltaTime;
            Locks[target] = Mathf.Min (Locks[target] + progress, 100);
        }
    }

    public void Tick () {
        ManageSelectedAndLocks ();
        TickLocks ();

        if (_aiEnabled) _ai.Tick (this);

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
        if (_profile != null && _profile.Stats != null) {
            foreach (StatType key in _profile.Stats.Keys) {
                _stats.TryAdd (key, new Stat { Name = key.ToString (), BaseValue = _profile.Stats[key].BaseValue });
            }
        }
    }

    public StructureSaveData GetSaveData () {
        StructureSaveData data = new StructureSaveData {
            Name = gameObject.name,
            Position = new float[] { transform.localPosition.x, transform.localPosition.y, transform.localPosition.z },
            Rotation = new float[] { transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w },
            Hull = _hull,
            Inventory = _inventory.Save (),
            Stats = _stats,
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
        _hull = saveData.Hull;
        _faction = FactionManager.Instance.GetFaction (saveData.FactionId);
        for (int i = 0; i < _equipmentSlots.Count; i++) {
            _equipmentSlots[i].Data = saveData.Equipment[i].Load ();
            _equipmentSlots[i].Data.Slot = _equipmentSlots[i];
        }
        _inventory = saveData.Inventory.Load ();
        _stats = saveData.Stats;
        if (_stats == null) _stats = new StatTypeToStatDictionary ();
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
    public StatTypeToStatDictionary Stats;
    public string SectorId;
    public bool AIEnabled;
    public bool IsPlayer;
}
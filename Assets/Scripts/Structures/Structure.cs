using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Structure : BehaviorBase {
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
                structureManager.DestroyStructure (this);
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

    public HashSet<Structure> Detected { get => detected ?? (detected = structureManager.GetDetected (this)); }
    [SerializeField] private HashSet<Structure> detected;

    public DockingBayList DockingBays { get => dockingBays; }
    [SerializeReference] private DockingBayList dockingBays;

    public Structure Dockee { get => dockee; }
    [SerializeField] private Structure dockee;
    public bool IsDocked { get => dockee != null; }

    private new Rigidbody rigidbody;

    private StructureRegistry registry;
    private StructureManager structureManager;

    [Inject]
    public void Construct (StructureRegistry registry, StructureManager structureManager) {
        this.registry = registry;
        this.structureManager = structureManager;
    }

    protected override void SingleInitialize () {
        rigidbody = GetComponent<Rigidbody> ();

        registry.Add (this);
        structureManager.TryManage (this);

        if (transform.parent != null) {
            _sector = GetComponentInParent<Sector> ();
            if (_sector != null) _sector.Entered (this);
        }

        if (_faction != null) {
            FactionManager.Instance.AddFaction (_faction);
            _faction.AddProperty (this);
        }

        EnsureStats ();

        if (_inventory == null) _inventory = new Inventory (stats.GetAppliedValue (StatNames.InventoryVolume, 0), 1);

        if (_ai == null) _ai = ScriptableObject.CreateInstance<AI> ();
        else _ai = _ai.Copy ();

        StructureInventoryAdder adder = GetComponent<StructureInventoryAdder> ();
        if (adder != null) adder.Run (this);
    }

    protected override void MultiInitialize () {
        SynchronizeInventoryVolume (this, EventArgs.Empty);
        // Stop slots from ticking themselves
        foreach (EquipmentSlot slot in _equipmentSlots) slot.Manager = manager;
    }

    protected override void InternalTick (float dt) {
        ManageSelectedAndLocks ();
        TickLocks ();

        foreach (EquipmentSlot slot in _equipmentSlots) slot.Tick (dt);

        stats.Tick (dt);
    }

    protected override void InternalExpensiveTick (float dt) {
        detected = null;

        if (_aiEnabled) _ai.Tick (this, dt);
    }

    protected override void InternalFixedTick (float dt) {
        foreach (EquipmentSlot slot in _equipmentSlots) slot.FixedTick (dt);

        if (_profile.SnapToPlane) {
            // Set position and rotation
            transform.LeanSetLocalPosY (0);
            transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, rigidbody.angularVelocity.y * -25);
        }
    }

    public override bool Validate () {
        // Should have dependencies
        if (registry == null) return false;
        if (structureManager == null) return false;
        // Should have profile
        if (_profile == null) return false;
        // Should have parent
        if (transform.parent == null) return false;
        // Should have valid id
        if (string.IsNullOrEmpty (_id)) _id = Guid.NewGuid ().ToString ();
        // Should have sector in parent tree
        if (_sector == null) {
            _sector = GetComponentInParent<Sector> ();
            if (_sector == null) return false;
            _sector.Entered (this);
        }
        // Should have a manager
        if (manager == null) {
            structureManager.TryManage (this);
        }
        // Should have stats
        EnsureStats ();
        // Should have inventory
        if (_inventory == null) _inventory = new Inventory (stats.GetAppliedValue (StatNames.InventoryVolume, 0), 1);
        // Should have AI
        if (_ai == null) _ai = ScriptableObject.CreateInstance<AI> ();
        // Should have docking bays
        if (dockingBays == null) dockingBays = _profile.DockingBays.Copy (this);
        return true;
    }

    protected override void SubscribeEventListeners () {
        stats.GetStat (StatNames.InventoryVolume, 0).OnValueChanged += SynchronizeInventoryVolume;
    }

    protected override void UnsubscribeEventListeners () {
        stats.GetStat (StatNames.InventoryVolume, 0).OnValueChanged -= SynchronizeInventoryVolume;
    }

    private void SynchronizeInventoryVolume (object sender, EventArgs args) => _inventory.Volume = stats.GetAppliedValue (StatNames.InventoryVolume, 0);

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

    private void ManageSelectedAndLocks () {
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

    private void TickLocks () {
        foreach (Structure target in Locks.Keys.ToArray ()) {
            float progress = stats.GetAppliedValue (StatNames.ScannerStrength, 0) * target.Stats.GetAppliedValue (StatNames.SignatureSize, 0) * Time.deltaTime;
            Locks[target] = Mathf.Min (Locks[target] + progress, 100);
        }
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
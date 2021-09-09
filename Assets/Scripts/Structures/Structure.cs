using DarkFrontier.AI;
using DarkFrontier.Equipment;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkFrontier.Structures {
    public class Structure : ComponentBehavior {
        public StructureSO Profile { get => _profile; }
        [SerializeField] private StructureSO _profile;

        public Id Id { get => id; }
        [SerializeField] private Id id = new Id ();

        public StatList Stats { get => stats; }
        [SerializeField] private StatList stats = new StatList ();

        public float Hull {
            get => hull;
            set {
                hull = Mathf.Min (value, stats.GetBaseValue (StatNames.MaxHull, 1));
                if (hull <= 0) {
                    Faction.Value?.Property.Remove (this);
                    if (Sector.Value != null) Sector.Value.Population.Remove (this);
                    structureManager.DestroyStructure (this);
                }
            }
        }
        [SerializeField] private float hull;

        public FactionGetter Faction { get => faction; }
        [SerializeField] private FactionGetter faction = new FactionGetter ();

        public List<EquipmentSlot> Equipment { get => equipmentSlots; }
        [SerializeField] private List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot> ();

        public Inventory Inventory { get => _inventory; }
        [SerializeReference] private Inventory _inventory;

        public AIBase AI {
            get => _ai;
            set => _ai = value;
        }
        [SerializeReference, Expandable] private AIBase _ai;
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

        public SectorGetter Sector { get => sector; }
        [SerializeField] private SectorGetter sector = new SectorGetter ();

        public HashSet<Structure> Detected { get => detected ?? (detected = structureManager.GetDetected (this)); }
        [SerializeField] private HashSet<Structure> detected;

        public DockingBayList DockingBays { get => dockingBays; }
        [SerializeReference] private DockingBayList dockingBays;

        public Structure Dockee { get => dockee; }
        [SerializeField] private Structure dockee;
        public bool IsDocked { get => dockee != null; }

        private new Rigidbody rigidbody;

        private SectorManager sectorManager;
        private FactionManager factionManager;
        private StructureManager structureManager;

        protected override void SingleInitialize () {
            rigidbody = GetComponent<Rigidbody> ();

            Sector sector = GetComponentInParent<Sector> ();
            if (sector != null) {
                Sector.Id.Value = sector.Id;
                if (Sector.Value != null) Sector.Value.Population.Set (this);
            }

            EnsureStats ();

            if (_inventory == null) _inventory = new Inventory (stats.GetAppliedValue (StatNames.InventoryVolume, 0), 1);

            if (_ai == null) _ai = ScriptableObject.CreateInstance<AIBase> ();
            else _ai = _ai.Copy ();

            StructureInventoryAdder adder = GetComponent<StructureInventoryAdder> ();
            if (adder != null) adder.Run (this);
        }

        protected override void MultiInitialize () {
            structureManager.Registry.Set (this);
            if (Faction.Value != null) {
                factionManager.Registry.Set (Faction.Value);
                Faction.Value.Property.Set (this);
            }

            canTickSelf = false;
            equipmentSlots.ForEach (e => e.CanTickSelf = false);

            SynchronizeInventoryVolume (this, EventArgs.Empty);
        }

        public override void GetServices () {
            sectorManager = Singletons.Get<SectorManager> ();
            factionManager = Singletons.Get<FactionManager> ();
            structureManager = Singletons.Get<StructureManager> ();
        }

        protected override void InternalTick (float dt) {
            // Expensive
            detected = null;
            if (_aiEnabled) _ai.Tick (this, dt);

            ManageSelectedAndLocks ();
            TickLocks ();
            stats.Tick (dt);
        }

        protected override void PropagateTick (float dt) {
            equipmentSlots.ForEach (e => e.Tick (dt));
        }

        protected override void InternalFixedTick (float dt) {
            if (_profile.SnapToPlane) {
                // Set position and rotation
                transform.LeanSetLocalPosY (0);
                transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, rigidbody.angularVelocity.y * -25);
            }
        }

        protected override void PropagateFixedTick (float dt) {
            equipmentSlots.ForEach (e => e.FixedTick (dt));
        }

        public override bool Validate () {
            // Should have dependencies
            if (sectorManager == null) return false;
            if (factionManager == null) return false;
            if (structureManager == null) return false;
            // Should have profile
            if (_profile == null) return false;
            // Should have parent
            if (transform.parent == null) return false;
            // Should have sector in parent tree
            if (Sector.Value == null) {
                Sector.Id.Value = GetComponentInParent<Sector> ().Id;
                if (Sector.Value == null) return false;
            }
            // Sector should have structure in population
            Sector.Value.Population.Set (this);
            // Should have stats
            EnsureStats ();
            // Should have inventory
            if (_inventory == null) _inventory = new Inventory (stats.GetAppliedValue (StatNames.InventoryVolume, 0), 1);
            // Should have AI
            if (_ai == null) _ai = ScriptableObject.CreateInstance<AIBase> ();
            // Should have docking bays
            if (dockingBays == null) dockingBays = new DockingBayList (_profile.DockingBays, this);
            return true;
        }

        protected override void InternalSubscribeEventListeners () {
            stats.GetStat (StatNames.InventoryVolume, 0).OnValueChanged += SynchronizeInventoryVolume;
        }

        protected override void InternalUnsubscribeEventListeners () {
            stats.GetStat (StatNames.InventoryVolume, 0).OnValueChanged -= SynchronizeInventoryVolume;
        }

        private void SynchronizeInventoryVolume (object sender, EventArgs args) => _inventory.Volume = stats.GetAppliedValue (StatNames.InventoryVolume, 0);

        public List<EquipmentSlot> GetEquipmentSlots<T> () where T : EquipmentPrototype.State {
            List<EquipmentSlot> slots = new List<EquipmentSlot> ();
            equipmentSlots.ForEach (slot => {
                if (slot.State is T) slots.Add (slot);
            });
            return slots;
        }

        public List<T> GetEquipmentStates<T> () where T : EquipmentPrototype.State {
            List<T> states = new List<T> ();
            equipmentSlots.ForEach (slot => {
                if (slot.State is T) states.Add (slot.State as T);
            });
            return states;
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
            ShieldPrototype.State closest = null;
            GetEquipmentStates<ShieldPrototype.State> ().ForEach (state => {
                if (closest == null) closest = state;
                else {
                    float disA = (from - closest.Slot.transform.position).sqrMagnitude;
                    float disB = (state.Slot.transform.position - closest.Slot.transform.position).sqrMagnitude;
                    if (disB < disA) closest = state;
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
                    if (!stats.HasStat (stat)) {
                        stats.AddStat (stat.Copy ());
                    }
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
            data.FactionId = Faction.Id.Value;
            equipmentSlots.ForEach (slot => { data.Equipment.Add (slot.ToSerializable ()); });
            if (Sector.Value != null) data.SectorId = Sector.Value.Id;
            data.AIEnabled = _aiEnabled;
            if (PlayerController.Instance.Player == this) data.IsPlayer = true;
            return data;
        }

        public void SetSaveData (StructureSaveData saveData) {
            gameObject.name = saveData.Name;
            transform.localPosition = new Vector3 (saveData.Position[0], saveData.Position[1], saveData.Position[2]);
            transform.localRotation = new Quaternion (saveData.Rotation[0], saveData.Rotation[1], saveData.Rotation[2], saveData.Rotation[3]);
            hull = saveData.Hull;
            Faction.Id.Value = saveData.FactionId;
            for (int i = 0; i < equipmentSlots.Count; i++) {
                equipmentSlots[i].FromSerializable (saveData.Equipment[i]);
            }
            _inventory = saveData.Inventory.Load ();
            stats = saveData.Stats.Load ();
            if (stats == null) stats = new StatList ();
            EnsureStats ();
            Sector.Id.Value = saveData.SectorId;
            Sector.Value.Population.Set (this);
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
        public List<EquipmentSlot.Serializable> Equipment;
        public InventorySaveData Inventory;
        public StatListSaveData Stats;
        public string SectorId;
        public bool AIEnabled;
        public bool IsPlayer;
    }
}
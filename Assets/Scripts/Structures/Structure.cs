using DarkFrontier.Factions;
using DarkFrontier.Foundation;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Extensions;
using DarkFrontier.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

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
                    if (Faction.Value (factionManager.Registry.Find) != null) Faction.Cached.Property.Remove (this);
                    if (Sector != null) Sector.Value (sectorManager.Registry.Find).Exited (this);
                    structureManager.DestroyStructure (this);
                }
            }
        }
        [SerializeField] private float hull;

        public FactionRetriever Faction { get => faction; }
        [SerializeField] private FactionRetriever faction = new FactionRetriever ();

        public List<EquipmentSlot> Equipment { get => equipmentSlots; }
        [SerializeField] private List<EquipmentSlot> equipmentSlots = new List<EquipmentSlot> ();

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

        public SectorRetriever Sector { get => sector; }
        [SerializeField] private SectorRetriever sector = new SectorRetriever ();

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

        [Inject]
        public void Construct (SectorManager sectorManager, FactionManager factionManager, StructureManager structureManager) {
            this.sectorManager = sectorManager;
            this.factionManager = factionManager;
            this.structureManager = structureManager;
        }

        protected override void SingleInitialize () {
            rigidbody = GetComponent<Rigidbody> ();

            if (transform.parent != null) {
                Sector.Id.Value = GetComponentInParent<Sector> ().Id;
                if (Sector.Value (sectorManager.Registry.Find) != null) Sector.Cached.Entered (this);
            }

            EnsureStats ();

            if (_inventory == null) _inventory = new Inventory (stats.GetAppliedValue (StatNames.InventoryVolume, 0), 1);

            if (_ai == null) _ai = ScriptableObject.CreateInstance<AI> ();
            else _ai = _ai.Copy ();

            StructureInventoryAdder adder = GetComponent<StructureInventoryAdder> ();
            if (adder != null) adder.Run (this);
        }

        protected override void MultiInitialize () {
            // Add to registries
            structureManager.Registry.Add (this);
            if (Faction.Value (factionManager.Registry.Find) != null) {
                factionManager.Registry.Add (Faction.Cached);
                Faction.Cached.Property.Add (this);
            }
            // Do not tick self
            canTickSelf = false;
            // Sync inventory volume according to stats
            SynchronizeInventoryVolume (this, EventArgs.Empty);
            // Stop slots from ticking themselves
            equipmentSlots.ForEach (e => e.CanTickSelf = false);
        }

        protected override void InternalTick (float dt) {
            ManageSelectedAndLocks ();
            TickLocks ();
            stats.Tick (dt);
        }

        protected override void InternalExpensiveTick (float dt) {
            detected = null;
            if (_aiEnabled) _ai.Tick (this, dt);
        }

        protected override void PropagateTick (float dt, float? edt = null) {
            equipmentSlots.ForEach (e => e.Tick (dt, edt));
        }

        protected override void InternalFixedTick (float dt) {
            if (_profile.SnapToPlane) {
                // Set position and rotation
                transform.LeanSetLocalPosY (0);
                transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, rigidbody.angularVelocity.y * -25);
            }
        }

        protected override void PropagateFixedTick (float dt, float? edt = null) {
            equipmentSlots.ForEach (e => e.FixedTick (dt, edt));
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
            Sector.Value (sectorManager.Registry.Find);
            if (Sector.Cached == null) {
                Sector.Id.Value = GetComponentInParent<Sector> ().Id;
                if (Sector.Value (sectorManager.Registry.Find) == null) return false;
                Sector.Cached.Entered (this);
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

        protected override void InternalSubscribeEventListeners () {
            stats.GetStat (StatNames.InventoryVolume, 0).OnValueChanged += SynchronizeInventoryVolume;
        }

        protected override void InternalUnsubscribeEventListeners () {
            stats.GetStat (StatNames.InventoryVolume, 0).OnValueChanged -= SynchronizeInventoryVolume;
        }

        private void SynchronizeInventoryVolume (object sender, EventArgs args) => _inventory.Volume = stats.GetAppliedValue (StatNames.InventoryVolume, 0);

        public List<T> GetEquipmentData<T> () where T : EquipmentSlotData {
            List<T> data = new List<T> ();
            equipmentSlots.ForEach (slot => {
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
            equipmentSlots.ForEach (slot => { data.Equipment.Add (slot.Data.Save ()); });
            if (Sector.Value (sectorManager.Registry.Find) != null) data.SectorId = Sector.Cached.Id;
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
                equipmentSlots[i].Data = saveData.Equipment[i].Load ();
                equipmentSlots[i].Data.Slot = equipmentSlots[i];
            }
            _inventory = saveData.Inventory.Load ();
            stats = saveData.Stats.Load ();
            if (stats == null) stats = new StatList ();
            EnsureStats ();
            Sector.Id.Value = saveData.SectorId;
            Sector.Value (sectorManager.Registry.Find).Entered (this);
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
}
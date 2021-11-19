using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Camera;
using DarkFrontier.Controllers;
using DarkFrontier.Equipment;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Events;
using DarkFrontier.Foundation.Identification;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items;
using DarkFrontier.Items.Inventories;
using DarkFrontier.Items.Prototypes;
using DarkFrontier.Locations;
using DarkFrontier.UI.Indicators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Structures {
    public class Structure : ComponentBehavior, ICtorArgs<Structure.State> {
        [SerializeField] private State iState = new State (null);

        public StructurePrototype? UPrototype => iState.uPrototype;
        public string UId => iState.uId.uId;

        public FactionGetter UFaction => iState.uFaction;
        public SectorGetter USector => iState.uSector;

        public float UHull {
            get => iState.uHull;

            set {
                iState.uHull = Mathf.Min (value, iState.uStats.UValues.MaxHull);
                if (iState.uHull <= 0) {
                    iStructureManager.Value.DestroyStructure (this);
                }
            }
        }
        public State.Stats UStats => iState.uStats;

        public Inventory UInventory => iState.uInventory;
        public Equipment UEquipment;
        public List<DockingPoint> UDockingPoints => iState.uDockingPoints;
        public StructureGetter UDockedAt => iState.uDockedAt;
        public bool UIsDocked => iState.uDockedAt.UValue != null;

        public StructureGetter USelected => iState.uSelected;
        public Dictionary<StructureGetter, float> ULocks => iState.uLocks;

        public NpcController? UNpcController { get => iState.uNpcController; set => iState.uNpcController = value; }

        private Rigidbody? iRigidbody;

        private readonly Lazy<FactionManager> iFactionManager = new Lazy<FactionManager>(() => Singletons.Get<FactionManager>(), false);
        private readonly Lazy<StructureManager> iStructureManager = new Lazy<StructureManager>(() => Singletons.Get<StructureManager>(), false);
        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController> (), false);

        private bool iEnabled;

        public void Construct (State aState) => iState = aState;
        
        public override void Initialize () {
            if (iRigidbody == null) {
                iRigidbody = GetComponent<Rigidbody> ();
            }

            iState.uStats.Recalculate (iState.uPrototype);

            iState.uInventory.Volume = iState.uStats.UValues.InventoryVolume;
            
            if (iState.uEquipment.Count == 0) {
                iState.uEquipment.AddRange (GetComponentsInChildren<EquipmentSlot> ());
            } else {
                iState.uEquipment.RemoveAll (aSlot => aSlot == null);
            }
            
            UEquipment = new Equipment(iState);
            
            if (iState.uDockingPoints.Count == 0) {
                iState.uDockingPoints.AddRange (GetComponentsInChildren<DockingPoint> ());
            } else {
                iState.uDockingPoints.RemoveAll (aPoint => aPoint == null);
            }

            UNpcController = GetComponent<NpcController>();
            
            StructureInventoryAdder lInventoryAdder = GetComponent<StructureInventoryAdder> ();
            if (lInventoryAdder != null) {
                lInventoryAdder.Run (this);
            }
        }

        public override void Enable () {
            iEnabled = true;

            iStructureManager.Value.Registry.Set (this);
            if (iState.uFaction.UValue != null) {
                iFactionManager.Value.Registry.Set (iState.uFaction.UValue);
                iState.uFaction.UValue.Property.Set (this);
            }
            if (iState.uSector.UValue == null) {
                Sector lSector = GetComponentInParent<Sector> ();
                if (lSector != null) {
                    iState.uSector.UId.Value = lSector.UId.uId;
                }
            }
            if (iState.uSector.UValue != null) {
                iState.uSector.UValue.UPopulation.Set (this);
            }

            iState.Enable ();
        }

        public override void Disable () {
            iEnabled = false;

            iStructureManager.Value.Registry.Remove (iState.uId.uId);
            iState.uFaction.UValue?.Property.Remove (iState.uId.uId);
            if (iState.uSector.UValue != null) {
                iState.uSector.UValue.UPopulation.Remove (iState.uId.uId);
            }

            iState.Disable ();
        }

        public override void Tick (object aTicker, float aDt) {
            if (!iEnabled) return;

            iState.uStats.Recalculate (iState.uPrototype);
            iState.uStats.Tick (this, aDt);

            if (iState.uNpcController != null) {
                iState.uNpcController.Tick (this, aDt);
            }

            ManageSelectedAndLocks ();
            TickLocks ();

            var lEquipmentSlotsLength = iState.uEquipment.Count;
            for (var lIndex = 0; lIndex < lEquipmentSlotsLength; lIndex++) {
                iState.uEquipment[lIndex].Tick(this, aDt);
            }
        }

        public override void FixedTick (object aTicker, float aDt) {
            if (iRigidbody != null) {
                if (iState.uPrototype?.SnapToPlane ?? false) {
                    transform.LeanSetLocalPosY (0);
                    transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, iRigidbody.angularVelocity.y * -25);
                }
            }

            var lEquipmentSlotsLength = iState.uEquipment.Count;
            for (var lIndex = 0; lIndex < lEquipmentSlotsLength; lIndex++) {
                iState.uEquipment[lIndex].FixedTick(this, aDt);
            }
        }

        public NpcController CreateNpcController<T>() where T : NpcController {
            if (UNpcController != null) {
                Destroy(UNpcController);
            }

            return UNpcController = gameObject.AddComponent<T>();
        }

        public NpcController GetNpcController<T>() where T : NpcController {
            if (UNpcController is T) {
                return UNpcController;
            }

            return CreateNpcController<T>();
        }

        public bool IsLocked(Structure aTarget) {
            var lQuery = new StructureGetter();
            lQuery.UId.Value = aTarget.UId;
            return ULocks.ContainsKey(lQuery);
        }
        
        public void OnDocked (Structure aDockedAt, DockingPoint aDockingPoint) {
            // Cache dockee
            iState.uDockedAt.UId.Value = aDockedAt.UId;
            // Add as child
            transform.parent = aDockingPoint.transform;
            // Set kinematic
            if (iRigidbody != null) {
                iRigidbody.isKinematic = true;
            }
            // Set position
            transform.localPosition = Vector3.zero;
            // TODO Disable all equipment
            //foreach (NewEquipmentSlot slot in docker.Equipment) slot.TargetState = false;
            if (this == iPlayerController.Value.UPlayer) {
                // Send notification
                NotificationUI.GetInstance ().AddNotification ("Docked at " + aDockedAt.name);
                // TODO Update UI
                //UIStateManager.Instance.AddState (UIState.Docked);
                // Update camera anchor
                CameraController.GetInstance ().SetAnchor (new Location (aDockedAt.transform));
            }
        }

        public void OnUndocked (Structure aDockedAt) {
            // Remove dockee
            iState.uDockedAt.UId.Value = "";
            // Remove as child
            transform.parent = aDockedAt.transform.parent;
            // Set kinematic
            if (iRigidbody != null) {
                iRigidbody.isKinematic = false;
            }
            if (this == iPlayerController.Value.UPlayer) {
                // Send notification
                NotificationUI.GetInstance ().AddNotification ("Undocked from " + aDockedAt.name);
                // TODO Update UI
                //UIStateManager.Instance.RemoveState ();
                // Update camera anchor
                CameraController.GetInstance ().RemoveAnchor ();
            }
        }

        public bool CanAccept (Structure aDocker) {
            return iState.uDockingPoints.Any (aPoint => aPoint.CanAccept (aDocker));
        }

        public bool TryAccept (Structure aDocker) {
            return iState.uDockingPoints.Any (aPoint => aPoint.TryAccept (aDocker));
        }

        public bool CanRelease (Structure aDocker) {
            return iState.uDockingPoints.Any (aPoint => aPoint.CanRelease (aDocker));
        }

        public bool TryRelease (Structure aDocker) {
            return iState.uDockingPoints.Any (aPoint => aPoint.TryRelease (aDocker));
        }

        public void TakeDamage (Damage aDamage, Location aSource) {
            ShieldPrototype.State? lShield = null;
            var lClosest = float.PositiveInfinity;
            foreach (var lState in UEquipment.States<ShieldPrototype.State>()) {
                if (lShield == null) {
                    lShield = lState;
                    lClosest = (aSource.Position - lShield.Slot.transform.position).sqrMagnitude;
                } else {
                    var lDistance = (aSource.Position - lState.Slot.transform.position).sqrMagnitude;
                    if (!(lDistance < lClosest)) continue;
                    lShield = lState;
                    lClosest = lDistance;
                }
            }
            float lPercentShield = 0;
            if (lShield != null) {
                lPercentShield = Mathf.Clamp01 (lShield.Strength / aDamage.ShieldDamage) * (1 - aDamage.ShieldPenetration);
            }
            var lPercentHull = 1 - lPercentShield;
            if (lShield != null) {
                lShield.Strength -= aDamage.ShieldDamage * lPercentShield;
            }
            UHull -= aDamage.HullDamage * lPercentHull;
            // TODO equipment damage damage.EquipmentDamage * ph
        }

        public float GetAngleTo (Location aLocation) {
            var lHeading = aLocation.Position - transform.position;
            return Vector3.SignedAngle (transform.forward, lHeading, transform.up);
        }

        public float GetElevationTo (Location aLocation) {
            var lHeading = aLocation.Position - transform.position;
            return Vector3.SignedAngle (transform.forward, lHeading, -transform.right);
        }

        private void ManageSelectedAndLocks () {
            if (iState.uSelected.UValue != null && !CanDetect(iState.uSelected.UValue)) {
                iState.uSelected.UId.Value = "";
            }
            bool lLocksChanged = false;
            foreach (StructureGetter lGetter in iState.uLocks.Keys.ToArray ()) {
                if (lGetter.UValue == null || !CanDetect (lGetter.UValue) || iState.uLocks.Keys.Count > iState.uStats.UValues.MaxTargetLocks) {
                    iState.uLocks.Remove (lGetter);
                    lLocksChanged = true;
                }
            }
            if (lLocksChanged) {
                PlayerController lPlayerController = iPlayerController.Value;
                if (lPlayerController.UPlayer == this) {
                    lPlayerController.OnLocksChanged.Invoke (this, EventArgs.Empty);
                }
            }
        }

        public bool CanDetect(Structure aOther) {
            if (USector.UId.Value != aOther.USector.UId.Value) return false;
            var lSqrDis = (transform.localPosition - aOther.transform.localPosition).sqrMagnitude;
            var lSqrRange = UStats.UValues.SensorStrength * aOther.UStats.UValues.Detectability;
            return lSqrDis <= lSqrRange;
        }

        public bool Lock (Structure aTarget) {
            if (aTarget == null) return false;
            if (iState.uLocks.Keys.Count >= iState.uStats.UValues.MaxTargetLocks) return false;
            if (IsLocked(aTarget)) return false;
            if (!CanDetect (aTarget)) return false;
            StructureGetter lKey = new StructureGetter ();
            lKey.UId.Value = aTarget.UId;
            iState.uLocks[lKey] = 0;
            PlayerController lPlayerController = iPlayerController.Value;
            if (lPlayerController.UPlayer == this) lPlayerController.OnLocksChanged.Invoke (this, EventArgs.Empty);
            return true;
        }

        public bool Unlock (Structure aTarget) {
            if (aTarget == null) return false;
            if (!IsLocked(aTarget)) return false;
            var lTarget = new StructureGetter();
            lTarget.UId.Value = aTarget.UId;
            iState.uLocks.Remove(lTarget);
            PlayerController lPlayerController = iPlayerController.Value;
            if (lPlayerController.UPlayer == this) lPlayerController.OnLocksChanged.Invoke (this, EventArgs.Empty);
            return true;
        }

        private void TickLocks () {
            foreach (StructureGetter lGetter in iState.uLocks.Keys.ToArray ()) {
                var lProgress = iState.uStats.UValues.ScannerStrength * (lGetter.UValue?.iState.uStats.UValues.SignatureSize ?? 0) * UnityEngine.Time.deltaTime;
                iState.uLocks[lGetter] = Mathf.Min (iState.uLocks[lGetter] + lProgress, 1);
            }
        }

        [Serializable]
        public class State : Behavior {
            public StructurePrototype? uPrototype;
            public Id uId = new Id ();

            public FactionGetter uFaction = new FactionGetter ();
            public SectorGetter uSector = new SectorGetter ();

            public float uHull;
            public Stats uStats = new Stats ();

            public Inventory uInventory = new Inventory (0, 1);
            public List<EquipmentSlot> uEquipment = new List<EquipmentSlot> ();
            public List<DockingPoint> uDockingPoints = new List<DockingPoint> ();
            public StructureGetter uDockedAt = new StructureGetter ();

            public StructureGetter uSelected = new StructureGetter ();
            public Dictionary<StructureGetter, float> uLocks = new Dictionary<StructureGetter, float> ();

            public NpcController? uNpcController;

            public State (StructurePrototype? aPrototype) : base (false) => uPrototype = aPrototype;

            public State (JObject aObj, JsonSerializer aSerializer) : base (false) {
                uPrototype = ItemManager.Instance.GetItem (aObj.Value<string> ("PrototypeId")) as StructurePrototype;
                uId = new Id (aObj.Value<string> ("Id"));

                uFaction.UId.Value = aObj.Value<string> ("FactionId");
                uSector.UId.Value = aObj.Value<string> ("SectorId");

                uHull = aObj.Value<float> ("Hull");
                uStats = aSerializer.Deserialize<Stats> (new JTokenReader (aObj.Value<JObject> ("Stats"))) ?? new Stats ();

                uInventory = aObj.Value<InventorySaveData> ("Inventory").Load ();
                uDockedAt.UId.Value = aObj.Value<string> ("DockedAtId");

                uSelected.UId.Value = aObj.Value<string> ("SelectedId");
                aSerializer.Deserialize<Dictionary<string, float>> (new JTokenReader (aObj.Value<JObject> ("Locks"))).Select (aPair => {
                    StructureGetter lGetter = new StructureGetter ();
                    lGetter.UId.Value = aPair.Key;
                    return new KeyValuePair<StructureGetter, float> (lGetter, aPair.Value);
                }).ToList ().ForEach (aPair => uLocks[aPair.Key] = aPair.Value);
            }

            public override void Enable () {
                uStats.Enable ();
            }

            public override void Disable () {
                uStats.Disable ();
            }

            [Serializable]
            public class Stats : Behavior {
                public Structure.Stats UValues => iValues;
                public List<Structure.Stats.Modifier> UModifiers => iModifiers.ToList ();

                [SerializeField] private Structure.Stats iValues = new Structure.Stats();
                [SerializeField] private List<Structure.Stats.Modifier> iModifiers = new List<Structure.Stats.Modifier> ();

                private bool iRecalculate = true;

                public Stats () : base (false) { }

                public Stats (Structure.Stats aStats, List<Structure.Stats.Modifier> aModifiers) : base (false) {
                    iValues = aStats;
                    iModifiers = aModifiers;
                }

                public override void Tick (object aTicker, float aDt) {
                    var lLength = iModifiers.Count;
                    for (var lIndex = 0; lIndex < lLength; lIndex++) {
                        var lModifier = iModifiers[lIndex];
                        lModifier.Tick (this, aDt);
                        if (lModifier.UExpired) {
                            iModifiers.RemoveAt(lIndex);
                            lIndex--;
                            lLength--;
                        }
                    }
                }

                public bool Add (Structure.Stats.Modifier aModifier) {
                    var lLength = iModifiers.Count;
                    int lIndex;
                    Structure.Stats.Modifier lModifier;
                    for (lIndex = 0; lIndex < lLength; lIndex++) {
                        lModifier = iModifiers[lIndex];
                        if (lModifier == aModifier) {
                            return false;
                        }

                        if (lModifier.uOrder > aModifier.uOrder) {
                            break;
                        }
                    }
                    iModifiers.Insert (lIndex, aModifier);
                    return iRecalculate = true;
                }

                public bool Remove (Structure.Stats.Modifier aModifier) {
                    iRecalculate = iModifiers.Remove (aModifier);
                    return iRecalculate;
                }

                public bool Recalculate (StructurePrototype? aPrototype) {
                    if (!iRecalculate) return false;
                    iValues = aPrototype == null ? new Structure.Stats() : aPrototype.Stats.Clone();
                    foreach (Structure.Stats.Modifier lModifier in iModifiers) {
                        iValues = lModifier.Modify (iValues);
                    }
                    iRecalculate = false;
                    return true;
                }

                public class Converter : JsonConverter<Stats> {
                    public override Stats ReadJson (JsonReader aReader, Type aType, Stats? aValue, bool aExists, JsonSerializer aSerializer) {
                        JObject lObj = (JToken.ReadFrom (aReader) as JObject)!;

                        JsonSerializer lSerializer = new JsonSerializer {
                            Formatting = Formatting.Indented,
                            TypeNameHandling = TypeNameHandling.All,
                        };
                        lSerializer.Converters.Add (new Structure.Stats.Modifier.Converter ());

                        return new Stats (
                            lSerializer.Deserialize<Structure.Stats> (
                                new JTokenReader (lObj.Value<JObject> ("Stats")))!,
                            new List<Structure.Stats.Modifier> (
                                lObj.Value<JArray> ("Modifiers").ToList ().ConvertAll (
                                    aModifier => lSerializer.Deserialize<Structure.Stats.Modifier> (new JTokenReader (aModifier)) ?? new Structure.Stats.Modifier (0, 0)
                                )
                            )
                        );
                    }

                    public override void WriteJson (JsonWriter aWriter, Stats? aValue, JsonSerializer aSerializer) {
                        if (aValue == null) {
                            aWriter.WriteNull ();
                        } else {
                            JObject lObj = new JObject {
                                    new JProperty ("Stats", JObject.FromObject (aValue.iValues)),
                                };

                            JsonSerializer lSerializer = new JsonSerializer {
                                Formatting = Formatting.Indented,
                                TypeNameHandling = TypeNameHandling.All,
                            };
                            lSerializer.Converters.Add (new Structure.Stats.Modifier.Converter ());
                            lObj.Add (
                                new JProperty ("Modifiers", new JArray (aValue.iModifiers.ToList ().ConvertAll (
                                    aModifier => JObject.FromObject (aModifier, lSerializer)
                                )))
                            );

                            lObj.WriteTo (aWriter);
                        }
                    }
                }
            }

            public class Converter : JsonConverter<State> {
                public override State ReadJson (JsonReader aReader, Type aType, State? aValue, bool aExists, JsonSerializer aSerializer) {
                    JObject lObj = (JToken.ReadFrom (aReader) as JObject)!;

                    JsonSerializer lSerializer = new JsonSerializer {
                        Formatting = Formatting.Indented,
                        TypeNameHandling = TypeNameHandling.All,
                    };
                    lSerializer.Converters.Add (new Stats.Converter ());
                    lSerializer.Converters.Add (new DockingPoint.State.Converter ());

                    return new State (lObj, lSerializer);
                }

                public override void WriteJson (JsonWriter aWriter, State? aValue, JsonSerializer aSerializer) {
                    if (aValue == null) {
                        aWriter.WriteNull ();
                    } else {
                        JsonSerializer lSerializer = new JsonSerializer {
                            Formatting = Formatting.Indented,
                            TypeNameHandling = TypeNameHandling.All,
                        };
                        lSerializer.Converters.Add (new Stats.Converter ());
                        lSerializer.Converters.Add (new DockingPoint.Converter ());

                        JObject lObj = new JObject {
                            new JProperty ("PrototypeId", aValue.uPrototype?.Id ?? ""),
                            new JProperty ("Id", aValue.uId.uId),

                            new JProperty ("FactionId", aValue.uFaction.UId.Value),
                            new JProperty ("SectorId", aValue.uSector.UId.Value),

                            new JProperty ("Hull", aValue.uHull),
                            new JProperty ("Stats", JObject.FromObject (aValue.uStats, lSerializer)),

                            new JProperty ("Inventory", JObject.FromObject (aValue.uInventory.Save (), lSerializer)),
                            new JProperty ("EquipmentSlots", new JArray (aValue.uEquipment.ConvertAll (lSlot => lSlot.ToSerializable ()))),
                            new JProperty ("DockingPoints", new JArray (aValue.uDockingPoints.ConvertAll (lPoint => JObject.FromObject (lPoint, lSerializer)))),
                            new JProperty ("DockedAtId", aValue.uDockedAt.UId.Value),

                            new JProperty ("SelectedId", aValue.uSelected.UId.Value),
                            new JProperty ("Locks", JObject.FromObject(aValue.uLocks.Select(lPair => new KeyValuePair<string, float> (lPair.Key.UId.Value, lPair.Value)), lSerializer)),
                        };

                        lObj.WriteTo (aWriter);
                    }
                }
            }
        }

        [Serializable]
        public class Stats {
            public float MaxHull = 1;

            public float LinearMaxSpeedMultiplier = 1;
            public float AngularMaxSpeedMultiplier = 1;
            public float LinearAccelerationMultiplier = 1;
            public float AngularAccelerationMultiplier = 1;

            public float SensorStrength;
            public float ScannerStrength;
            public float Detectability;
            public float SignatureSize;
            public int MaxTargetLocks;

            public float CargoDropPercentage = 1;
            public float InventoryVolume;

            public Stats Clone() {
                return new Stats{
                    MaxHull = MaxHull,
                    
                    LinearMaxSpeedMultiplier = LinearMaxSpeedMultiplier,
                    AngularMaxSpeedMultiplier = AngularMaxSpeedMultiplier,
                    LinearAccelerationMultiplier = LinearAccelerationMultiplier,
                    AngularAccelerationMultiplier = AngularAccelerationMultiplier,
                    
                    SensorStrength = SensorStrength,
                    ScannerStrength = ScannerStrength,
                    Detectability = Detectability,
                    SignatureSize = SignatureSize,
                    MaxTargetLocks = MaxTargetLocks,
                    
                    CargoDropPercentage = CargoDropPercentage,
                    InventoryVolume = InventoryVolume,
                };
            }
            
            [Serializable]
            public class Modifier : Behavior {
                public readonly int uOrder;
                
                public bool UExpired => iDuration <= 0;
                private float iDuration;

                public Modifier (int aOrder, float aDuration) {
                    uOrder = aOrder;
                    iDuration = aDuration;
                }

                public override void Tick (object aTicker, float aDt) {
                    iDuration -= aDt;
                }

                public virtual Stats Modify (Stats aStats) => aStats;

                public class Derived : Modifier {
                    private readonly byte iDerived;

                    public Derived (int aOrder, float aDuration, byte aDerived) : base (aOrder, aDuration) {
                        iDerived = aDerived;
                    }

                    public new class Converter : JsonConverter<Derived> {
                        public override Derived ReadJson (JsonReader aReader, Type aType, Derived? aValue, bool aExists, JsonSerializer aSerializer) {
                            JObject lObj = (JToken.ReadFrom (aReader) as JObject)!;

                            return new Derived (lObj.Value<int> ("Order"), lObj.Value<float> ("Duration"), lObj.Value<byte> ("Derived"));
                        }

                        public override void WriteJson (JsonWriter aWriter, Derived? aValue, JsonSerializer aSerializer) {
                            if (aValue == null) {
                                aWriter.WriteNull ();
                            } else {
                                JObject lObj = new JObject {
                                    new JProperty ("Type", "Derived"),
                                    new JProperty ("Order", aValue.uOrder),
                                    new JProperty ("Duration", aValue.iDuration),
                                    new JProperty ("Derived", aValue.iDerived),
                                };

                                lObj.WriteTo (aWriter);
                            }
                        }
                    }
                }

                public class OrderComparer : IComparer<Modifier> {
                    public int Compare (Modifier a, Modifier b) {
                        return a.uOrder.CompareTo (b.uOrder);
                    }
                }

                public class Converter : JsonConverter<Modifier> {
                    public override Modifier ReadJson (JsonReader aReader, Type aType, Modifier? aValue, bool aExists, JsonSerializer aSerializer) {
                        JObject lObj = (JToken.ReadFrom (aReader) as JObject)!;

                        if (lObj.Value<string> ("Type") == "Derived") {
                            JsonSerializer lSerializer = new JsonSerializer {
                                Formatting = Formatting.Indented,
                                TypeNameHandling = TypeNameHandling.All,
                            };
                            lSerializer.Converters.Add (new Derived.Converter ());

                            return lSerializer.Deserialize<Derived> (new JTokenReader (lObj))!;
                        }

                        return new Modifier (lObj.Value<int> ("Order"), lObj.Value<float> ("Duration"));
                    }

                    public override void WriteJson (JsonWriter aWriter, Modifier? aValue, JsonSerializer aSerializer) {
                        if (aValue == null) {
                            aWriter.WriteNull ();
                        } else {
                            if (aValue is Derived) {
                                JsonSerializer lSerializer = new JsonSerializer {
                                    Formatting = Formatting.Indented,
                                    TypeNameHandling = TypeNameHandling.All,
                                };
                                lSerializer.Converters.Add (new Derived.Converter ());
                                JObject lObj = JObject.FromObject (aValue, lSerializer);

                                lObj.WriteTo (aWriter);
                            } else {
                                JObject lObj = new JObject {
                                    new JProperty ("Order", aValue.uOrder),
                                    new JProperty ("Duration", aValue.iDuration),
                                };

                                lObj.WriteTo (aWriter);
                            }
                        }
                    }
                }
            }
        }

        [Serializable]
        public class Equipment : INotifier<Type> {
            private readonly State iState;

            public List<EquipmentSlot> UAll => iState.uEquipment;
            
            public event EventHandler<Type>? Notifier;

            public Equipment(State aState) {
                iState = aState;
                iSlotsLength = iState.uEquipment.Count;
            }

            private int iSlotsLength;
        
            private readonly Dictionary<Type, List<EquipmentSlot>> iSlotsCache = new Dictionary<Type, List<EquipmentSlot>>();
            public List<EquipmentSlot> Slots<T> () where T : EquipmentPrototype.State {
                if (iSlotsCache.ContainsKey(typeof(T))) {
                    return iSlotsCache[typeof(T)];
                }
            
                List<EquipmentSlot> lRet = new List<EquipmentSlot>(iSlotsLength);
                for (var lIndex = 0; lIndex < iSlotsLength; lIndex++) {
                    if (!(iState.uEquipment[lIndex].UState is T)) continue;
                    lRet.Add(iState.uEquipment[lIndex]);
                }

                iSlotsCache[typeof(T)] = lRet;
                return lRet;
            }
        
            private readonly Dictionary<Type, object> iStatesCache = new Dictionary<Type, object>();
            public List<T> States<T> () where T : EquipmentPrototype.State {
                if (iStatesCache.ContainsKey(typeof(T))) {
                    return (iStatesCache[typeof(T)] as List<T>)!;
                }
            
                List<T> lRet = new List<T>(iSlotsLength);
                for (var lIndex = 0; lIndex < iSlotsLength; lIndex++) {
                    if (!(iState.uEquipment[lIndex].UState is T lState)) continue;
                    lRet.Add(lState);
                }

                iStatesCache[typeof(T)] = lRet;
                return lRet;
            }

            public void Update(object aSender, Type aType) {
                iSlotsLength = iState.uEquipment.Count;
                
                iSlotsCache.Clear();
                iStatesCache.Clear();
                
                Notifier?.Invoke(this, aType);
            }
        }

        public class Converter : JsonConverter<Structure> {
            public override Structure ReadJson (JsonReader aReader, Type aType, Structure? aValue, bool aExists, JsonSerializer aSerializer) {
                JObject lObj = (JToken.ReadFrom (aReader) as JObject)!;

                JsonSerializer lSerializer = new JsonSerializer {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.All,
                };
                lSerializer.Converters.Add (new State.Converter ());
                lSerializer.Converters.Add (new DockingPoint.State.Converter ());

                State lState = lSerializer.Deserialize<State> (new JTokenReader (lObj)) ?? new State (null);

                StructurePrototype? lPrototype;
                if ((lPrototype = lState.uPrototype) != null) {
                    GameObject? lPrefab;
                    if ((lPrefab = lPrototype.Prefab) != null) {
                        GameObject lInstantiated = Instantiate (lPrefab);
                        Structure? lComponent;
                        if ((lComponent = lInstantiated.GetComponent<Structure> ()) == null) {
                            lComponent = lInstantiated.AddComponent<Structure> ();
                        }

                        lComponent.Construct (lState);
                        BehaviorManager lBehaviorManager = Singletons.Get<BehaviorManager> ();
                        lBehaviorManager.InitializeImmediately (lComponent);

                        lObj.Value<JArray> ("EquipmentSlots").ToList ().ForEach (aSerializedSlot => {
                            EquipmentSlot.Serializable lDeserializedSlot = lSerializer.Deserialize<EquipmentSlot.Serializable> (new JTokenReader (aSerializedSlot)) ?? new EquipmentSlot.Serializable ();
                            lState.uEquipment.Find (aSlot => aSlot.USerializationId == aSerializedSlot.Value<string> ("SerializationId")).FromSerializable (lDeserializedSlot);
                        });
                        lObj.Value<JArray> ("DockingPoints").ToList ().ForEach (aSerializedPoint => {
                            DockingPoint.State lDeserializedPoint = lSerializer.Deserialize<DockingPoint.State> (new JTokenReader (aSerializedPoint)) ?? new DockingPoint.State ();
                            lState.uDockingPoints.Find (aPoint => aPoint.USerializationId == aSerializedPoint.Value<string> ("SerializationId")).Construct (lDeserializedPoint);
                        });

                        return lComponent;
                    }
                }

                GameObject lEmpty = new GameObject ();
                return lEmpty.AddComponent<Structure> ();
            }

            public override void WriteJson (JsonWriter aWriter, Structure? aValue, JsonSerializer aSerializer) {
                if (aValue == null) {
                    aWriter.WriteNull ();
                } else {
                    JsonSerializer lSerializer = new JsonSerializer {
                        Formatting = Formatting.Indented,
                        TypeNameHandling = TypeNameHandling.All,
                    };
                    lSerializer.Converters.Add (new State.Converter ());

                    JObject.FromObject (aValue.iState, lSerializer).WriteTo (aWriter);
                }
            }
        }
    }
}
#nullable restore

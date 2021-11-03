using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Serialization;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using DarkFrontier.Items;
using DarkFrontier.Items.Conditions;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

#nullable enable
namespace DarkFrontier.Equipment {
    public class EquipmentSlot : ComponentBehavior, ISavableState<EquipmentSlot.Serializable> {
        public Structure? Equipper;
        public List<ItemConditionSO> Filters = new List<ItemConditionSO> ();

        public EquipmentPrototype? Equipment;

        public string USerializationId => iSerializationId;

#pragma warning disable IDE0044 // Add readonly modifier
        [SerializeField] private string iSerializationId = "";
#pragma warning restore IDE0044 // Add readonly modifier

        public EquipmentPrototype.State UState {
            get {
                // State might be of a left-over incompatible type
                if (Equipment == null) UnsafeState = new EquipmentPrototype.State (this);
                else Equipment.EnsureStateType (this);

                // State might be null if EnsureStateType is implemented incorrectly
                return UnsafeState ??= new EquipmentPrototype.State(this);
            }
        }
        [SerializeReference] public EquipmentPrototype.State? UnsafeState;

        private bool iHaveEquipment = false;
        
        public override void Initialize () {
            Equipper = GetComponentInParent<Structure> ();
            iHaveEquipment = Equipment != null;
        }

        public override void Tick (object aTicker, float aDt) {
            if (iHaveEquipment) Equipment!.Tick (this, aDt);
        }

        public override void FixedTick (object aTicker, float aDt) {
            if (iHaveEquipment) Equipment!.FixedTick (this, aDt);
        }

        public bool ChangeEquipment (EquipmentPrototype? aTarget) {
            if (aTarget != null) {
                foreach (ItemConditionSO lFilter in Filters) {
                    if (!lFilter.MeetsCondition(aTarget)) {
                        return false;
                    }
                }
            }
            
            if (Equipment != null) {
                Equipment.OnUnequip(this);
            }
            Equipment = aTarget;
            if (Equipment == null) {
                iHaveEquipment = false;
            } else {
                iHaveEquipment = true;
                Equipment.OnEquip(this);
            }

            if (Equipper != null) {
                Equipper.InvalidateEquipmentQueryCache();
            }
            
            return true;
        }

        public virtual Serializable ToSerializable () {
            return new Serializable {
                EquipmentId = Equipment == null ? "" : Equipment.Id,
                SerializationId = iSerializationId,
                State = UState.ToSerializable (),
            };
        }

        public virtual void FromSerializable (Serializable serializable) {
            Equipment = Singletons.Get<ItemManager> ().GetItem (serializable.EquipmentId) as EquipmentPrototype;
            if (Equipment != null) {
                UnsafeState = Equipment.GetNewState (this);
                UState.FromSerializable (serializable.State);
            }
        }

        [Serializable]
        public class Serializable {
            public string EquipmentId = "";
            public string SerializationId = "";
            public EquipmentPrototype.State.Serializable State = new EquipmentPrototype.State.Serializable ();
        }
    }
}
#nullable restore

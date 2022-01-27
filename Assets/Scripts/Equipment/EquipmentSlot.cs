using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Serialization;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using DarkFrontier.Items;
using DarkFrontier.Items.Conditions;
using UnityEngine;


namespace DarkFrontier.Equipment {
    public class EquipmentSlot : ComponentBehavior, ISavableState<EquipmentSlot.Serializable> {
        public Structure? Equipper;
        public List<ItemConditionSO> Filters = new List<ItemConditionSO> ();

        public EquipmentPrototype? Equipment;

        public string USerializationId => iSerializationId;

        [SerializeField] private string iSerializationId = "";

        [SerializeReference] public EquipmentPrototype.State? UState;

        private bool iHaveEquipment = false;
        
        public override void Initialize () {
            Equipper = GetComponentInParent<Structure> ();
            if (iHaveEquipment = Equipment != null) Equipment!.EnsureStateType(this);
        }

        public override void Tick (object aTicker, float aDt) {
            if (iHaveEquipment) Equipment!.Tick (this, aDt);
        }

        public override void FixedTick (object aTicker, float aDt) {
            if (iHaveEquipment) Equipment!.FixedTick (this, aDt);
        }

        public bool ChangeEquipment (EquipmentPrototype? aTarget) {
            // Check if target equipment is allowed on this slot
            if (aTarget != null) {
                foreach (ItemConditionSO lFilter in Filters) {
                    if (!lFilter.MeetsCondition(aTarget)) {
                        return false;
                    }
                }
            }
            
            // Unequip old equipment
            if (Equipment != null) {
                Equipment.OnUnequip(this);
                if (Equipper != null) {
                    Equipper.uEquipment.Update(this, UState.GetType());
                }
            }
            
            // Equip new equipment
            Equipment = aTarget;
            if (Equipment == null) {
                iHaveEquipment = false;
            } else {
                iHaveEquipment = true;
                Equipment.OnEquip(this);
                Equipment.EnsureStateType(this);
                if (Equipper != null) {
                    Equipper.uEquipment.Update(this, UState.GetType());
                }
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
            Equipment = ItemManager.Instance.GetItem (serializable.EquipmentId) as EquipmentPrototype;
            if (Equipment != null) {
                UState = Equipment.GetNewState (this);
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


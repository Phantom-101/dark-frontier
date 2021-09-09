using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Serialization;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    public class EquipmentSlot : ComponentBehavior, ISavableState<EquipmentSlot.Serializable> {
        public Structure? Equipper;
        public List<ItemConditionSO> Filters = new List<ItemConditionSO> ();

        public EquipmentPrototype? Equipment;

        public EquipmentPrototype.State State {
            get {
                // State might be of a left-over incompatible type
                if (Equipment == null) UnsafeState = new EquipmentPrototype.State (this);
                else Equipment.EnsureStateType (this);

                // State might be null if EnsureStateType is implemented incorrectly
                if (UnsafeState == null) UnsafeState = new EquipmentPrototype.State (this);

                return UnsafeState;
            }
        }
        [SerializeReference] public EquipmentPrototype.State? UnsafeState;

        protected override void SingleInitialize () {
            Equipper = GetComponentInParent<Structure> ();
        }

        protected override void InternalTick (float dt) {
            if (Equipment != null) Equipment.Tick (this, dt);
        }

        protected override void InternalFixedTick (float dt) {
            if (Equipment != null) Equipment.FixedTick (this, dt);
        }

        public override bool Validate () {
            if (Equipper == null) {
                Equipper = GetComponentInParent<Structure> ();

                // If null, there is no equipper in parent tree
                if (Equipper == null) return false;
            }

            if (Equipment != null) State.Durability = Mathf.Clamp (State.Durability, 0, Equipment.Durability);

            return true;
        }

        public bool ChangeEquipment (EquipmentPrototype target) {
            foreach (ItemConditionSO filter in Filters)
                if (!filter.MeetsCondition (target))
                    return false;

            Equipment = target;
            Equipment.OnEquip (this);
            return true;
        }

        public virtual Serializable ToSerializable () {
            return new Serializable {
                EquipmentId = Equipment == null ? "" : Equipment.Id,
                State = State.ToSerializable (),
            };
        }

        public virtual void FromSerializable (Serializable serializable) {
            Equipment = Singletons.Get<ItemManager> ().GetItem (serializable.EquipmentId) as EquipmentPrototype;
            if (Equipment != null) {
                UnsafeState = Equipment.GetNewState (this);
                State.FromSerializable (serializable.State);
            }
        }

        [Serializable]
        public class Serializable {
            public string EquipmentId = "";
            public EquipmentPrototype.State.Serializable State = new EquipmentPrototype.State.Serializable ();
        }
    }
}
#nullable restore

using DarkFrontier.Foundation.Serialization;
using System;
using UnityEngine;

namespace DarkFrontier.Equipment {

#nullable enable
    public class EquipmentPrototype : ItemPrototype {
        public float Durability;
        public GameObject? ButtonPrefab;
        public GameObject? PanelPrefab;

        /// <summary>
        /// Called when game is started and equipment already exists
        /// </summary>
        /// <param name="slot"></param>
        public virtual void OnAwake (EquipmentSlot slot) => EnsureStateType (slot);

        /// <summary>
        /// Called when the equipment is added
        /// </summary>
        /// <param name="slot"></param>
        public virtual void OnEquip (EquipmentSlot slot) => slot.UnsafeState = GetNewState (slot);

        /// <summary>
        /// Called when the equipment is removed
        /// </summary>
        /// <param name="slot"></param>
        public virtual void OnUnequip (EquipmentSlot slot) { }

        /// <summary>
        /// Called every game tick
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="dt"></param>
        public virtual void Tick (EquipmentSlot slot, float dt) { }

        /// <summary>
        /// Called every physics tick
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="dt"></param>
        public virtual void FixedTick (EquipmentSlot slot, float dt) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot"></param>
        /// <returns>Whether or not the equipment button be clicked</returns>
        public virtual bool CanClick (EquipmentSlot slot) => false;

        /// <summary>
        /// Called when the equipment button is clicked
        /// </summary>
        /// <param name="slot"></param>
        public virtual void OnClicked (EquipmentSlot slot) { }

        /// <summary>
        /// Ensures that the type of the state is suitable for the current equipment
        /// </summary>
        /// <param name="slot"></param>
        public virtual void EnsureStateType (EquipmentSlot slot) {
            if (!(slot.UnsafeState is State)) slot.UnsafeState = GetNewState (slot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot"></param>
        /// <returns>A state suitable for the current equipment</returns>
        public virtual State GetNewState (EquipmentSlot slot) => new State (slot, this);

        [Serializable]
        public class State : ISavableState<State.Serializable> {
            public EquipmentSlot Slot;
            public float Durability;

            public State (EquipmentSlot slot) {
                Slot = slot;
            }

            public State (EquipmentSlot slot, EquipmentPrototype equipment) : this (slot) {
                Durability = equipment.Durability;
            }

            public virtual Serializable ToSerializable () {
                return new Serializable {
                    Durability = Durability,
                };
            }

            public virtual void FromSerializable (Serializable serializable) {
                Durability = serializable.Durability;
            }

            [Serializable]
            public class Serializable {
                public float Durability = 0;
            }
        }
    }
}
#nullable restore

using System;
using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;

namespace DarkFrontier.UI.Equipment.Buttons {
    public class EquipmentButton : ComponentBehavior {
        public EquipmentSlot Slot;

        private readonly Lazy<BehaviorTimekeeper> iBehaviorTimekeeper = new Lazy<BehaviorTimekeeper>(() => Singletons.Get<BehaviorTimekeeper>(), false);
        
        public override void Enable () {
            iBehaviorTimekeeper.Value.Subscribe (this);
        }

        public override void Disable () {
            iBehaviorTimekeeper.Value.Unsubscribe (this);
        }
    }
}
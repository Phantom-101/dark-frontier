using DarkFrontier.Foundation.Events;
using System;
using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class BehaviorTicker : Behavior, INotifier<BehaviorTicker.BehaviorTickArgs> {
        public event EventHandler<BehaviorTickArgs> Notifier;

        [SerializeField] protected float tickInterval;
        protected float tickCounter;

        protected override void InternalTick (float dt) {
            tickCounter += dt;

            if (tickCounter >= tickInterval) {
                Notifier?.Invoke (this, new BehaviorTickArgs (tickCounter));
                tickCounter = 0;
            }
        }

        public class BehaviorTickArgs : EventArgs {
            public float dt;

            public BehaviorTickArgs (float dt) {
                this.dt = dt;
            }
        }
    }
}

using System;
using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class BehaviorTicker : Behavior, INotifier {
        public event EventHandler Notifier;

        [SerializeField] protected float tickInterval;
        protected float tickCounter;
        [SerializeField] protected int expensiveTickPer;
        protected int ticksPassed;
        protected float expensiveTickCounter;

        protected override void InternalTick (float dt) {
            tickCounter += dt;
            expensiveTickCounter += dt;

            if (tickCounter >= tickInterval) {
                ticksPassed++;
                if (ticksPassed >= expensiveTickPer) {
                    Notifier?.Invoke (this, new BehaviorTickArgs (tickCounter, expensiveTickCounter));
                    ticksPassed = 0;
                    expensiveTickCounter = 0;
                } else {
                    Notifier?.Invoke (this, new BehaviorTickArgs (tickCounter));
                }
                tickCounter = 0;
            }
        }

        public class BehaviorTickArgs : EventArgs {
            public float dt;
            public float? edt;

            public BehaviorTickArgs (float dt, float? edt = null) {
                this.dt = dt;
                this.edt = edt;
            }
        }
    }
}

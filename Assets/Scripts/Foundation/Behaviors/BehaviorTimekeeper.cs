using System;
using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class BehaviorTimekeeper : MonoBehaviour {
        public event EventHandler<float> TickNotifier;
        public event EventHandler<float> FixedTickNotifier;
        public event EventHandler<float> LateTickNotifier;

        private void Update () {
            TickNotifier?.Invoke (this, UnityEngine.Time.deltaTime);
        }

        private void FixedUpdate () {
            FixedTickNotifier?.Invoke (this, UnityEngine.Time.fixedDeltaTime);
        }

        private void LateUpdate () {
            LateTickNotifier?.Invoke (this, UnityEngine.Time.deltaTime);
        }

        public void Subscribe (IBehavior aBehavior) {
            TickNotifier += aBehavior.Tick;
            FixedTickNotifier += aBehavior.FixedTick;
            LateTickNotifier += aBehavior.LateTick;
        }

        public void Unsubscribe (IBehavior aBehavior) {
            TickNotifier -= aBehavior.Tick;
            FixedTickNotifier -= aBehavior.FixedTick;
            LateTickNotifier -= aBehavior.LateTick;
        }
    }
}

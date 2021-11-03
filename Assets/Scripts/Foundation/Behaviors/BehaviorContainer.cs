using DarkFrontier.Foundation.Services;
using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class BehaviorContainer : MonoBehaviour, IBehavior {
        public Behavior uBehavior;

        private BehaviorManager iBehaviorManager;

        protected virtual void Awake () {
            iBehaviorManager = Singletons.Get<BehaviorManager> ();
        }

        protected virtual void OnEnable () {
            if (uBehavior != null) {
                iBehaviorManager.QueueEnable (uBehavior);
            }
        }

        protected virtual void OnDisable () {
            if (uBehavior != null) {
                iBehaviorManager.QueueDisable (uBehavior);
            }
        }

        protected virtual void OnDestroy () {
            if (uBehavior != null) {
                iBehaviorManager.DisableImmediately (uBehavior);
            }
        }

        public void Initialize () => uBehavior?.Initialize ();

        public void Enable () => uBehavior?.Enable ();
        public void Disable () => uBehavior?.Disable ();

        public void Tick (object aTicker, float aDt) => uBehavior?.Tick (aTicker, aDt);
        public void FixedTick (object aTicker, float aDt) => uBehavior?.FixedTick (aTicker, aDt);
        public void LateTick (object aTicker, float aDt) => uBehavior?.LateTick (aTicker, aDt);
    }
}

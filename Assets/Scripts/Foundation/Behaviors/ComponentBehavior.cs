using DarkFrontier.Foundation.Services;
using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class ComponentBehavior : MonoBehaviour, IBehavior {
        protected BehaviorManager oBehaviorManager;

        protected virtual void Awake () {
            oBehaviorManager = Singletons.Get<BehaviorManager> ();
            oBehaviorManager.QueueInitialize (this);
        }

        protected virtual void OnEnable () {
            oBehaviorManager.QueueEnable (this);
        }

        protected virtual void OnDisable () {
            oBehaviorManager.QueueDisable (this);
        }

        protected virtual void OnDestroy () {
            oBehaviorManager.DisableImmediately (this);
        }

        public virtual void Initialize () { }

        public virtual void Enable () { }
        public virtual void Disable () { }

        public virtual void Tick (object aTicker, float aDt) { }
        public virtual void FixedTick (object aTicker, float aDt) { }
        public virtual void LateTick (object aTicker, float aDt) { }
    }
}
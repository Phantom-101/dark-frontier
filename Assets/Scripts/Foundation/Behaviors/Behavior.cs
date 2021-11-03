using DarkFrontier.Foundation.Services;

namespace DarkFrontier.Foundation.Behaviors {
    public class Behavior : IBehavior {
        protected BehaviorManager oBehaviorManager;

        public Behavior () : this (true) { }

        public Behavior (bool aInitialize) {
            if (aInitialize) {
                oBehaviorManager = Singletons.Get<BehaviorManager> ();
                oBehaviorManager.QueueInitialize (this);
                oBehaviorManager.QueueEnable (this);
            }
        }

        public virtual void Initialize () { }

        public virtual void Enable () { }
        public virtual void Disable () { }

        public virtual void Tick (object aTicker, float aDt) { }
        public virtual void FixedTick (object aTicker, float aDt) { }
        public virtual void LateTick (object aTicker, float aDt) { }
    }
}

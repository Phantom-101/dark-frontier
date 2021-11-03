using DarkFrontier.Foundation.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class BehaviorManager : Behavior {
        private readonly HashSet<IBehavior> iInitializeQueue = new HashSet<IBehavior> ();
        private readonly HashSet<IBehavior> iEnableQueue = new HashSet<IBehavior> ();
        private readonly HashSet<IBehavior> iDisableQueue = new HashSet<IBehavior> ();

        public BehaviorManager () : base (false) {
            InitializeImmediately (this);
            EnableImmediately (this);
        }

        public override void Enable () {
            Singletons.Get<BehaviorTimekeeper> ().Subscribe (this);
        }

        public override void Disable () {
            Singletons.Get<BehaviorTimekeeper> ().Unsubscribe (this);
        }

        public void InitializeImmediately (IBehavior aBehavior) {
            DequeueInitialize (aBehavior);
            if (aBehavior != null) {
                aBehavior.Initialize ();
            }
        }

        public void EnableImmediately (IBehavior aBehavior) {
            DequeueEnable (aBehavior);
            if (aBehavior != null) {
                aBehavior.Enable ();
            }
        }

        public void DisableImmediately (IBehavior aBehavior) {
            DequeueDisable (aBehavior);
            if (aBehavior != null) {
                aBehavior.Disable ();
            }
        }

        public bool QueueInitialize (IBehavior aBehavior) => iInitializeQueue.Add (aBehavior);
        public bool QueueEnable (IBehavior aBehavior) => iEnableQueue.Add (aBehavior);
        public bool QueueDisable (IBehavior aBehavior) => iDisableQueue.Add (aBehavior);

        public bool DequeueInitialize (IBehavior aBehavior) => iInitializeQueue.Remove (aBehavior);
        public bool DequeueEnable (IBehavior aBehavior) => iEnableQueue.Remove (aBehavior);
        public bool DequeueDisable (IBehavior aBehavior) => iDisableQueue.Remove (aBehavior);

        public void AddBehavior<T, TArgs> (GameObject aGameObject, TArgs aArgs) where T : ComponentBehavior, ICtorArgs<TArgs> {
            T behavior = aGameObject.AddComponent<T> ();
            behavior.Construct (aArgs);
        }

        public override void Tick (object aTicker, float aDt) {
            iInitializeQueue.ToList ().ForEach (lBehavior => InitializeImmediately (lBehavior));
            iEnableQueue.ToList ().ForEach (lBehavior => EnableImmediately (lBehavior));
            iDisableQueue.ToList ().ForEach (lBehavior => DisableImmediately (lBehavior));
        }
    }
}

﻿using System;
using DarkFrontier.Foundation.Services;

namespace DarkFrontier.Foundation.Behaviors {
    public class Behavior : IBehavior {
        protected readonly Lazy<BehaviorManager> oBehaviorManager = new Lazy<BehaviorManager>(() => Singletons.Get<BehaviorManager>(), false);

        public Behavior () : this (true) { }

        public Behavior (bool aInitialize) {
            if (!aInitialize) return;
            oBehaviorManager.Value.QueueInitialize (this);
            oBehaviorManager.Value.QueueEnable (this);
        }

        public virtual void Initialize () { }

        public virtual void Enable () { }
        public virtual void Disable () { }

        public virtual void Tick (object aTicker, float aDt) { }
        public virtual void FixedTick (object aTicker, float aDt) { }
        public virtual void LateTick (object aTicker, float aDt) { }
    }
}

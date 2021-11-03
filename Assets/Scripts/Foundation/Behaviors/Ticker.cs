using DarkFrontier.Foundation.Events;
using System;
using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class Ticker : Behavior, INotifier<float> {
        public event EventHandler<float> Notifier;

        [SerializeField] protected float iInterval;
        protected float iCounter;

        public Ticker (float aInterval = 0) {
            iInterval = aInterval;
        }

        public override void Tick (object aTicker, float aDt) {
            iCounter += aDt;

            if (iCounter >= iInterval) {
                Notifier?.Invoke (this, iCounter);
                iCounter = 0;
            }
        }
    }
}

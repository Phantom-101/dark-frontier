using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class BehaviorContainer : MonoBehaviour, IBehavior {
        public Behavior Behavior;

        public void TryInitialize () => Behavior?.TryInitialize ();

        public void Tick (float dt, float? edt = null) => Behavior?.Tick (dt, edt);
        public void LateTick (float dt, float? edt = null) => Behavior?.LateTick (dt, edt);
        public void FixedTick (float dt, float? edt = null) => Behavior?.FixedTick (dt, edt);

        public bool Validate () => Behavior?.Validate () ?? false;

        public void SubscribeEventListeners () => Behavior?.SubscribeEventListeners ();
        public void UnsubscribeEventListeners () => Behavior?.UnsubscribeEventListeners ();
    }
}

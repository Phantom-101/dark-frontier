using UnityEngine;

namespace DarkFrontier.Foundation.Behaviors {
    public class BehaviorContainer : MonoBehaviour, IBehavior {
        public Behavior Behavior;

        public void TryInitialize () => Behavior?.TryInitialize ();
        public void GetServices () => Behavior?.GetServices ();

        public void Tick (float dt) => Behavior?.Tick (dt);
        public void LateTick (float dt) => Behavior?.LateTick (dt);
        public void FixedTick (float dt) => Behavior?.FixedTick (dt);

        public bool Validate () => Behavior?.Validate () ?? false;

        public void SubscribeEventListeners () => Behavior?.SubscribeEventListeners ();
        public void UnsubscribeEventListeners () => Behavior?.UnsubscribeEventListeners ();
    }
}

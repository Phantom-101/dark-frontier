using DarkFrontier.Foundation.Behaviors;

namespace DarkFrontier.Other {
    public class Unpacker : ComponentBehavior {
        public Timing uTiming;

        protected override void Awake () {
            base.Awake ();
            if (uTiming == Timing.Immediate) {
                Unpack ();
            }
        }

        public override void Enable () {
            if (uTiming == Timing.Late) {
                Unpack ();
            }
        }

        private void Unpack () {
            while (transform.childCount > 0) {
                transform.GetChild (0).SetParent (transform.parent, true);
            }
            Destroy (gameObject);
        }

        public enum Timing {
            Immediate,
            Late,
            Trigger,
        }
    }
}
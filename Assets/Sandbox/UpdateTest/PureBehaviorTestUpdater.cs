using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;

namespace DarkFrontier.Sandbox.UpdateTest {
    public class PureBehaviorTestUpdater : Behavior {
        private int iNumber;

        public override void Enable () {
            Singletons.Get<BehaviorTimekeeper> ().Subscribe (this);
        }

        public override void Disable () {
            Singletons.Get<BehaviorTimekeeper> ().Unsubscribe (this);
        }

        public override void Tick (object aTicker, float aDt) {
            iNumber++;
        }
    }
}

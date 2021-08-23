using DarkFrontier.Foundation.Behaviors;

public class PureBehaviorTestUpdater : Behavior {
    private int number;

    protected override void InternalTick (float dt) {
        number++;
    }
}

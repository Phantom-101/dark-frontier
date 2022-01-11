using DarkFrontier.Structures;

namespace DarkFrontier.Items.Conditions {
    public abstract class ItemCondition {
        public abstract bool MeetsCondition (ItemPrototype input);
    }
}

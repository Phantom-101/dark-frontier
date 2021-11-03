using DarkFrontier.Items.Prototypes;

namespace DarkFrontier.Items.Conditions {
    public abstract class ItemCondition {
        public abstract bool MeetsCondition (ItemPrototype input);
    }
}

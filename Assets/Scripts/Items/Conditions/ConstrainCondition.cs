using System.Collections.Generic;
using DarkFrontier.Items.Prototypes;

namespace DarkFrontier.Items.Conditions {
    public class ConstrainCondition : ItemCondition {
        public List<ItemPrototype> Constraint;

        public ConstrainCondition (List<ItemPrototype> Constraint) {
            this.Constraint = Constraint;
        }

        public override bool MeetsCondition (ItemPrototype input) {
            return Constraint.Contains (input);
        }
    }
}

using System.Collections.Generic;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Items.Conditions {
    [CreateAssetMenu (menuName = "Items/Conditions/Constrain")]
    public class ConstrainConditionSO : ItemConditionSO {
        public List<ItemPrototype> Constraint;

        public override bool MeetsCondition (ItemPrototype input) {
            return new ConstrainCondition (Constraint).MeetsCondition (input);
        }
    }
}

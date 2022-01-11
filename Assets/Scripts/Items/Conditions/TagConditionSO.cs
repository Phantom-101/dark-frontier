using System.Collections.Generic;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Items.Conditions {
    [CreateAssetMenu (menuName = "Items/Conditions/Tag")]
    public class TagConditionSO : ItemConditionSO {
        public List<ItemTag> Tags;

        public override bool MeetsCondition (ItemPrototype input) {
            return new TagCondition (Tags).MeetsCondition (input);
        }
    }
}

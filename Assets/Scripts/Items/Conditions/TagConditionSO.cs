using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Conditions/Tag")]
public class TagConditionSO : ItemConditionSO {
    public List<ItemTag> Tags;

    public override bool MeetsCondition (ItemSO input) {
        return new TagCondition (Tags).MeetsCondition (input);
    }
}

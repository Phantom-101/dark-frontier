using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Conditions/Constrain")]
public class ConstrainConditionSO : ItemConditionSO {
    public List<ItemSO> Constraint;

    public override bool MeetsCondition (ItemSO input) {
        return new ConstrainCondition (Constraint).MeetsCondition (input);
    }
}

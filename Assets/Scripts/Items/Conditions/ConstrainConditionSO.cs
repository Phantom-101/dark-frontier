using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Conditions/Constrain")]
public class ConstrainConditionSO : ItemConditionSO {
    public List<ItemPrototype> Constraint;

    public override bool MeetsCondition (ItemPrototype input) {
        return new ConstrainCondition (Constraint).MeetsCondition (input);
    }
}

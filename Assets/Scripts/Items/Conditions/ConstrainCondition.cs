using System.Collections.Generic;

public class ConstrainCondition : ItemCondition {
    public List<ItemSO> Constraint;

    public ConstrainCondition (List<ItemSO> Constraint) {
        this.Constraint = Constraint;
    }

    public override bool MeetsCondition (ItemSO input) {
        return Constraint.Contains (input);
    }
}

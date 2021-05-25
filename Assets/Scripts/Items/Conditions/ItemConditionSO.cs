using UnityEngine;

public abstract class ItemConditionSO : ScriptableObject {
    public abstract bool MeetsCondition (ItemSO input);
}

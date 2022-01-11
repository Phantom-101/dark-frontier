using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Items.Conditions {
    public abstract class ItemConditionSO : ScriptableObject {
        public abstract bool MeetsCondition (ItemPrototype input);
    }
}

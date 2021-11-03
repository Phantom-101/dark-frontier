using UnityEngine;

namespace DarkFrontier.Items.Conditions {
    [CreateAssetMenu (menuName = "Items/Item Tag")]
    public class ItemTag : ScriptableObject {
        public string Name;
        public string Description;
    }
}

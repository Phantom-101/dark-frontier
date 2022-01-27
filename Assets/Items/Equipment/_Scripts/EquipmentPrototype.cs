using UnityEngine;


namespace DarkFrontier.Items.Equipment
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Equipment")]
    public class EquipmentPrototype : Items._Scripts.ItemPrototype
    {
        public float hp;

        public int rating;

        public GameObject? selectorPrefab;

        public GameObject? prefab;
        
        public GameObject? destructionFx;
    }
}

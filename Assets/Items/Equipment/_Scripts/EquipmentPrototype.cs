#nullable enable
using DarkFrontier.Items._Scripts;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Equipment
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Equipment")]
    public class EquipmentPrototype : Items._Scripts.ItemPrototype
    {
        public float hp;

        public int rating;

        public VisualTreeAsset? selectorElement;

        public GameObject? prefab;
        
        public GameObject? destructionFx;
        
        public override ItemInstance NewInstance() => new EquipmentInstance(this);
    }
}

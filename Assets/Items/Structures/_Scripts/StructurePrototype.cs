#nullable enable
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Structure")]
    public class StructurePrototype : _Scripts.ItemPrototype
    {
        public float size;
        
        public Sprite? hullIndicator;

        public VisualTreeAsset? selectorElement;

        public GameObject? prefab;
        
        public GameObject? destructionFx;
    }
}

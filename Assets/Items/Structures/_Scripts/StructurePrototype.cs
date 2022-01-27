using UnityEngine;


namespace DarkFrontier.Items.Structures
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Structure")]
    public class StructurePrototype : _Scripts.ItemPrototype
    {
        public float size;
        
        public Sprite? hullIndicator;

        public GameObject? selectorPrefab;

        public GameObject? prefab;
        
        public GameObject? destructionFx;
    }
}

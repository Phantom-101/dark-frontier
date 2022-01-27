using UnityEngine;


namespace DarkFrontier.Items.Segments
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Segment")]
    public class SegmentPrototype : _Scripts.ItemPrototype
    {
        public float hp;

        public float poolHp;

        public int rating;
        
        public Sprite? hullIndicator;

        public GameObject? selectorPrefab;

        public GameObject? prefab;
        
        public GameObject? destructionFx;
    }
}

﻿#nullable enable
using DarkFrontier.Items._Scripts;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Segments
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Segment")]
    public class SegmentPrototype : _Scripts.ItemPrototype
    {
        public float hp;

        public float poolHp;

        public int rating;
        
        public Sprite? hullIndicator;

        public VisualTreeAsset? selectorElement;

        public GameObject? prefab;
        
        public GameObject? destructionFx;
        
        public override ItemInstance NewInstance() => new SegmentInstance(this);
    }
}

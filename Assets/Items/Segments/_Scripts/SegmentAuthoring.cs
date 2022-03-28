#nullable enable
using System;
using UnityEngine;

namespace DarkFrontier.Items.Segments
{
    public class SegmentAuthoring : MonoBehaviour
    {
        public string slot = "";
        public SegmentPrototype? prototype;
        public string id = Guid.NewGuid().ToString();
        public new string name = "Segment";
    }
}
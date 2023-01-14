#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Items.Segments;

namespace DarkFrontier.Items.Structures
{
    [Serializable]
    public class StructureLayout
    {
        public readonly StructureLayoutSource source;
        public readonly StructurePrototype? prototype;
        public readonly StructureComponent? component;
        public readonly List<SegmentLayout>? segments;

        public StructureLayout(StructurePrototype prototype)
        {
            source = StructureLayoutSource.Prototype;
            this.prototype = prototype;
            segments = new List<SegmentLayout>();
            if (prototype.prefab != null)
            {
                var components = prototype.prefab.GetComponentsInChildren<SegmentComponent>();
                for (int i = 0, l = components.Length; i < l; i++)
                {
                    segments.Add(new SegmentLayout(components[i]));
                }
            }
        }
        
        public StructureLayout(StructureComponent component)
        {
            source = StructureLayoutSource.Component;
            var instance = component.Instance;
            if (instance != null)
            {
                prototype = instance.Prototype;
            }
            this.component = component;
            segments = new List<SegmentLayout>();
            var components = component.GetComponentsInChildren<SegmentComponent>();
            for (int i = 0, l = components.Length; i < l; i++)
            {
                segments.Add(new SegmentLayout(components[i]));
            }
        }
    }
}
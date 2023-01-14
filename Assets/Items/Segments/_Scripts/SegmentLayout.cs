#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Items.Equipment;

namespace DarkFrontier.Items.Segments
{
    [Serializable]
    public class SegmentLayout
    {
        public readonly SegmentLayoutSource source;
        public readonly SegmentPrototype? prototype;
        public readonly SegmentComponent? component;
        public readonly List<SegmentPrototype>? compatible;
        public readonly List<EquipmentLayout>? equipment;

        public SegmentLayout(SegmentPrototype prototype)
        {
            source = SegmentLayoutSource.Prototype;
            this.prototype = prototype;
            equipment = new List<EquipmentLayout>();
            if (prototype.prefab != null)
            {
                var components = prototype.prefab.GetComponentsInChildren<EquipmentComponent>();
                for (int i = 0, l = components.Length; i < l; i++)
                {
                    equipment.Add(new EquipmentLayout(components[i]));
                }
            }
        }
        
        public SegmentLayout(SegmentComponent component)
        {
            source = SegmentLayoutSource.Component;
            var instance = component.Instance;
            if (instance != null)
            {
                prototype = instance.Prototype;
            }
            this.component = component;
            compatible = new List<SegmentPrototype>(component.Compatible);
            equipment = new List<EquipmentLayout>();
            var components = component.GetComponentsInChildren<EquipmentComponent>();
            for (int i = 0, l = components.Length; i < l; i++)
            {
                equipment.Add(new EquipmentLayout(components[i]));
            }
        }
    }
}
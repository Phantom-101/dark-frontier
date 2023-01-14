#nullable enable
using System;
using System.Collections.Generic;

namespace DarkFrontier.Items.Equipment
{
    [Serializable]
    public class EquipmentLayout
    {
        public EquipmentLayoutSource source;
        public readonly EquipmentPrototype? prototype;
        public readonly EquipmentComponent? component;
        public readonly List<EquipmentPrototype>? compatible;
        
        public EquipmentLayout(EquipmentPrototype prototype)
        {
            source = EquipmentLayoutSource.Prototype;
            this.prototype = prototype;
        }
        
        public EquipmentLayout(EquipmentComponent component)
        {
            source = EquipmentLayoutSource.Component;
            var instance = component.Instance;
            if (instance != null)
            {
                prototype = instance.Prototype;
            }
            this.component = component;
            compatible = new List<EquipmentPrototype>(component.Compatible);
        }
    }
}
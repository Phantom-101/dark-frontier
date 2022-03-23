#nullable enable
using DarkFrontier.Data.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Capacitors
{
    public class CapacitorInstance : EquipmentInstance
    {
        public new CapacitorPrototype Prototype => (CapacitorPrototype)base.Prototype;

        [SerializeReference, JsonProperty("mutator")]
        public FloatAddMutator? mutator;
        
        public CapacitorInstance()
        {
        }
        
        public CapacitorInstance(CapacitorPrototype prototype) : base(prototype)
        {
        }
        
        public override void OnEnabled(EquipmentComponent component)
        {
            if(mutator != null || component.Segment == null || component.Segment.Structure == null) return;
            component.Segment.Structure.Instance?.Capacitor.Max.AddMutator(mutator = new FloatAddMutator(new MutableValue<float>(Prototype.capacitance), 0));
        }

        public override void OnDisabled(EquipmentComponent component)
        {
            if(mutator == null || component.Segment == null || component.Segment.Structure == null) return;
            component.Segment.Structure.Instance?.Capacitor.Max.RemoveMutator(mutator);
        }
    }
}
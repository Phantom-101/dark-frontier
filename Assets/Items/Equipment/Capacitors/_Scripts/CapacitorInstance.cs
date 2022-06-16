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
            mutator = new FloatAddMutator(new ConstantFloat(Prototype.capacitance), 0);
        }
        
        public override void OnEquipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.Capacitance.AddMutator(mutator);
        }

        public override void OnUnequipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.Capacitance.RemoveMutator(mutator);
        }
    }
}
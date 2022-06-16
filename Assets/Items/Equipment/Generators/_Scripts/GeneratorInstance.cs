#nullable enable
using DarkFrontier.Data.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Generators
{
    public class GeneratorInstance : EquipmentInstance
    {
        public new GeneratorPrototype Prototype => (GeneratorPrototype)base.Prototype;
        
        [SerializeReference, JsonProperty("mutator")]
        public FloatAddMutator? mutator;
        
        public GeneratorInstance()
        {
        }
        
        public GeneratorInstance(GeneratorPrototype prototype) : base(prototype)
        {
            mutator = new FloatAddMutator(new ConstantFloat(Prototype.generation), 0);
        }
        
        public override void OnEquipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.Generation.AddMutator(mutator);
        }

        public override void OnUnequipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.Generation.RemoveMutator(mutator);
        }
    }
}
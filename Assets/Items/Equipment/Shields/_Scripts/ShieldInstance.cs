#nullable enable
using DarkFrontier.Data.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Shields
{
    public class ShieldInstance : EquipmentInstance
    {
        public new ShieldPrototype Prototype => (ShieldPrototype)base.Prototype;
        
        [SerializeReference, JsonProperty("mutator-shielding")]
        public FloatAddMutator? shieldingMutator;
        
        [SerializeReference, JsonProperty("mutator-reinforcement")]
        public FloatAddMutator? reinforcementMutator;
        
        public ShieldInstance()
        {
        }
        
        public ShieldInstance(ShieldPrototype prototype) : base(prototype)
        {
            shieldingMutator = new FloatAddMutator(new ConstantValue<float>(Prototype.shielding), 0);
            reinforcementMutator = new FloatAddMutator(new ConstantValue<float>(Prototype.reinforcement), 0);
        }
        
        public override void OnEquipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.Shielding.AddMutator(shieldingMutator);
            component.Structure.Instance?.Reinforcement.AddMutator(reinforcementMutator);
        }

        public override void OnUnequipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.Shielding.RemoveMutator(shieldingMutator);
            component.Structure.Instance?.Reinforcement.RemoveMutator(reinforcementMutator);
        }
    }
}
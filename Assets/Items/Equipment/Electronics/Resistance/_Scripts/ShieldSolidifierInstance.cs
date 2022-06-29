#nullable enable
using DarkFrontier.Data.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Electronics.Resistance
{
    public class ShieldAmplifierInstance : EquipmentInstance
    {
        public new ShieldAmplifierPrototype Prototype => (ShieldAmplifierPrototype)base.Prototype;
        
        [SerializeReference, JsonProperty("mutator-field")]
        public FloatAddMutator? fieldMutator;
        
        [SerializeReference, JsonProperty("mutator-explosive")]
        public FloatAddMutator? explosiveMutator;
        
        [SerializeReference, JsonProperty("mutator-particle")]
        public FloatAddMutator? particleMutator;
        
        [SerializeReference, JsonProperty("mutator-kinetic")]
        public FloatAddMutator? kineticMutator;
        
        public ShieldAmplifierInstance()
        {
        }
        
        public ShieldAmplifierInstance(ShieldAmplifierPrototype prototype) : base(prototype)
        {
            fieldMutator = new FloatAddMutator(new ConstantFloat(Prototype.amplification), 0);
            explosiveMutator = new FloatAddMutator(new ConstantFloat(Prototype.amplification), 0);
            particleMutator = new FloatAddMutator(new ConstantFloat(Prototype.amplification), 0);
            kineticMutator = new FloatAddMutator(new ConstantFloat(Prototype.amplification), 0);
        }
        
        public override void OnEquipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.ShieldFieldResist.AddMutator(fieldMutator);
            component.Structure.Instance?.ShieldExplosiveResist.AddMutator(explosiveMutator);
            component.Structure.Instance?.ShieldParticleResist.AddMutator(particleMutator);
            component.Structure.Instance?.ShieldKineticResist.AddMutator(kineticMutator);
        }

        public override void OnUnequipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.ShieldFieldResist.RemoveMutator(fieldMutator);
            component.Structure.Instance?.ShieldExplosiveResist.RemoveMutator(explosiveMutator);
            component.Structure.Instance?.ShieldParticleResist.RemoveMutator(particleMutator);
            component.Structure.Instance?.ShieldKineticResist.RemoveMutator(kineticMutator);
        }
    }
}
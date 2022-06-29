#nullable enable
using DarkFrontier.Data.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Electronics.Resistance
{
    public class AdaptivePlatingInstance : EquipmentInstance
    {
        public new HullEnhancerPrototype Prototype => (HullEnhancerPrototype)base.Prototype;
        
        [SerializeReference, JsonProperty("mutator-field")]
        public FloatAddMutator? fieldMutator;
        
        [SerializeReference, JsonProperty("mutator-explosive")]
        public FloatAddMutator? explosiveMutator;
        
        [SerializeReference, JsonProperty("mutator-particle")]
        public FloatAddMutator? particleMutator;
        
        [SerializeReference, JsonProperty("mutator-kinetic")]
        public FloatAddMutator? kineticMutator;
        
        public AdaptivePlatingInstance()
        {
        }
        
        public AdaptivePlatingInstance(HullEnhancerPrototype prototype) : base(prototype)
        {
            fieldMutator = new FloatAddMutator(new ConstantFloat(Prototype.adaptation), 0);
            explosiveMutator = new FloatAddMutator(new ConstantFloat(Prototype.adaptation), 0);
            particleMutator = new FloatAddMutator(new ConstantFloat(Prototype.adaptation), 0);
            kineticMutator = new FloatAddMutator(new ConstantFloat(Prototype.adaptation), 0);
        }
        
        public override void OnEquipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.HullFieldResist.AddMutator(fieldMutator);
            component.Structure.Instance?.HullExplosiveResist.AddMutator(explosiveMutator);
            component.Structure.Instance?.HullParticleResist.AddMutator(particleMutator);
            component.Structure.Instance?.HullKineticResist.AddMutator(kineticMutator);
        }

        public override void OnUnequipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.HullFieldResist.RemoveMutator(fieldMutator);
            component.Structure.Instance?.HullExplosiveResist.RemoveMutator(explosiveMutator);
            component.Structure.Instance?.HullParticleResist.RemoveMutator(particleMutator);
            component.Structure.Instance?.HullKineticResist.RemoveMutator(kineticMutator);
        }
    }
}
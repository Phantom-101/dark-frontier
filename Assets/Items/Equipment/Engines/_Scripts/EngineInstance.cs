#nullable enable
using DarkFrontier.Data.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Engines
{
    public class EngineInstance : EquipmentInstance
    {
        public new EnginePrototype Prototype => (EnginePrototype)base.Prototype;

        [SerializeReference, JsonProperty("mutator-speed-linear")]
        public FloatMaxMutator? linearSpeedMutator;
        
        [SerializeReference, JsonProperty("mutator-speed-angular")]
        public FloatMaxMutator? angularSpeedMutator;
        
        [SerializeReference, JsonProperty("mutator-acceleration-linear")]
        public FloatAddMutator? linearAccelerationMutator;
        
        [SerializeReference, JsonProperty("mutator-acceleration-angular")]
        public FloatAddMutator? angularAccelerationMutator;
        
        public EngineInstance()
        {
        }
        
        public EngineInstance(EnginePrototype prototype) : base(prototype)
        {
            linearSpeedMutator = new FloatMaxMutator(new ConstantFloat(Prototype.linearSpeed), 0);
            angularSpeedMutator = new FloatMaxMutator(new ConstantFloat(Prototype.angularSpeed), 0);
            linearAccelerationMutator = new FloatAddMutator(new ConstantFloat(Prototype.linearAcceleration), 0);
            angularAccelerationMutator = new FloatAddMutator(new ConstantFloat(Prototype.angularAcceleration), 0);
        }
        
        public override void OnEquipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.LinearSpeed.AddMutator(linearSpeedMutator);
            component.Structure.Instance?.AngularSpeed.AddMutator(angularSpeedMutator);
            component.Structure.Instance?.LinearAcceleration.AddMutator(linearAccelerationMutator);
            component.Structure.Instance?.AngularAcceleration.AddMutator(angularAccelerationMutator);
        }

        public override void OnUnequipped(EquipmentComponent component)
        {
            if(component.Structure == null) return;
            component.Structure.Instance?.LinearSpeed.RemoveMutator(linearSpeedMutator);
            component.Structure.Instance?.AngularSpeed.RemoveMutator(angularSpeedMutator);
            component.Structure.Instance?.LinearAcceleration.RemoveMutator(linearAccelerationMutator);
            component.Structure.Instance?.AngularAcceleration.RemoveMutator(angularAccelerationMutator);
        }
    }
}
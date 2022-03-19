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
    }
}
using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Equipment
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class EquipmentRecord
    {
        [JsonProperty]
        [field: SerializeReference]
        public string Name { get; private set; } = "";

        [JsonProperty]
        [field: SerializeReference]
        public EquipmentInstance Instance { get; private set; } = new();
        
        public EquipmentRecord()
        {
        }

        public EquipmentRecord(string name, EquipmentInstance instance)
        {
            Name = name;
            Instance = instance;
        }
    }
}

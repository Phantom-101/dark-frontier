using System;
using DarkFrontier.Items.Equipment._Scripts;
using Newtonsoft.Json;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Items.Equipment
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class EquipmentRecord
    {
        [JsonProperty]
        [field: SerializeReference]
        public string Name { get; private set; } = "";

        [JsonProperty]
        [field: SerializeReference]
        public EquipmentInstance Instance { get; private set; } = new EquipmentInstance();
    }
}
#nullable restore
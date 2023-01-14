#nullable enable
using System;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Equipment
{
    [Serializable]
    public class EquipmentAdaptor
    {
        [field: SerializeReference] [JsonProperty("id")]
        public string id = Guid.NewGuid().ToString();
        public EquipmentComponent slot = null!;
    }
}
#nullable enable
using System;
using DarkFrontier.Controllers.New;
using DarkFrontier.Factions;
using DarkFrontier.Positioning.Sectors;
using Newtonsoft.Json;
using UnityEngine;

namespace DarkFrontier.Items.Structures
{
    [Serializable, JsonObject(MemberSerialization.OptIn, IsReference = true)]
    public class StructureAdaptor
    {
        public StructureComponent slot = null!;
        
        [SerializeReference, JsonProperty("id")]
        public string id = Guid.NewGuid().ToString();
        
        [SerializeReference]
        public ISelectable? selected;

        [JsonProperty("selected-id")]
        public string? selectedId;
        
        [SerializeReference]
        public Faction? faction;

        [JsonProperty("faction-id")]
        public string? factionId;
        
        [SerializeField]
        public SectorComponent sector = null!;

        [JsonProperty("sector-id")]
        public string? sectorId;
        
        [SerializeReference, JsonProperty("controller")]
        public Controller controller = null!;
        
        [SerializeField, JsonProperty("linear-target")]
        public Vector3 linearTarget;
        
        [SerializeField, JsonProperty("angular-target")]
        public Vector3 angularTarget;
    }
}
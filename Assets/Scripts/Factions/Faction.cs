using DarkFrontier.Foundation.Extensions;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using System;
using UnityEngine;

namespace DarkFrontier.Factions {
    [Serializable]
    public class Faction : ISaveTo<FactionSaveData> {
        public string Name { get => name; }
        [SerializeField] private string name;
        public string Id { get => id; }
        [SerializeField] private string id;
        public long Wealth { get => wealth; }
        [SerializeField] private long wealth;
        public StringToFloatDictionary Relations { get => relations; }
        [SerializeField] private StringToFloatDictionary relations = new StringToFloatDictionary ();
        public StructureRegistry Property { get => property; }
        [SerializeField] private StructureRegistry property = new StructureRegistry ();

        public Faction () { }
        public Faction (FactionSaveData saveData) : this () {
            name = saveData.Name;
            id = saveData.Id;
            wealth = saveData.Wealth;
            relations = saveData.Relations;
        }

        public void ChangeWealth (long delta) { wealth += delta; }
        public bool HasWealth (long condition) { return wealth >= condition; }
        public float GetRelation (string factionId) { return factionId == null ? 0 : relations.TryGet (factionId, 0); }
        public void SetRelation (string factionId, float target) { if (factionId != null) { relations[factionId] = Mathf.Clamp (target, -1, 1); Singletons.Get<FactionManager> ().Registry.Find (factionId).SetRelationBack (Id, target); } }
        private void SetRelationBack (string factionId, float target) { if (factionId != null) { relations[factionId] = Mathf.Clamp (target, -1, 1); } }
        public void ChangeRelation (string factionId, float delta) { SetRelation (factionId, GetRelation (factionId) + delta); }
        public bool IsAlly (string factionId) { return GetRelation (factionId) > 0.75f; }
        public bool IsEnemy (string factionId) { return GetRelation (factionId) < -0.75f; }
        public bool IsNeutral (string factionId) { return !IsAlly (factionId) && !IsEnemy (factionId); }

        public FactionSaveData Save () {
            return new FactionSaveData {
                Name = name,
                Id = id,
                Wealth = wealth,
                Relations = relations,
            };
        }
    }

    [Serializable]
    public class FactionSaveData : ILoadTo<Faction> {
        public string Name;
        public string Id;
        public long Wealth;
        public StringToFloatDictionary Relations;

        public Faction Load () {
            return new Faction (this);
        }
    }
}
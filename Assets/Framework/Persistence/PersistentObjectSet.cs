#nullable enable
using System.Collections.Generic;
using System.Linq;
using Framework.Variables;
using UnityEngine;

namespace Framework.Persistence {
    [CreateAssetMenu(menuName = "Set/PersistentObject", fileName = "NewPersistentObjectSet")]
    public class PersistentObjectSet : ScriptableSet<PersistentObject> {
        public PersistentObject? Get(string id) {
            return value.FirstOrDefault(e => e.id == id);
        }

        public IEnumerable<PersistentData> Save() {
            return value.Select(e => e.Save());
        }
    }
}
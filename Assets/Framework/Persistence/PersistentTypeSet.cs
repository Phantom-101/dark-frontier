#nullable enable
using System.Collections.Generic;
using System.Linq;
using Framework.Variables;
using UnityEngine;

namespace Framework.Persistence {
    [CreateAssetMenu(menuName = "Set/PersistentType", fileName = "NewPersistentTypeSet")]
    public class PersistentTypeSet : ScriptableSet<PersistentType> {
        public PersistentType? Get(string id) {
            return value.FirstOrDefault(e => e.id == id);
        }

        public PersistentObject? Load(PersistentData data, Transform? parent = null) {
            var type = Get(data.typeId);
            if (type == null) {
                return null;
            }

            var ret = type.New(parent);
            ret.id = data.id;
            ret.Load(data);
            return ret;
        }

        public IEnumerable<PersistentObject?> Load(IEnumerable<PersistentData> data, Transform? parent = null) {
            return data.Select(e => Load(e, parent));
        }
    }
}
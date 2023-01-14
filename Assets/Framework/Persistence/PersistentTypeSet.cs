#nullable enable
using System.Collections.Generic;
using Framework.Sets;
using UnityEngine;

namespace Framework.Persistence
{
    [CreateAssetMenu(menuName = "Set/PersistentType", fileName = "NewPersistentTypeSet")]
    public class PersistentTypeSet : ScriptableSet<PersistentType>
    {
        public PersistentType? Get(string id)
        {
            for (int i = 0, l = items.Count; i < l; i++)
            {
                if (items[i].id == id)
                {
                    return items[i];
                }
            }
            return null;
        }

        public PersistentObject? Load(PersistentData data, Transform? parent = null)
        {
            var type = Get(data.typeId);
            if (type == null)
            {
                return null;
            }
            var ret = type.New(parent);
            ret.id = data.id;
            ret.Load(data);
            return ret;
        }

        public List<PersistentObject> Load(List<PersistentData> data, Transform? parent = null)
        {
            List<PersistentObject> ret = new();
            for (int i = 0, l = data.Count; i < l; i++)
            {
                var obj = Load(data[i], parent);
                if (obj != null)
                {
                    ret.Add(obj);
                }
            }
            return ret;
        }
    }
}
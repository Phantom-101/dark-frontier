#nullable enable
using System.Collections.Generic;
using Framework.Sets;
using UnityEngine;

namespace Framework.Persistence
{
    [CreateAssetMenu(menuName = "Set/PersistentObject", fileName = "NewPersistentObjectSet")]
    public class PersistentObjectSet : ScriptableSet<PersistentObject>
    {
        public PersistentObject? Get(string id)
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
        
        public List<PersistentData> Save()
        {
            var ret = new List<PersistentData>();
            for (int i = 0, l = items.Count; i < l; i++)
            {
                ret.Add(items[i].Save());
            }
            return ret;
        }
    }
}
#nullable enable
using System;

namespace Framework.Persistence
{
    [Serializable]
    public class PersistentReference<T> where T : PersistentObject
    {
        public T? reference;
        public string? id;
        public bool useId;

        public PersistentReference()
        {
        }

        public PersistentReference(T? obj)
        {
            Set(obj);
        }

        public PersistentReference(string? id)
        {
            Set(id);
        }

        public T? Get(PersistentObjectSet set)
        {
            return useId ? id == null ? null : (T?)set.Get(id) : reference;
        }
        
        public void Set(T? value)
        {
            reference = value;
            useId = false;
        }

        public void Set(string? value)
        {
            id = value;
            useId = true;
        }
    }
}

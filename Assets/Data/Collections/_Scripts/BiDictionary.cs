using System.Collections.Generic;

namespace DarkFrontier.Data.Collections
{
    public class BiDictionary<TKeyA, TKeyB>
    {
        private readonly Dictionary<TKeyA, TKeyB> _dictA = new();
        private readonly Dictionary<TKeyB, TKeyA> _dictB = new();

        public Dictionary<TKeyA, TKeyB>.KeyCollection AKeys => _dictA.Keys;
        public Dictionary<TKeyB, TKeyA>.KeyCollection BKeys => _dictB.Keys;
        
        public TKeyB this[TKeyA key]
        {
            get => _dictA[key];
            set => _dictA[key] = value;
        }

        public TKeyA this[TKeyB key]
        {
            get => _dictB[key];
            set => _dictB[key] = value;
        }

        public void Add(TKeyA a, TKeyB b)
        {
            if(_dictA.ContainsKey(a) || _dictB.ContainsKey(b)) return;
            _dictA.Add(a, b);
            _dictB.Add(b, a);
        }

        public void Set(TKeyA a, TKeyB b)
        {
            Remove(a);
            Remove(b);
            _dictA[a] = b;
            _dictB[b] = a;
        }

        public void Remove(TKeyA key)
        {
            if(!_dictA.ContainsKey(key)) return;
            _dictB.Remove(_dictA[key]);
            _dictA.Remove(key);
        }
        
        public void Remove(TKeyB key)
        {
            if(!_dictB.ContainsKey(key)) return;
            _dictA.Remove(_dictB[key]);
            _dictB.Remove(key);
        }

        public bool Contains(TKeyA key) => _dictA.ContainsKey(key);

        public bool Contains(TKeyB key) => _dictB.ContainsKey(key);
        
        public bool Contains(TKeyA a, TKeyB b) => _dictA.ContainsKey(a) && _dictB.ContainsKey(b);
    }
}
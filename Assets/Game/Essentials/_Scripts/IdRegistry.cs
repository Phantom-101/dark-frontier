#nullable enable
using System.Collections.Generic;
using DarkFrontier.Foundation.Extensions;

namespace DarkFrontier.Game.Essentials
{
    public class IdRegistry
    {
        private readonly Dictionary<string, object> _idToObj = new();
        private readonly Dictionary<object, string> _objToId = new();

        public void Register(string id, object obj)
        {
            Unregister(id);
            _idToObj[id] = obj;
            _objToId[obj] = id;
        }

        public void Unregister(string id)
        {
            if(!_idToObj.ContainsKey(id)) return;
            _objToId.Remove(_idToObj[id]);
            _idToObj.Remove(id);
        }

        public void Unregister(object obj)
        {
            if(!_objToId.ContainsKey(obj)) return;
            _idToObj.Remove(_objToId[obj]);
            _objToId.Remove(obj);
        }

        public bool Contains(string id) => _idToObj.ContainsKey(id);

        public bool Contains(object obj) => _objToId.ContainsKey(obj);

        public object? Get(string id) => _idToObj.TryGet(id, null);
        
        public T? Get<T>(string id) => (T?)_idToObj.TryGet(id, null);

        public string? Get(object obj) => _objToId.TryGet(obj, null);
    }
}

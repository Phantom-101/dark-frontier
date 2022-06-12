#nullable enable
using System.Collections.Generic;
using DarkFrontier.Foundation.Extensions;

namespace DarkFrontier.Game.Essentials
{
    public class IdRegistry
    {
        private readonly HashSet<IId> _registry = new();
        private readonly Dictionary<string, IId> _dictionary = new();

        public void Register(IId id)
        {
            Unregister(id.Id);
            _registry.Add(id);
            _dictionary[id.Id] = id;
        }

        public void Unregister(string id)
        {
            if(!_dictionary.ContainsKey(id)) return;
            _registry.Remove(_dictionary[id]);
            _dictionary.Remove(id);
        }

        public void Unregister(IId id)
        {
            _registry.Remove(id);
            _dictionary.Remove(id.Id);
        }

        public T? Get<T>(string id) where T : IId => (T?)_dictionary.TryGet(id, null);
    }
}

#nullable enable
using System;
using System.Collections.Generic;
using DarkFrontier.Foundation.Extensions;

namespace DarkFrontier.Items.Structures
{
    [Serializable]
    public class StructureRegistry
    {
        public HashSet<StructureComponent> Structures { get; private set; } = new();
        
        public Dictionary<string, StructureComponent> Lookup { get; private set; } = new();

        public bool Has(string id)
        {
            return Lookup.ContainsKey(id);
        }

        public bool Has(StructureComponent instance)
        {
            return Structures.Contains(instance);
        }
        
        public StructureComponent? Get(string id)
        {
            return Lookup.TryGet(id, null);
        }
        
        public bool Add(StructureComponent component)
        {
            if(Structures.Contains(component) && Lookup.ContainsKey(component.Instance!.Id)) return false;
            Structures.Add(component);
            Lookup.Add(component.Instance!.Id, component);
            return true;
        }

        public void Set(StructureComponent component)
        {
            Structures.Add(component);
            Lookup[component.Instance!.Id] = component;
        }

        public bool Remove(string id)
        {
            return Remove(Get(id));
        }
        
        public bool Remove(StructureComponent? component)
        {
            return component == null || Structures.Remove(component) && Lookup.Remove(component.Instance!.Id);
        }
    }
}
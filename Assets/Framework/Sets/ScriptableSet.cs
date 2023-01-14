using System.Collections.Generic;
using UnityEngine;

namespace Framework.Sets
{
    public class ScriptableSet<T> : ScriptableObject
    {
        public List<T> items = new();

        public void Add(T element)
        {
            if (!items.Contains(element))
            {
                items.Add(element);
            }
        }

        public void Remove(T element)
        {
            items.Remove(element);
        }
    }
}
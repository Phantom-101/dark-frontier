using System.Collections.Generic;
using DarkFrontier.Attributes;
using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
    public class NavigationRegistry : MonoBehaviour
    {
        [field: SerializeReference, ReadOnly]
        public List<NavigationCollider> Colliders { get; } = new();

        public void Add(NavigationCollider navigationCollider)
        {
            if(!Colliders.Contains(navigationCollider))
            {
                Colliders.Add(navigationCollider);
            }
        }
    }
}

using System.Collections.Generic;
using DarkFrontier.Attributes;
using UnityEngine;


namespace DarkFrontier.Positioning.Navigation
{
    public class NavigationRegistry : MonoBehaviour
    {
        [field: SerializeReference, ReadOnly]
        public List<NavigationCollider> Colliders { get; private set; } = new();

        public bool Add(NavigationCollider collider)
        {
            if(Colliders.Contains(collider))
            {
                return false;
            }

            Colliders.Add(collider);
            return true;
        }

        public bool Remove(NavigationCollider collider)
        {
            return Colliders.Remove(collider);
        }
    }
}

using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


namespace DarkFrontier.Positioning.Navigation
{
    public class NavigationRouteFinder : MonoBehaviour
    {
        public Transform? from;
        public Transform? to;

        [ReadOnly]
        public List<Vector3> route = new();

        public void Tick()
        {
            if(from != null && to != null)
            {
                route = FindObjectOfType<NavigationPathfinder>().GetRoute(from.position, to.position);
            }
        }

        private void OnDrawGizmos()
        {
            if(from != null && to != null)
            {
                Gizmos.color = Color.green;
                var current = from.transform.position;
                for(int i = 0, l = route.Count; i < l; i++)
                {
                    Gizmos.DrawLine(current, current = route[i]);
                }
            }
        }
    }
}

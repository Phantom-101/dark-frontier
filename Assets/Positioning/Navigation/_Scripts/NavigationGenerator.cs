using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
    public class NavigationGenerator : MonoBehaviour
    {
        [SerializeField]
        private float _colliders = 100;
        
        private void Start()
        {
            var registry = GetComponent<NavigationRegistry>();
            if(registry == null) return;
            
            for(var i = 0; i < _colliders; i++)
            {
                var obj = new GameObject
                {
                    name = "Collider",
                    transform =
                    {
                        position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100))
                    }
                };
                var col = obj.AddComponent<NavigationCollider>();
                col.extents = new Vector3(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10));
                registry.Add(col);
            }
        }
    }
}

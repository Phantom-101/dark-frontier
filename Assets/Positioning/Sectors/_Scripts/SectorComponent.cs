#nullable enable
using UnityEngine;

namespace DarkFrontier.Positioning.Sectors
{
    public class SectorComponent : MonoBehaviour
    {
        [field: SerializeReference]
        public SectorInstance? Instance { get; private set; }
        
        private void Start()
        {
            SectorSpawner.Create(this);
        }
        
        public bool Initialize()
        {
            return Instance != null && SetInstance(Instance);
        }
        
        public bool SetInstance(SectorInstance instance)
        {
            (Instance = instance).SetComponent(this);
            return true;
        }
    }
}

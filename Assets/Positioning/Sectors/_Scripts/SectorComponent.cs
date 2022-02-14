#nullable enable
using DarkFrontier.Foundation.Services;
using DarkFrontier.UI.Indicators.Selectors;
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
            var detectableRegistry = Singletons.Get<DetectableRegistry>();
            if(Instance != null)
            {
                detectableRegistry.Detectables.Remove(Instance);
            }
            
            (Instance = instance).SetComponent(this);
            detectableRegistry.Detectables.Add(instance);
            return true;
        }
    }
}

#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Segments;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;

namespace DarkFrontier.Items.Structures
{
    public class StructureComponent : MonoBehaviour
    {
        [field: SerializeReference]
        public StructureInstance? Instance { get; private set; }

        [field: SerializeReference, ReadOnly]
        public SegmentComponent[] Segments { get; private set; } = Array.Empty<SegmentComponent>();

        public bool Initialize()
        {
            return Instance != null && SetInstance(Instance);
        }
        
        public bool SetInstance(StructureInstance instance)
        {
            var detectableRegistry = Singletons.Get<DetectableRegistry>();
            if(Instance != null)
            {
                detectableRegistry.Detectables.Remove(Instance);
            }
            
            (Instance = instance).SetComponent(this);
            detectableRegistry.Detectables.Add(instance);
            
            if (Instance.Prototype.prefab != null)
            {
                Instantiate(Instance.Prototype.prefab, transform);
            }
            
            for (int i = 0, li = (Segments = GetComponentsInChildren<SegmentComponent>()).Length; i < li; i++)
            {
                Segments[i].structure = this;
                for(int j = 0, lj = Instance.Segments.Length; j < lj; j++)
                {
                    if(Segments[i].Name == Instance.Segments[j]?.Name)
                    {
                        if(!Segments[i].SetInstance(Instance.Segments[j]!.Instance))
                        {
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }
    }
}

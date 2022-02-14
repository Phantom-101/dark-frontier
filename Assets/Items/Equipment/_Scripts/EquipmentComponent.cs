#nullable enable
using System;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Segments;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;

namespace DarkFrontier.Items.Equipment._Scripts
{
    public class EquipmentComponent : MonoBehaviour
    {
        public SegmentComponent? segment;

        [field: SerializeReference]
        public EquipmentInstance? Instance { get; private set; }
        
        [field: SerializeReference]
        public string Name { get; private set; } = "";

        [field: SerializeReference, TextArea]
        public string Description { get; private set; } = "";

        [field: SerializeReference]
        public EquipmentPrototype[] Compatible { get; private set; } = Array.Empty<EquipmentPrototype>();
        
        public bool SetInstance(EquipmentInstance instance)
        {
            var detectableRegistry = Singletons.Get<DetectableRegistry>();
            if(Instance != null)
            {
                detectableRegistry.Detectables.Remove(Instance);
            }
            
            (Instance = instance).SetComponent(this);
            detectableRegistry.Detectables.Add(instance);
            
            if(Instance.Prototype.prefab != null)
            {
                Instantiate(Instance.Prototype.prefab, transform);
            }

            return true;
        }

        public bool RemoveInstance()
        {
            Instance?.RemoveComponent();
            Instance = null;
            return true;
        }
    }
}

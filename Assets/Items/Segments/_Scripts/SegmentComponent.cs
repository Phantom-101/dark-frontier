#nullable enable
using System;
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Equipment._Scripts;
using DarkFrontier.Items.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;

namespace DarkFrontier.Items.Segments
{
    public class SegmentComponent : MonoBehaviour
    {
        public StructureComponent? structure;

        [field: SerializeReference]
        public SegmentInstance? Instance { get; private set; }

        [field: SerializeReference]
        public string Name { get; private set; } = "";

        [field: SerializeReference, TextArea]
        public string Description { get; private set; } = "";

        [field: SerializeReference]
        public SegmentPrototype[] Compatible { get; private set; } = Array.Empty<SegmentPrototype>();

        [field: SerializeReference, ReadOnly]
        public EquipmentComponent[] Equipment { get; private set; } = Array.Empty<EquipmentComponent>();

        public bool SetInstance(SegmentInstance instance)
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
            
            for (int i = 0, li = (Equipment = GetComponentsInChildren<EquipmentComponent>()).Length; i < li; i++)
            {
                Equipment[i].segment = this;
                for(int j = 0, lj = Instance.Equipment.Length; j < lj; j++)
                {
                    if(Equipment[i].Name == Instance.Equipment[j]?.Name)
                    {
                        if(!Equipment[i].SetInstance(Instance.Equipment[j]!.Instance))
                        {
                            return false;
                        }
                    }
                }
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

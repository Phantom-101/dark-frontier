using System;
using DarkFrontier.Attributes;
using DarkFrontier.Items.Segments;
using DarkFrontier.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Items.Structures
{
    public class StructureComponent : MonoBehaviour, IInstanceInfo, IDamageable
    {
        [SerializeReference]
        public StructureInstance? instance;

        public ISelectable? Parent => null;

        public ISelectable[] Children => Segments;

        public Vector3 Position => transform.position;
        
        public IInfo? InstanceInfo => instance;
        
        public string Name => instance?.Prototype.name ?? "";

        public string Description => instance?.Prototype.description ?? "";

        [field: ReadOnly]
        [field: SerializeReference]
        public float MaxHp { get; private set; }

        public float CurrentHp => instance?.PoolHp ?? 0;

        [field: SerializeReference]
        public SegmentComponent[] Segments { get; private set; } = Array.Empty<SegmentComponent>();

        private void Start()
        {
            Initialize();
        }
        
        public bool Initialize()
        {
            if (instance == null) return false;

            instance.Component = this;

            if (instance.Prototype.prefab != null)
            {
                Instantiate(instance.Prototype.prefab, transform);
            }

            Segments = GetComponentsInChildren<SegmentComponent>();
            
            MaxHp = 0;
            
            for (int i = 0, l = Segments.Length; i < l; i++)
            {
                if (Segments[i].Initialize(this, instance.Segments))
                {
                    MaxHp += Segments[i].instance!.Prototype.poolHp;
                }
            }

            return true;
        }
        
        public bool IsDetected(StructureComponent structure)
        {
            return true;
        }

        public Selector? CreateSelector(Transform? root)
        {
            if (instance == null || instance.Prototype.selectorPrefab == null)
            {
                return null;
            }

            var selector = Instantiate(instance.Prototype.selectorPrefab, root).GetComponent<BasicSelector>();
            selector.selectable = this;

            return selector;
        }

        public void Inflict(Damage damage)
        {
            throw new NotImplementedException();
        }
    }
}
#nullable restore
using System;
using System.Collections.Generic;
using DarkFrontier.Attributes;
using DarkFrontier.Items.Segments;
using DarkFrontier.Items.Structures;
using DarkFrontier.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;


namespace DarkFrontier.Items.Equipment._Scripts
{
    public class EquipmentComponent : MonoBehaviour, IInstanceInfo, IDamageable
    {
        [field: SerializeReference]
        public SegmentComponent? Segment { get; private set; }

        [SerializeReference]
        public EquipmentInstance? instance;

        public ISelectable? Parent => Segment;

        public ISelectable[] Children => Array.Empty<ISelectable>();

        public Vector3 Position => transform.position;

        public IInfo? InstanceInfo => instance;

        [field: SerializeField]
        public string Name { get; private set; } = "";

        [field: TextArea]
        [field: SerializeReference]
        public string Description { get; private set; } = "";

        [field: ReadOnly]
        [field: SerializeReference]
        public float MaxHp { get; private set; }

        public float CurrentHp => instance?.Hp ?? 0;

        [field: SerializeReference]
        public EquipmentPrototype[] Compatible { get; private set; } = Array.Empty<EquipmentPrototype>();

        public bool Initialize(SegmentComponent segment, EquipmentRecord?[] records)
        {
            Segment = segment;

            for(int i = 0, l = records.Length; i < l; i++)
            {
                if(records[i] != null)
                {
                    if(records[i]!.Name == Name)
                    {
                        instance = records[i]!.Instance;
                        break;
                    }
                }
            }

            if(instance == null) return false;

            if(instance.Prototype.prefab != null)
            {
                Instantiate(instance.Prototype.prefab, transform);
            }

            MaxHp = instance.Prototype.hp;

            return true;
        }

        public bool IsDetected(StructureComponent structure) => Parent == null ? false : Parent.IsDetected(structure);

        public Selector? CreateSelector(Transform? root)
        {
            if(instance == null || instance.Prototype.selectorPrefab == null)
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

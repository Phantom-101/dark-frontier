using System;
using System.Collections.Generic;
using DarkFrontier.Attributes;
using DarkFrontier.Items.Equipment._Scripts;
using DarkFrontier.Items.Structures;
using DarkFrontier.Structures;
using DarkFrontier.UI.Indicators.Selectors;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Items.Segments
{
    public class SegmentComponent : MonoBehaviour, IInstanceInfo, IDamageable
    {
        [field: SerializeReference]
        public StructureComponent? Structure { get; private set; }

        [SerializeReference]
        public SegmentInstance? instance;

        public ISelectable? Parent => Structure;

        public ISelectable[] Children => Equipment;

        public Vector3 Position => transform.position;

        public IInfo? InstanceInfo => instance;

        [field: SerializeReference]
        public string Name { get; private set; } = "";

        [field: TextArea]
        [field: SerializeReference]
        public string Description { get; private set; } = "";

        [field: ReadOnly]
        [field: SerializeReference]
        public float MaxHp { get; private set; }

        public float CurrentHp => instance?.Hp ?? 0;

        [field: SerializeReference]
        public SegmentPrototype[] Compatible { get; private set; } = Array.Empty<SegmentPrototype>();

        [field: SerializeReference]
        public EquipmentComponent[] Equipment { get; private set; } = Array.Empty<EquipmentComponent>();

        public bool Initialize(StructureComponent structure, SegmentRecord?[] records)
        {
            Structure = structure;

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

            Equipment = GetComponentsInChildren<EquipmentComponent>();

            MaxHp = instance.Prototype.hp;

            for(int i = 0, l = Equipment.Length; i < l; i++)
            {
                if(!Equipment[i].Initialize(this, instance.Equipment))
                {
                    return false;
                }
            }

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
#nullable restore
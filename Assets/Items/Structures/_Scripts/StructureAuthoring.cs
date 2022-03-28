#nullable enable
using System;
using DarkFrontier.Items.Equipment;
using DarkFrontier.Items.Segments;
using DarkFrontier.Utils;
using UnityEngine;

namespace DarkFrontier.Items.Structures
{
    public class StructureAuthoring : MonoBehaviour
    {
        public StructurePrototype? prototype;
        public string id = Guid.NewGuid().ToString();
        public new string name = "Structure";
        public string faction = "";
        public string sector = "";
        public string selected = "";

        public void Author()
        {
            if(prototype == null) return;
            var component = gameObject.AddOrGet<StructureComponent>();
            component.Initialize();
            component.Set(new StructureInstance(this));
            component.Enable();
            
            var segments = GetComponentsInChildren<SegmentAuthoring>();
            for(int i = 0, li = segments.Length; i < li; i++)
            {
                component.Instance!.Equip(segments[i].slot, new SegmentInstance(segments[i]));
                var equipment = segments[i].GetComponentsInChildren<EquipmentAuthoring>();
                for(int j = 0, lj = equipment.Length; j < lj; j++)
                {
                    component.Instance!.Equip(segments[i].slot, equipment[i].slot, new EquipmentInstance(equipment[i]));
                    Destroy(equipment[i].gameObject);
                }
                Destroy(segments[i].gameObject);
            }
            Destroy(this);
        }
    }
}
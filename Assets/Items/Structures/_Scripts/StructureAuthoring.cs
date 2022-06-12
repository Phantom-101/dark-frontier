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
        public bool randomizeId = true;

        public void Author()
        {
            // Structure
            if(prototype == null) return;
            if(randomizeId) id = Guid.NewGuid().ToString();
            var component = gameObject.AddOrGet<StructureComponent>();
            component.Initialize();
            var instance = (StructureInstance)prototype.NewInstance();
            instance.Apply(this);
            component.Set(instance);
            component.Enable();
            
            // Segments
            var segments = GetComponentsInChildren<SegmentAuthoring>();
            for(int i = 0, li = segments.Length; i < li; i++)
            {
                if(segments[i].prototype != null)
                {
                    var segmentInstance = (SegmentInstance)segments[i].prototype!.NewInstance();
                    segmentInstance.Apply(segments[i]);
                    component.Equip(segments[i].slot, segmentInstance);
                    
                    // Equipment
                    var equipment = segments[i].GetComponentsInChildren<EquipmentAuthoring>();
                    for(int j = 0, lj = equipment.Length; j < lj; j++)
                    {
                        if(equipment[i].prototype != null)
                        {
                            var equipmentInstance = (EquipmentInstance)equipment[i].prototype!.NewInstance();
                            equipmentInstance.Apply(equipment[i]);
                            component.Equip(segments[i].slot, equipment[i].slot, equipmentInstance);
                        }
                        Destroy(equipment[i].gameObject);
                    }
                }
                Destroy(segments[i].gameObject);
            }
            Destroy(this);
        }
    }
}
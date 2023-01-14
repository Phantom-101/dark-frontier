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
        public float hp;
        public string faction = string.Empty;
        public string selected = string.Empty;
        public bool randomizeId = true;

        public void Author()
        {
            // TODO destroy authorings
            if(prototype == null) return;
            if(randomizeId) id = Guid.NewGuid().ToString();
            
            var instance = (StructureInstance)prototype.NewInstance();
            var adaptor = instance.NewAdaptor();
            adaptor.id = id;
            instance.Name = name;
            instance.CurrentHp = hp;
            adaptor.factionId = string.IsNullOrEmpty(faction) ? null : faction;
            adaptor.selectedId = string.IsNullOrEmpty(selected) ? null : selected;
            // TODO set adaptor sector id based on parent sector authoring
            
            var segments = GetComponentsInChildren<SegmentAuthoring>();
            for(int i = 0, li = segments.Length; i < li; i++)
            {
                if(segments[i].prototype != null)
                {
                    var segmentInstance = (SegmentInstance)segments[i].prototype!.NewInstance();
                    segmentInstance.Apply(segments[i]);
                    component.Equip(segments[i].slot, segmentInstance);
                    
                }
            }
            
            var component = gameObject.AddOrGet<StructureComponent>();
            component.Set(instance, adaptor);
        }
    }
}
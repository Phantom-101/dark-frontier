#nullable enable
using System;
using UnityEngine;

namespace DarkFrontier.Items.Equipment
{
    public class EquipmentAuthoring : MonoBehaviour
    {
        public string slot = "";
        public EquipmentPrototype? prototype;
        public string id = Guid.NewGuid().ToString();
        public new string name = "Equipment";
    }
}
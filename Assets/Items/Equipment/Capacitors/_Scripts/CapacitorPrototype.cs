using DarkFrontier.Data.Values;
using UnityEngine;

namespace DarkFrontier.Items.Equipment.Capacitors
{
    [CreateAssetMenu(menuName = "Items/Prototypes/Equipment/Capacitors/Capacitor")]
    public class CapacitorPrototype : EquipmentPrototype
    {
        public float capacitance;
        
        public override void OnEquipped(EquipmentComponent component)
        {
            CheckType(component);
            var instance = (CapacitorInstance)component.Instance;
            if(instance == null || component.Segment == null || component.Segment.Structure == null) return;
            component.Segment.Structure.Instance?.Capacitor.Max.AddMutator(instance.mutator = new FloatAddMutator(new MutableValue<float>(capacitance), 0));
        }

        public override void OnUnequipped(EquipmentComponent component)
        {
            CheckType(component);
            var instance = (CapacitorInstance)component.Instance;
            if(instance == null || component.Segment == null || component.Segment.Structure == null) return;
            component.Segment.Structure.Instance?.Capacitor.Max.RemoveMutator(instance.mutator);
        }

        public override void CheckType(EquipmentComponent component)
        {
            if(component.Instance is CapacitorInstance) return;
            component.Set(new CapacitorInstance(this));
            component.Enable();
        }
    }
}
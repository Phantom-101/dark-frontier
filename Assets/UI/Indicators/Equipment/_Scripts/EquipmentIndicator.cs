#nullable enable
using DarkFrontier.Items.Equipment;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Indicators.Equipment
{
    public class EquipmentIndicator : VisualElement
    {
        public EquipmentComponent? Component { get; init; }
        
        public new class UxmlFactory : UxmlFactory<EquipmentIndicator, UxmlTraits>
        {
        }
    }
}

using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    public interface IDetectable
    {
        bool IsDetectedBy(StructureComponent detector);

        VisualElement CreateSelector();

        void UpdateSelector(VisualElement selector, bool selected);
    }
}
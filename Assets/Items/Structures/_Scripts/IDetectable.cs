using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    public interface IDetectable
    {
        bool IsDetected(StructureInstance detector);

        VisualElement CreateSelector();

        Vector3 GetSelectorPosition();

        VisualElement CreateSelected();
    }
}
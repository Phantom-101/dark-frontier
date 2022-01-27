using UnityEngine.UIElements;

namespace DarkFrontier.UI.Inspector
{
    public interface IInspectable
    {
        public VisualElement CreateInspector();
    }
}
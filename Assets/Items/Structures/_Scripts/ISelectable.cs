using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    public interface ISelectable
    {
        public string Id { get; }
        
        bool SelectorDirty { get; }
        
        bool CanBeSelectedBy(StructureComponent other);

        VisualElement CreateSelector();

        void UpdateSelector(bool selected);
    }
}
﻿using DarkFrontier.Game.Essentials;
using UnityEngine.UIElements;

namespace DarkFrontier.Items.Structures
{
    public interface ISelectable : IId
    {
        bool SelectorDirty { get; }
        
        bool CanBeSelectedBy(StructureComponent other);

        VisualElement CreateSelector();

        void UpdateSelector(bool selected);
    }
}
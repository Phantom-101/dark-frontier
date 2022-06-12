using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Indicators.Interactions
{
    public class InteractionList : VisualElement
    {
        private readonly InteractionEntry[] _entries = new InteractionEntry[5];
        private readonly List<IInteraction> _interactions = new();

        public InteractionList()
        {
            for(int i = 0, l = _entries.Length; i < l; i++)
            {
                Add(_entries[i] = new InteractionEntry());
            }
        }
        
        public void AddInteraction(IInteraction interaction)
        {
            _interactions.Add(interaction);
            if(_interactions.Count > 5)
            {
                _interactions.RemoveAt(0);
            }
            UpdateInteractions();
            schedule.Execute(() => TimeoutInteraction(interaction)).StartingIn(5000);
        }

        private void TimeoutInteraction(IInteraction interaction)
        {
            _interactions.Remove(interaction);
            UpdateInteractions();
        }

        private void UpdateInteractions()
        {
            for(int i = 0, l = _entries.Length - _interactions.Count; i < l; i++)
            {
                _entries[i].Reset();
            }
            for(int i = 0, l = _interactions.Count; i < l; i++)
            {
                _entries[i + _entries.Length - l].Set(_interactions[i]);
            }
        }
        
        public new class UxmlFactory : UxmlFactory<InteractionList, UxmlTraits>
        {
        }
    }
}
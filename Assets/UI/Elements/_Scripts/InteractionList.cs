using UnityEngine.UIElements;

namespace DarkFrontier.UI.Elements
{
    public class InteractionList : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InteractionList, UxmlTraits>
        {
        }

        public void AddInteraction(Interaction interaction)
        {
            if (childCount >= 5)
            {
                RemoveAt(0);
            }
            Add(interaction);
            schedule.Execute(() => TimeoutInteraction(interaction)).StartingIn(5000);
        }

        private void TimeoutInteraction(Interaction interaction)
        {
            Remove(interaction);
        }
    }
}
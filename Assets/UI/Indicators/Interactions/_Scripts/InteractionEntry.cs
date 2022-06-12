using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Indicators.Interactions
{
    public class InteractionEntry : VisualElement
    {
        private readonly Label _text;
        
        public InteractionEntry()
        {
            _text = new Label
            {
                style =
                {
                    width = new Length(100, LengthUnit.Percent),
                    marginLeft = 0,
                    marginRight = 0,
                    marginTop = 8,
                    marginBottom = 8,
                    paddingLeft = 0,
                    paddingRight = 0,
                    paddingTop = 0,
                    paddingBottom = 0,
                    //unityFont = 
                    fontSize = 20,
                    color = Color.white,
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };
            Add(_text);
        }

        public void Reset()
        {
            _text.text = string.Empty;
        }

        public void Set(IInteraction interaction)
        {
            _text.text = interaction.Text;
        }
        
        public new class UxmlFactory : UxmlFactory<InteractionEntry, UxmlTraits>
        {
        }
    }
}
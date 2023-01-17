using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Elements
{
    public class Interaction : VisualElement
    {
        public Interaction(Font font, string text, float margin = 8)
        {
            var label = new Label
            {
                style =
                {
                    unityFont = font,
                    fontSize = 20,
                    color = Color.white,
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };
            label.style.SetMargin(0, 0, margin, margin);
            label.style.SetPadding(0, 0, 0, 0);
            label.text = text;
            Add(label);
        }
    }
}

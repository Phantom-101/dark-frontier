#nullable enable
using System.Collections.Generic;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Structures;
using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Indicators.Selectors
{
    public class SelectorDocument : MonoBehaviour
    {
        private UIDocument _document = null!;
        private DetectableRegistry _registry = null!;

        private Dictionary<IDetectable, VisualElement> _selectors = new();

        private void Start()
        {
            _document = ComponentUtils.AddOrGet<UIDocument>(gameObject);
            _registry = Singletons.Get<DetectableRegistry>();
        }

        private void Update()
        {
            for(int i = 0, l = _registry.Detectables.Count; i < l; i++)
            {
                if(!_selectors.ContainsKey(_registry.Detectables[i]))
                {
                    var selector = _registry.Detectables[i].CreateSelector();
                    _document.rootVisualElement.Add(selector);
                    _selectors[_registry.Detectables[i]] = selector;
                }
                var position = _registry.Detectables[i].GetSelectorPosition();
                var element = _selectors[_registry.Detectables[i]];
                if(position.z > 0)
                {
                    element.style.visibility = Visibility.Visible;
                    element.style.left = new StyleLength(new Length(position.x * 100, LengthUnit.Percent));
                    element.style.top = new StyleLength(new Length(100 - position.y * 100, LengthUnit.Percent));
                }
                else
                {
                    element.style.visibility = Visibility.Hidden;
                }
            }
        }
    }
}

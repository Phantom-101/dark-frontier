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

        private IDetectable? _selected;

        private readonly Dictionary<IDetectable, VisualElement> _selectors = new();
        
        private void Start()
        {
            _document = gameObject.AddOrGet<UIDocument>();
            _registry = Singletons.Get<DetectableRegistry>();
        }

        private void Update()
        {
            TrimSelectors();
            UpdateSelectors();
        }

        private void TrimSelectors()
        {
            foreach(IDetectable key in _selectors.Keys)
            {
                if(key == null)
                {
                    Destroy(key!, _selectors[key!]);
                }
            }
        }

        private void UpdateSelectors()
        {
            // ReSharper disable once TooWideLocalVariableScope
            IDetectable cur;
            for(int i = 0, l = _registry.Registry.Count; i < l; i++)
            {
                cur = _registry.Registry[i];
                if(_selectors.ContainsKey(cur))
                {
                    Update(cur, _selectors[cur]);
                }
                else
                {
                    var element = Create(cur);
                    Update(cur, element);
                }
            }
        }
        
        private VisualElement Create(IDetectable detectable)
        {
            var selector = detectable.CreateSelector();
            _document.rootVisualElement.Add(selector);
            selector.Q("unselected").RegisterCallback<ClickEvent, IDetectable>(OnClick, detectable);
            _selectors[detectable] = selector;
            return selector;
        }

        private void Destroy(IDetectable detectable, VisualElement selector)
        {
            _document.rootVisualElement.Remove(selector);
            _selectors.Remove(detectable);
        }

        private void Update(IDetectable detectable, VisualElement selector)
        {
            detectable.UpdateSelector(selector, _selected == detectable);
        }
        
        private void OnClick(ClickEvent evt, IDetectable detectable)
        {
            _selected = detectable;
        }
    }
}

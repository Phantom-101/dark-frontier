#nullable enable
using System.Collections.Generic;
using DarkFrontier.Controllers.New;
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
        private PlayerController _playerController = null!;

        private readonly Dictionary<ISelectable, VisualElement> _selectors = new();

        private void Start()
        {
            _document = gameObject.AddOrGet<UIDocument>();
            _registry = Singletons.Get<DetectableRegistry>();
            _playerController = Singletons.Get<PlayerController>();
        }

        private void Update()
        {
            TrimSelectors();
            UpdateSelectors();
        }

        private void TrimSelectors()
        {
            foreach(ISelectable key in _selectors.Keys)
            {
                if(key == null || key.SelectorDirty)
                {
                    Destroy(key!, _selectors[key!]);
                }
            }
        }

        private void UpdateSelectors()
        {
            // ReSharper disable once TooWideLocalVariableScope
            ISelectable cur;
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

        private VisualElement Create(ISelectable selectable)
        {
            var selector = selectable.CreateSelector();
            _document.rootVisualElement.Add(selector);
            selector.Q("unselected").RegisterCallback<ClickEvent, ISelectable>(OnClick, selectable);
            _selectors[selectable] = selector;
            return selector;
        }

        private void Destroy(ISelectable selectable, VisualElement selector)
        {
            _document.rootVisualElement.Remove(selector);
            _selectors.Remove(selectable);
        }

        private void Update(ISelectable selectable, VisualElement selector)
        {
            selectable.UpdateSelector(_playerController.Player != null && _playerController.Player.Instance?.Selected == selectable);
        }

        private void OnClick(ClickEvent evt, ISelectable selectable)
        {
            if(_playerController.Player != null && _playerController.Player.Instance != null)
            {
                _playerController.Player.Instance.Selected = selectable;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Controllers.New;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Items.Equipment;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Indicators.Equipment
{
    public class EquipmentList : VisualElement
    {
        private PlayerController _playerController;

        private EquipmentComponent[] _components;
        private readonly Dictionary<EquipmentComponent, VisualElement> _indicators = new();

        public void Initialize()
        {
            _playerController = Singletons.Get<PlayerController>();
        }
        
        public void Tick()
        {
            _components = _playerController.Player == null ? Array.Empty<EquipmentComponent>() : _playerController.Player.Equipment ?? Array.Empty<EquipmentComponent>();
            TrimIndicators();
            UpdateIndicators();
        }

        private void TrimIndicators()
        {
            foreach(var key in _indicators.Keys)
            {
                if(key == null || key.IndicatorDirty || !_components.Contains(key))
                {
                    Destroy(key!);
                }
            }
        }

        private void UpdateIndicators()
        {
            // ReSharper disable once TooWideLocalVariableScope
            EquipmentComponent cur;
            for(int i = 0, l = _components.Length; i < l; i++)
            {
                cur = _components[i];
                if(_indicators.ContainsKey(cur))
                {
                    Update(cur);
                }
                else
                {
                    Create(cur);
                    Update(cur);
                }
            }
        }
        
        private void Create(EquipmentComponent component)
        {
            var indicator = component.CreateIndicator();
            Add(indicator);
            _indicators[component] = indicator;
        }

        private void Destroy(EquipmentComponent component)
        {
            Remove(_indicators[component]);
            _indicators.Remove(component);
        }

        private void Update(EquipmentComponent component)
        {
            component.UpdateIndicator();
        }
        
        public new class UxmlFactory : UxmlFactory<EquipmentList, UxmlTraits>
        {
        }
    }
}

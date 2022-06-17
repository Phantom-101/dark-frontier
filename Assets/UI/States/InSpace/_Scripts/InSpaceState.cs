#nullable enable
using DarkFrontier.Controllers.New;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Input;
using DarkFrontier.UI.Indicators.Equipment;
using DarkFrontier.UI.Indicators.Interactions;
using DarkFrontier.UI.Indicators.Modifiers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.States.InSpace
{
    public class InSpaceState : New.UIState
    {
        public OnScreenAxis forward = null!;
        public OnScreenAxis backward = null!;
        public OnScreenVector2 strafe = null!;
        public OnScreenVector2 turn = null!;
        public OnScreenAxis right = null!;
        public OnScreenAxis left = null!;

        private RadialProgressBar _hull = null!;
        private RadialProgressBar _shield = null!;
        private RadialProgressBar _capacitor = null!;
        private RadialProgressBar _speed = null!;
        private Slider _acceleration = null!;
        private EquipmentList _equipment = null!;
        // private ModifierList _modifiers = null!;
        private InteractionList _interactions = null!;

        private PlayerController _playerController = null!;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _hull = document.rootVisualElement.Q<RadialProgressBar>("hull");
            _shield = document.rootVisualElement.Q<RadialProgressBar>("shield");
            _capacitor = document.rootVisualElement.Q<RadialProgressBar>("capacitor");
            _speed = document.rootVisualElement.Q<RadialProgressBar>("speed");
            _acceleration = document.rootVisualElement.Q<Slider>("acceleration");
            _playerController = Singletons.Get<PlayerController>();
            _equipment = document.rootVisualElement.Q<EquipmentList>("equipment");
            _equipment.Initialize();
            _interactions = document.rootVisualElement.Q<InteractionList>("interactions");
        }

        public override void OnStateRemain()
        {
            if(_playerController.Player != null && _playerController.Player.Instance != null)
            {
                var component = _playerController.Player;
                var instance = component.Instance;
                _hull.Value = instance.MaxHp == 0 ? 0 : Mathf.Clamp01(instance.CurrentHp / instance.MaxHp) / 4;
                _shield.Value = instance.Shielding.Value == 0 ? 0 : Mathf.Clamp01(instance.Shield / instance.Shielding.Value) / 6;
                _capacitor.Value = instance.Capacitance.Value == 0 ? 0 : Mathf.Clamp01(instance.Capacitor / instance.Capacitance.Value) / 4;
                _speed.Value = instance.LinearSpeed.Value == 0 ? 0 : Mathf.Clamp01(component.rigidbody.velocity.magnitude / instance.LinearSpeed.Value) / 6;
            }
            else
            {
                _hull.Value = 0;
                _shield.Value = 0;
                _capacitor.Value = 0;
                _speed.Value = 0;
            }
            _hull.MarkDirtyRepaint();
            _shield.MarkDirtyRepaint();
            _capacitor.MarkDirtyRepaint();
            _speed.MarkDirtyRepaint();
            forward.Send(_acceleration.value > 0 ? _acceleration.value : 0);
            backward.Send(_acceleration.value > 0 ? 0 : -_acceleration.value);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if(Mouse.current != null)
            {
                var pos = Mouse.current.position.ReadValue();
                var mid = new Vector2(Display.main.renderingWidth, Display.main.renderingHeight) / 2;
                var dist = (pos - mid).magnitude;
                var clamped = Mathf.Clamp(dist - 100, 0, 500);
                var norm = (pos - mid).normalized;
                turn.Send(norm * clamped / 500);
            }
            _equipment.Tick();
        }
    }
}

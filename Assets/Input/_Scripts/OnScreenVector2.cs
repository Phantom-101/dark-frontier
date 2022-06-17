using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace DarkFrontier.Input
{
    public class OnScreenVector2 : OnScreenControl
    {
        [SerializeField, InputControl(layout = "Vector2")]
        private string _controlPath;

        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        public void Send(Vector2 value) => SendValueToControl(value);
    }
}

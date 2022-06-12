using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace DarkFrontier.Input
{
    public class OnScreenAxis : OnScreenControl
    {
        [SerializeField, InputControl(layout = "Axis")]
        private string _controlPath;

        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }

        public void Send(float value) => SendValueToControl(value);
    }
}
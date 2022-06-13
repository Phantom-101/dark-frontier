using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.States.New
{
    public class UIState : MonoBehaviour
    {
        public UIDocument document;

        public virtual void OnStateEnter()
        {
            document.rootVisualElement.style.opacity = 0;
            document.rootVisualElement.style.AddTransition("opacity", 0, 0.5f, EasingMode.Ease);
            document.rootVisualElement.schedule.Execute(() => document.rootVisualElement.style.opacity = 1);
        }

        public virtual void OnStateRemain()
        {
        }

        public virtual void OnStateExit()
        {
            document.rootVisualElement.style.opacity = 1;
            document.rootVisualElement.style.AddTransition("opacity", 0, 0.5f, EasingMode.Ease);
            document.rootVisualElement.schedule.Execute(() => document.rootVisualElement.style.opacity = 0);
        }
    }
}
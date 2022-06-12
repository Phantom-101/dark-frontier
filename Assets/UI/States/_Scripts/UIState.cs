using System.Collections;
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
            StartCoroutine(ChangeOpacity(1));
        }

        public virtual void OnStateRemain()
        {
        }

        public virtual void OnStateExit()
        {
            StartCoroutine(ChangeOpacity(0));
        }

        private IEnumerator ChangeOpacity(float target)
        {
            yield return null;
            document.rootVisualElement.style.opacity = target;
        }
    }
}
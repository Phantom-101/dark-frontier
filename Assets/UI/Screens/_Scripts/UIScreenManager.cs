using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.UI.Screens
{
    public class UIScreenManager : MonoBehaviour
    {
        public bool Pop()
        {
            return true;
        }
        
        public bool Push(VisualElement element)
        {
            return true;
        }

        public bool PushReplacement(VisualElement element)
        {
            return Pop() && Push(element);
        }

        public UIDocument NewDocument()
        {
            var instantiated = new GameObject("New Document");
            instantiated.transform.SetParent(transform);
            return instantiated.AddComponent<UIDocument>();
        }
    }
}
using DarkFrontier.UI.Inspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Utils
{
#if UNITY_EDITOR
    public class InspectorUtils : EditorWindow
    {
        [MenuItem("Utils/Inspector")]
        public static void OpenWindow()
        {
            var window = GetWindow<InspectorUtils>();
            window.titleContent = new GUIContent("Inspector Utils");
        }

        private void OnEnable()
        {
            var box = new Box();
            rootVisualElement.Add(box);

            var scripts = FindObjectsOfType<MonoBehaviour>();
            for(int i = 0, l = scripts.Length; i < l; i++)
            {
                if(scripts[i] is IInspectable inspectable)
                {
                    box.Add(inspectable.CreateInspector());
                }
            }
        }
    }
#endif
}
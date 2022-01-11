using UnityEditor;
using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(NavigationParameters))]
    public class NavigationParametersEditor : Editor
    {
        private NavigationParameters _component;

        private void OnEnable()
        {
            _component = (NavigationParameters)target;
        }

        private void OnSceneGUI()
        {
            Handles.color = new Color(1, 1, 0, .5f);
            Handles.DrawSolidArc(_component.transform.position, _component.transform.up, Vector3.forward, _component.Azimuth, 10);
            Handles.DrawSolidArc(_component.transform.position, -_component.transform.right, Vector3.forward, _component.Altitude, 10);
        }
    }
#endif
}

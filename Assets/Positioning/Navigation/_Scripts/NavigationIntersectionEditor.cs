using UnityEditor;
using UnityEngine;


namespace DarkFrontier.Positioning.Navigation
{
#if UNITY_EDITOR
    [CustomEditor(typeof(NavigationIntersection))]
    public class NavigationIntersectionEditor : Editor
    {
        private NavigationIntersection _component = null!;

        private void OnEnable()
        {
            _component = (NavigationIntersection)target;
        }

        private void OnSceneGUI()
        {
            if(_component.from != null && _component.to != null && _component.box != null)
            {
                Handles.color = new Color(1, 1, 0, .5f);
                Handles.DrawLine(_component.from.position, _component.to.position);
                Handles.color = _component.Intersects ? new Color(0, 1, 0, .5f) : new Color(1, 0, 0, .5f);
                var bounds = _component.box.bounds;
                Handles.DrawWireCube(bounds.center, bounds.size);
            }
        }
    }
#endif
}

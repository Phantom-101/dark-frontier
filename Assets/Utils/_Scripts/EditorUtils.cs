using System;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Utils
{
#if UNITY_EDITOR
    public static class EditorUtils
    {
        public static Toggle Toggle(string label = null, bool value = false)
        {
            return new Toggle(label)
            {
                value = value
            };
        }
        
        public static ObjectField Texture2DField(string label = null, Texture2D value = null)
        {
            return new ObjectField(label)
            {
                objectType = typeof(Texture2D),
                value = value
            };
        }

        public static Button Button(string label = null, Action action = null)
        {
            return new Button(action)
            {
                text = label
            };
        }
    }
#endif
}
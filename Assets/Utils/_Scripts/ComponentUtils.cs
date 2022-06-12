#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkFrontier.Utils
{
    public static class ComponentUtils
    {
        public static T AddOrGet<T>() where T : Component
        {
            var component = Object.FindObjectOfType<T>();
            return component == null ? new GameObject().AddComponent<T>() : component;
        }

        public static TInterface AddOrGet<TInterface, TComponent>() where TComponent : Component, TInterface
        {
            var components = FindComponentsOfType<TInterface>();
            return components.Count == 0 ? new GameObject().AddComponent<TComponent>() : components[0];
        }
        
        public static T AddOrGet<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            return component == null ? gameObject.AddComponent<T>() : component;
        }

        public static TInterface AddOrGet<TInterface, TComponent>(this GameObject gameObject) where TComponent : Component, TInterface
        {
            var component = gameObject.GetComponent<TInterface>();
            return component == null ? gameObject.AddComponent<TComponent>() : component;
        }

        public static List<T> FindComponentsOfType<T>()
        {
            var ret = new List<T>();
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            for(int i = 0, li = rootGameObjects.Length; i < li; i++)
            {
                var interfaces = rootGameObjects[i].GetComponentsInChildren<T>();
                for(int j = 0, lj = interfaces.Length; j < lj; j++)
                {
                    ret.Add(interfaces[j]);
                }
            }
            return ret;
        }
    }
}
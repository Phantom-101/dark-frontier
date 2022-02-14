using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace DarkFrontier.Utils
{
    public static class ComponentUtils
    {
        public static T AddOrGet<T>(GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if(component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        public static bool AddOrGet<T>(GameObject gameObject, out T component) where T : Component
        {
            component = gameObject.GetComponent<T>();
            if(component == null)
            {
                component = gameObject.AddComponent<T>();
                return false;
            }

            return true;
        }

        public static TInterface AddOrGet<TInterface, TComponent>(GameObject gameObject)
            where TComponent : Component, TInterface
        {
            TInterface component = gameObject.GetComponent<TInterface>();
            if(component == null)
            {
                component = gameObject.AddComponent<TComponent>();
            }

            return component;
        }

        public static bool AddOrGet<TInterface, TComponent>(GameObject gameObject, out TInterface component)
            where TComponent : Component, TInterface
        {
            component = gameObject.GetComponent<TInterface>();
            if(component == null)
            {
                component = gameObject.AddComponent<TComponent>();
                return false;
            }

            return true;
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
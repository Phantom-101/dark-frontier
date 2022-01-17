using UnityEngine;

#nullable enable
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

        public static TInterface AddOrGet<TInterface, TComponent>(GameObject gameObject) where TComponent : Component, TInterface
        {
            TInterface component = gameObject.GetComponent<TInterface>();
            if(component == null)
            {
                component = gameObject.AddComponent<TComponent>();
            }
            return component;
        }

        public static bool AddOrGet<TInterface, TComponent>(GameObject gameObject, out TInterface component) where TComponent : Component, TInterface
        {
            component = gameObject.GetComponent<TInterface>();
            if(component == null)
            {
                component = gameObject.AddComponent<TComponent>();
                return false;
            }
            return true;
        }
    }
}
#nullable restore
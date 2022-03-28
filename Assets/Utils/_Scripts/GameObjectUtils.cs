#nullable enable
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace DarkFrontier.Utils
{
    [Il2CppEagerStaticClassConstruction]
    public static class GameObjectUtils
    {
        public static void DestroyChildren(this Transform transform)
        {
            for(int l = transform.childCount, i = l - 1; i >= 0; i--)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
            transform.DetachChildren();
        }
    }
}
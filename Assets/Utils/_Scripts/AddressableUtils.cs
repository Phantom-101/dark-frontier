using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable enable
namespace DarkFrontier.Utils
{
    public static class AddressableUtils
    {
        public static void Destroy(GameObject obj)
        {
            if(!Addressables.ReleaseInstance(obj))
            {
                Object.Destroy(obj);
            }
        }
    }
}
#nullable restore
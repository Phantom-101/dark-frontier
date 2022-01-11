using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable enable
namespace DarkFrontier
{
    public class AddressableUtils
    {
        public static void Destroy(GameObject obj)
        {
            if(!Addressables.ReleaseInstance(obj))
            {
                Destroy(obj);
            }
        }
    }
}
#nullable restore
using DarkFrontier.Foundation.Services;
using UnityEngine;

namespace DarkFrontier.UI.States
{
    public class UIAuthoring : MonoBehaviour
    {
        public New.UIState asset;
        
        private void Start()
        {
            Singletons.Get<UIStack>().Push(asset);
            Destroy(this);
        }
    }
}

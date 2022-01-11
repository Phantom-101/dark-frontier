using UnityEngine;

namespace DarkFrontier.UI.Indicators.Selectors
{
    public abstract class Selector : MonoBehaviour
    {
        public virtual void Initialize()
        {
        }

        public virtual void Tick(float dt)
        {
        }
    }
}
using UnityEngine;

namespace DarkFrontier.Objects.Components
{
    public class HitBox : MonoBehaviour
    {
        public Destructible destructible;

        private void OnEnable()
        {
            destructible = GetComponentInParent<Destructible>();
            destructible.Add(this);
        }

        private void OnDisable()
        {
            destructible.Remove(this);
        }
    }
}
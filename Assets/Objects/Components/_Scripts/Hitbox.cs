using UnityEngine;

namespace DarkFrontier.Objects.Components
{
    public class Hitbox : MonoBehaviour
    {
        public Collider volume;
        public Destructible destructible;

        private void OnEnable()
        {
            volume = GetComponent<Collider>();
            destructible = GetComponentInParent<Destructible>();
            destructible.Add(this);
        }

        private void OnDisable()
        {
            destructible.Remove(this);
        }

        public void SetActive(bool target)
        {
            volume.enabled = target;
        }
    }
}
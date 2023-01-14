#nullable enable
using Framework.Channels;
using UnityEngine;

namespace DarkFrontier.Objects
{
    public class DestructibleObject : SelectableObject
    {
        public FloatChannel onTickChannel = null!;
        public float health;
        public float maxHealth;

        protected virtual void OnTick(object sender, float deltaSeconds)
        {
            if (health <= 0)
            {
                HealthDepleted();
            }
        }

        public virtual void ApplyHeal(float heal)
        {
            health = Mathf.Min(health + heal, maxHealth);
        }

        public virtual void ApplyDamage(float damage)
        {
            health -= damage;
        }

        public virtual void HealthDepleted()
        {
            Destroy(gameObject);
        }
    }
}
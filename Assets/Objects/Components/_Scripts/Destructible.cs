#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkFrontier.Objects.Components
{
    public class Destructible : ObjectComponent
    {
        public float health;
        public float maxHealth;
        public List<HitBox> hitBoxes = new();

        public float HealthPercent => health / maxHealth;

        public event EventHandler<float>? OnHealthChanged;

        public void Add(HitBox hitBox)
        {
            if (!hitBoxes.Contains(hitBox))
            {
                hitBoxes.Add(hitBox);
            }
        }

        public void Remove(HitBox hitBox)
        {
            hitBoxes.Remove(hitBox);
        }

        public virtual float ApplyDamage(float damage)
        {
            if (health == 0)
            {
                return damage;
            }
            var delta = Mathf.Min(damage, health);
            health -= delta;
            RaiseHealthChangedEvent(delta);
            return damage - delta;
        }

        public virtual float ApplyHeal(float heal)
        {
            if (health == 0)
            {
                return heal;
            }
            var delta = Mathf.Min(heal, maxHealth - health);
            health += delta;
            RaiseHealthChangedEvent(delta);
            return heal - delta;
        }

        protected void RaiseHealthChangedEvent(float delta)
        {
            OnHealthChanged?.Invoke(this, delta);
        }
    }
}
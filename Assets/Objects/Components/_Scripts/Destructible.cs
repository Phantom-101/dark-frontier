#nullable enable
using System;
using System.Collections.Generic;
using Framework.Persistence;
using Framework.Variables;
using UnityEngine;

namespace DarkFrontier.Objects.Components
{
    public class Destructible : ObjectComponent
    {
        public float health;
        public FloatReference maxHealth = new();
        public List<Hitbox> hitboxes = new();

        public float HealthPercent => health / maxHealth.Value;

        public event EventHandler<float>? OnHealthChanged;

        public void Add(Hitbox hitbox)
        {
            if (!hitboxes.Contains(hitbox))
            {
                hitboxes.Add(hitbox);
            }
        }

        public void Remove(Hitbox hitbox)
        {
            hitboxes.Remove(hitbox);
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
            foreach (var hitbox in hitboxes)
            {
                hitbox.SetActive(health > 0);
            }
            OnHealthChanged?.Invoke(this, delta);
        }

        public override void Save(PersistentData data)
        {
            data.Add("health", health);
        }

        public override void Load(PersistentData data)
        {
            health = data.Get<float>("health");
        }
    }
}
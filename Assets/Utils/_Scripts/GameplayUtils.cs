using System.Linq;
using DarkFrontier.Objects.Components;
using UnityEngine;

namespace DarkFrontier.Utils
{
    public static class GameplayUtils
    {
        public static bool CanDetect(Detector detector, Detectable detectable)
        {
            return detector.CanAbsolutelyDetect(detectable) || detectable.CanAbsolutelyBeDetected(detector);
        }

        public static void HitscanDamage(Vector3 origin, Vector3 direction, float range, float damage, int mask)
        {
            direction = direction.normalized;
            var hits = Physics.RaycastAll(origin, direction, range, mask);
            var sorted = hits.ToList().OrderBy(e => e.distance);
            foreach (var hit in sorted)
            {
                var hitbox = hit.collider.GetComponent<Hitbox>();
                if (hitbox != null)
                {
                    damage = hitbox.destructible.ApplyDamage(damage);
                    if (damage == 0)
                    {
                        break;
                    }
                }
            }
        }

        public static void AreaDamage(Vector3 origin, float radius, float damage, int mask)
        {
            var hits = Physics.OverlapSphere(origin, radius, mask);
            foreach (var hit in hits)
            {
                var hitbox = hit.GetComponent<Hitbox>();
                if (hitbox != null)
                {
                    hitbox.destructible.ApplyDamage(damage);
                }
            }
        }
    }
}

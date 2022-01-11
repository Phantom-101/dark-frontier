using DarkFrontier.Items.Prototypes;
using DarkFrontier.Locations;
using DarkFrontier.Positioning.Navigation;
using DarkFrontier.Structures;

namespace DarkFrontier.Controllers
{
    public class MissileController : Controller {
        public Structure Target;
        public MissileSO Missile;
        public float DamageMultiplier;

        public override void Tick(object aTicker, float aDt) {
            if (!(aTicker is Structure s)) return;
            if (Target == null || Missile == null) {
                s.uHull = 0;
                return;
            }

            MoveTo(s, s.transform, Target.transform.position);

            if (Navigation.Chebyshev(Target.transform.position, s.transform.position) <= Missile.DetonationRange * Target.uPrototype.ApparentSize) {
                Target.TakeDamage (Missile.Damage * DamageMultiplier, new Location (s.transform));
                s.uHull = 0;
            }
        }
    }
}
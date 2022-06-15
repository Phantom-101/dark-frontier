#nullable enable
using System;

namespace DarkFrontier.Items.Structures.New
{
    [Serializable]
    public class Damage
    {
        public float field;
        public float explosive;
        public float particle;
        public float kinetic;

        public float Total => field + explosive + particle + kinetic;

        public Damage(float field, float explosive, float particle, float kinetic)
        {
            this.field = field;
            this.explosive = explosive;
            this.particle = particle;
            this.kinetic = kinetic;
        }

        public static Damage operator *(Damage damage, float multiplier)
        {
            return new Damage(damage.field * multiplier, damage.explosive * multiplier, damage.particle * multiplier, damage.kinetic * multiplier);
        }
    }
}
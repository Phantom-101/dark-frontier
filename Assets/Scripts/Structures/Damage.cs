using System;

[Serializable]
public struct Damage {
    public float ShieldDamage;
    public float HullDamage;
    public float EquipmentDamage;
    public float ShieldPenetration;

    public Damage (float sh, float e) : this (sh, sh, e, 0) { }

    public Damage (float s, float h, float e) : this (s, h, e, 0) { }

    public Damage (float s, float h, float e, float p) {
        ShieldDamage = s;
        HullDamage = h;
        EquipmentDamage = e;
        ShieldPenetration = p;
    }

    public static Damage operator * (float factor, Damage damage) {
        return new Damage (damage.ShieldDamage * factor, damage.HullDamage * factor, damage.EquipmentDamage * factor, damage.ShieldPenetration);
    }

    public static Damage operator * (Damage damage, float factor) {
        return new Damage (damage.ShieldDamage * factor, damage.HullDamage * factor, damage.EquipmentDamage * factor, damage.ShieldPenetration);
    }

    public static Damage operator * (Damage damage, Damage factor) {
        return new Damage (damage.ShieldDamage * factor.ShieldDamage, damage.HullDamage * factor.HullDamage, damage.EquipmentDamage * factor.EquipmentDamage, damage.ShieldPenetration * factor.ShieldPenetration);
    }

    public static Damage operator / (Damage damage, float factor) {
        return new Damage (damage.ShieldDamage / factor, damage.HullDamage / factor, damage.EquipmentDamage / factor, damage.ShieldPenetration);
    }
}

using System;

[Serializable]
public class DamageProfile {

    public float DamageAmount;
    public float ShieldEffectiveness;
    public float ShieldPenetration;
    public float ShieldBypass;
    public float HullEffectiveness;

    public DamageProfile () {

        DamageAmount = 0;
        ShieldEffectiveness = 1;
        ShieldPenetration = 0;
        ShieldBypass = 0;
        HullEffectiveness = 1;

    }

    public DamageProfile (DamageProfile other) {

        DamageAmount = other.DamageAmount;
        ShieldEffectiveness = other.ShieldEffectiveness;
        ShieldPenetration = other.ShieldPenetration;
        ShieldBypass = other.ShieldBypass;
        HullEffectiveness = other.HullEffectiveness;

    }

    public DamageProfile (DamageProfile other, DamageProfile mult) {

        DamageAmount = other.DamageAmount * mult.DamageAmount;
        ShieldEffectiveness = other.ShieldEffectiveness * mult.ShieldEffectiveness;
        ShieldPenetration = other.ShieldPenetration * mult.ShieldPenetration;
        ShieldBypass = other.ShieldBypass * mult.ShieldBypass;
        HullEffectiveness = other.HullEffectiveness * mult.HullEffectiveness;

    }

}

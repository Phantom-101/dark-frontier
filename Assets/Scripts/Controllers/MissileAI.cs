using System;
using UnityEngine;

[Serializable]
public class MissileAI : AI {

    [SerializeField] protected Structure _target;
    [SerializeField] protected MissileSO _missile;
    [SerializeField] protected DamageProfile _damageMultiplier;
    [SerializeField] protected float _rangeMultiplier;

    protected NavigationManager _nm;

    public MissileAI (Structure structure) : base (structure) {

        _nm = NavigationManager.GetInstance ();

    }

    public void SetTarget (Structure target) { _target = target; }

    public void SetMissile (MissileSO missile) { _missile = missile; }

    public void SetLauncher (LauncherSO launcher, float lockProgress) {

        _damageMultiplier = new DamageProfile (launcher.DamageMultiplier, lockProgress / 100);
        _rangeMultiplier = launcher.RangeMultiplier;

        _structure.AddStatModifier (new StructureStatModifier ("Launcher Range Modifier", "linear_speed_multiplier", _rangeMultiplier, StructureStatModifierType.Multiplicative, 100));

    }

    public override void Tick () {

        if (_target == null || _missile == null) {

            _structure.Hull = 0;
            return;

        }

        EngineSlot engine = _structure.GetEquipment<EngineSlot> ()[0];

        engine.SetSetting (0, 2, 1);

        float angle = _structure.GetAngleTo (_target.transform.localPosition);
        if (angle > _missile.HeadingAllowance) engine.SetSetting (1, 1, 1);
        else if (angle < -_missile.HeadingAllowance) engine.SetSetting (1, 1, -1);
        else engine.SetSetting (1, 1, 0);

        float elevation = _structure.GetElevationTo (_target.transform.localPosition);
        if (elevation > _missile.HeadingAllowance) engine.SetSetting (1, 0, -1);
        else if (elevation < -_missile.HeadingAllowance) engine.SetSetting (1, 0, 1);
        else engine.SetSetting (1, 0, 0);

        if (_nm.GetLocalDistance (_target, _structure) <= _missile.DetonationRange) {

            _target.TakeDamage (new DamageProfile (_missile.Damage, _damageMultiplier), _structure.transform.position);
            _structure.Hull = 0;

        }

    }

}

using System;
using UnityEngine;

[Serializable]
public class MissileAI : AI {
    [SerializeField] protected Structure _target;
    [SerializeField] protected MissileSO _missile;
    [SerializeField] protected Damage _damageMultiplier;
    [SerializeField] protected float _rangeMultiplier;

    protected NavigationManager _nm;

    public MissileAI (Structure structure) : base (structure) {
        _nm = NavigationManager.GetInstance ();
    }

    public void SetTarget (Structure target) { _target = target; }

    public void SetMissile (MissileSO missile) { _missile = missile; }

    public void SetLauncher (LauncherSO launcher, float lockProgress) {
        _damageMultiplier = launcher.DamageMultiplier * lockProgress / 100;
        _rangeMultiplier = launcher.RangeMultiplier;

        _structure.AddStatModifier (new StructureStatModifier {
            Name = "Launcher Range Modifier",
            Id = "launcher-range-modifier",
            Target = StructureStatNames.LinearSpeedMultiplier,
            Value = _rangeMultiplier,
            Type = StructureStatModifierType.Multiplicative,
            Duration = 100,
        });
    }

    public override void Tick () {
        if (_target == null || _missile == null) {
            _structure.Hull = 0;
            return;
        }

        Vector3[] target = new Vector3[2];
        target[0].z = 1;

        float angle = _structure.GetAngleTo (_target.transform.localPosition);
        if (angle > _missile.HeadingAllowance) target[1].y = 1;
        else if (angle < -_missile.HeadingAllowance) target[1].y = -1;

        float elevation = _structure.GetElevationTo (_target.transform.localPosition);
        if (elevation > _missile.HeadingAllowance) target[1].x = -1;
        else if (elevation < -_missile.HeadingAllowance) target[1].x = 1;

        _structure.GetEquipmentData<EngineSlotData> ().ForEach (engine => {
            engine.LinearSetting = target[0];
            engine.AngularSetting = target[1];
        });

        if (_nm.GetLocalDistance (_target, _structure) <= _missile.DetonationRange) {
            _target.TakeDamage (_missile.Damage * _damageMultiplier, _structure.transform.position);
            _structure.Hull = 0;
        }
    }
}

using System;
using UnityEngine;

[Serializable]
public class MissileAI : AI {

    [SerializeField] protected Structure _target;
    [SerializeField] protected MissileSO _missile;

    public MissileAI (Structure structure) : base (structure) { }

    public void SetTarget (Structure target) { _target = target; }

    public void SetMissile (MissileSO missile) { _missile = missile; }

    public override void Tick () {

        if (_target == null || _missile == null) return;

        EngineSlot engine = _structure.GetEquipment<EngineSlot> ()[0];

        engine.SetForwardSetting (1);

        float angle = _structure.GetAngleTo (_target.transform.localPosition);
        if (angle > _missile.HeadingAllowance) engine.SetTurnSetting (1);
        else if (angle < -_missile.HeadingAllowance) engine.SetTurnSetting (-1);
        else engine.SetTurnSetting (0);

        float elevation = _structure.GetElevationTo (_target.transform.localPosition);
        if (elevation > _missile.HeadingAllowance) engine.SetPitchSetting (1);
        else if (elevation < -_missile.HeadingAllowance) engine.SetPitchSetting (-1);
        else engine.SetPitchSetting (0);

        float dis = _missile.DetonationRange + _target.GetProfile ().ApparentSize;
        float sqrDis = dis * dis;

        if ((_structure.transform.localPosition - _target.transform.localPosition).sqrMagnitude <= sqrDis) {

            _target.TakeDamage (_missile.Damage, _structure.transform.localPosition);
            _structure.SetHull (0);

        }

    }

}

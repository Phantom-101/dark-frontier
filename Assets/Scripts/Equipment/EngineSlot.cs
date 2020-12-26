﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSlot : EquipmentSlot {

    [SerializeField] protected float _forwardSetting;
    [SerializeField] protected float _turnSetting;

    public EngineSO GetEngine () { return _equipment as EngineSO; }

    public float GetForwardSetting () { return _forwardSetting; }

    public void SetForwardSetting (float target) { _forwardSetting = Mathf.Clamp (target, 0, 1); }

    public float GetTurnSetting () { return _turnSetting; }

    public void SetTurnSetting (float target) { _turnSetting = Mathf.Clamp (target, -1, 1); }

    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is EngineSO && equipment.Meta <= _equipper.GetProfile ().MaxMeta);

    }

    protected override void Reset () {

        base.Reset ();

        _forwardSetting = 0;
        _turnSetting = 0;

    }

}

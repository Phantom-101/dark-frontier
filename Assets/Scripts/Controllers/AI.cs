﻿using System;
using UnityEngine;

[Serializable]
public class AI {

    [SerializeField] protected Structure _structure;

    public AI (Structure structure) { _structure = structure; }

    public virtual void Tick () {

        EngineSlot engine = _structure.GetEquipment<EngineSlot> ()[0];
        engine.ForwardSetting = 0;
        engine.YawSetting = 0;

    }

}

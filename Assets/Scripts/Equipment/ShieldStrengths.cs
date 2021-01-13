﻿using System;
using UnityEngine;

[Serializable]
public class ShieldStrengths {

    [SerializeField] private ShieldSlot _slot;
    [SerializeField] private float[] _strengths;
    [SerializeField] private int _sectors;
    [SerializeField] private float[] _maxStrength;
    [SerializeField] private float[] _rechargeRate;

    public ShieldStrengths (ShieldSlot slot, float[] strengths, float[] maxStrength, float[] rechargeRate) {

        if (strengths.Length != maxStrength.Length || maxStrength.Length != rechargeRate.Length) Debug.LogError ("Shield input arrays must be of equal size");

        _slot = slot;
        _strengths = strengths.Clone () as float[];
        _sectors = strengths.Length;
        _maxStrength = maxStrength;
        _rechargeRate = rechargeRate;

    }

    public int GetSectorCount () { return _sectors; }

    public float GetSectorStrength (int sector) {

        if (sector < 0 && sector >= _sectors) return 0;

        return _strengths[sector];

    }

    public void SetSectorStrength (int sector, float target) {

        if (sector < 0 && sector >= _sectors) return;

        _strengths[sector] = Mathf.Clamp (target, 0, _maxStrength[sector]);

    }

    public void ChangeSectorStrength (int sector, float delta) {

        if (sector < 0 && sector >= _sectors) return;

        _strengths[sector] = Mathf.Clamp (_strengths[sector] + delta, 0, _maxStrength[sector]);

    }

    public float GetSectorMaxStrength (int sector) {

        if (sector < 0 && sector >= _sectors) return 0;

        return _maxStrength[sector];

    }

    public float GetSectorRechargeRate (int sector) {

        if (sector < 0 && sector >= _sectors) return 0;

        return _rechargeRate[sector];

    }

    public float GetAngleStrength (float angle) {

        int sector = GetAngleSector (angle);

        return GetSectorStrength (sector);

    }

    public int GetAngleSector (float angle) {

        float rotated = angle + 360f / (2 * _sectors);
        float absolute = rotated < 0 ? rotated + 360f : rotated;
        int sector = (int) (absolute / (360f / _sectors));

        return sector;

    }

    public float GetSectorAngle () { return 360f / _sectors; }

    public int GetSectorTo (GameObject to) {

        return GetSectorTo (to.transform.localPosition);

    }

    public int GetSectorTo (Vector3 to) {

        Vector3 heading = to - _slot.GetEquipper ().transform.localPosition;
        float angle = Vector3.SignedAngle (_slot.GetEquipper ().transform.forward, heading, Vector3.up);

        return GetAngleSector (angle);

    }

    public void Tick () {

        for (int i = 0; i < _sectors; i++) ChangeSectorStrength (i, GetSectorRechargeRate (i) * Time.deltaTime);

    }

}

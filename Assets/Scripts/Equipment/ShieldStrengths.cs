using System;
using UnityEngine;

[Serializable]
public class ShieldStrengths {

    [SerializeField] private ShieldSlot _slot;
    [SerializeField] private readonly float[] _strengths;
    [SerializeField] private readonly int _sectors;
    [SerializeField] private readonly float[] _maxStrength;
    [SerializeField] private readonly float[] _rechargeRate;

    public ShieldStrengths (ShieldSlot slot, float[] strengths, float[] maxStrength, float[] rechargeRate) {

        if (strengths.Length != maxStrength.Length || maxStrength.Length != rechargeRate.Length) Debug.LogError ("Shield input arrays must be of equal size");

        _slot = slot;
        _strengths = strengths;
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

        Vector3 heading = to.transform.localPosition - _slot.GetEquipper ().transform.localPosition;
        Vector3 perp = Vector3.Cross (_slot.GetEquipper ().transform.forward, heading);
        float dir = Vector3.Dot (perp, _slot.GetEquipper ().transform.up);

        float unsigned = Vector3.Angle (_slot.GetEquipper ().transform.forward, heading);
        float angle = dir > 0 ? unsigned : -unsigned;

        return GetAngleSector (angle);

    }

    public void Tick () {

        for (int i = 0; i < _sectors; i++) ChangeSectorStrength (i, GetSectorRechargeRate (i) * Time.deltaTime);

    }

}

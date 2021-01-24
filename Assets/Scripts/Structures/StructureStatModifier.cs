using System;
using UnityEngine;

[Serializable]
public class StructureStatModifier {

    [SerializeField] private string _name;
    [SerializeField] private string _target;
    [SerializeField] private float _value;
    [SerializeField] private StructureStatModifierType _type;
    [SerializeField] private float _duration;

    public StructureStatModifier (string name, string target, float value, StructureStatModifierType type, float duration) {

        _name = name;
        _target = target;
        _value = value;
        _type = type;
        _duration = duration;

    }

    public string GetName () { return _name; }

    public string GetTarget () { return _target; }

    public float GetValue () { return _value; }

    public StructureStatModifierType GetModifierType () { return _type; }

    public bool Expired () { return _duration <= 0; }

    public void TimePassed (float time) { _duration -= time; }

    public StructureStatModifier GetCopy () { return new StructureStatModifier (_name, _target, _value, _type, _duration); }

}

public enum StructureStatModifierType {

    Additive,
    Multiplicative,
    PercentAdditive,

}

using UnityEngine;

[CreateAssetMenu (menuName = "Items/Charges/Missile")]
public class MissileSO : ChargeSO {
    public StructureSO MissileStructure;
    public float HeadingAllowance;
    public float DetonationRange;
    public Damage Damage;
    public float Range;
}

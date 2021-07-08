using UnityEngine;

[CreateAssetMenu (menuName = "Items/Craft")]
public class CraftSO : StructureSO {
    public float FuelCapacity;
    public float FuelConsumption;
    public float NoFuelMaxSpeedPenalty;
    public float NoFuelAccelerationPenalty;
    public float MaxOperationalRange;
    public float SignalConnectionRange;
    public float HeadingAllowance;
}

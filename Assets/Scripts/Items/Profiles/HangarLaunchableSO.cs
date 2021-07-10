using UnityEngine;

[CreateAssetMenu (menuName = "Items/Hangar Launchable")]
public class HangarLaunchableSO : StructureSO {
    public float LoadingPreparation;
    public float LaunchingPreparation;
    public float FuelCapacity;
    public float FuelConsumption;
    public float NoFuelMaxSpeedPenalty;
    public float NoFuelAccelerationPenalty;
    public float MaxOperationalRange;
    public float SignalConnectionRange;
    public float HeadingAllowance;
}

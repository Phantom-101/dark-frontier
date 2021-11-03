using UnityEngine;

namespace DarkFrontier.Items.Prototypes {
    [CreateAssetMenu (menuName = "Items/Hangar Launchable")]
    public class HangarLaunchableSO : StructurePrototype {
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
}

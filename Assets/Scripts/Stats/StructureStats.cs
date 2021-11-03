using System;

namespace DarkFrontier.Stats {
    [Serializable]
    public struct StructureStats {
        public float MaxHull;

        public float LinearMaxSpeedMultiplier;
        public float AngularMaxSpeedMultiplier;
        public float LinearAccelerationMultiplier;
        public float AngularAccelerationMultiplier;

        public float SensorStrength;
        public float ScannerStrength;
        public float Detectability;
        public float SignatureSize;
        public int MaxTargetLocks;

        public float CargoDropPercentage;
        public float InventoryVolume;

        public static readonly StructureStats Default = new StructureStats {
            MaxHull = 1,

            LinearMaxSpeedMultiplier = 1,
            AngularMaxSpeedMultiplier = 1,
            LinearAccelerationMultiplier = 1,
            AngularAccelerationMultiplier = 1,

            SensorStrength = 0,
            ScannerStrength = 0,
            Detectability = 0,
            SignatureSize = 0,
            MaxTargetLocks = 0,

            CargoDropPercentage = 1,
            InventoryVolume = 0,
        };
    }
}

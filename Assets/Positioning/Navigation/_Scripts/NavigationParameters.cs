using DarkFrontier.Attributes;
using UnityEngine;

namespace DarkFrontier.Positioning.Navigation
{
    public class NavigationParameters : MonoBehaviour
    {
        [field: SerializeField]
        public Transform Target { get; private set; }

        [field: SerializeField, ReadOnly]
        public float Azimuth { get; private set; }

        [field: SerializeField, ReadOnly]
        public float Altitude { get; private set; }

        [field: SerializeField, ReadOnly]
        public float AzimuthDir { get; private set; }

        [field: SerializeField, ReadOnly]
        public float AltitudeDir { get; private set; }

        [field: SerializeField, ReadOnly]
        public float AzimuthNorm { get; private set; }

        [field: SerializeField, ReadOnly]
        public float AltitudeNorm { get; private set; }

        private void Update()
        {
            Azimuth = Navigation.Azimuth(transform, Target.position);
            Altitude = Navigation.Altitude(transform, Target.position);

            AzimuthDir = Navigation.AzimuthDir(transform, Target.position);
            AltitudeDir = Navigation.AltitudeDir(transform, Target.position);

            AzimuthNorm = Navigation.AzimuthSign(transform, Target.position);
            AltitudeNorm = Navigation.AltitudeSign(transform, Target.position);
        }
    }
}

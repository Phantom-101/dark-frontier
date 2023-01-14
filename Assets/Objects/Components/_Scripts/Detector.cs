namespace DarkFrontier.Objects.Components
{
    public abstract class Detector : ObjectComponent
    {
        public abstract bool CanAbsolutelyDetect(Detectable detectable);
    }
}
namespace DarkFrontier.Objects.Components
{
    public abstract class Detectable : ObjectComponent
    {
        public abstract bool CanAbsolutelyBeDetected(Detector detector);
    }
}
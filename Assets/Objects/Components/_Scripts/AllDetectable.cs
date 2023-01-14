namespace DarkFrontier.Objects.Components
{
    public class AllDetectable : Detectable
    {
        public override bool CanAbsolutelyBeDetected(Detector detector)
        {
            return true;
        }
    }
}
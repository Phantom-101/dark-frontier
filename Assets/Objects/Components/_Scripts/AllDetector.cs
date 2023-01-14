namespace DarkFrontier.Objects.Components
{
    public class AllDetector : Detector
    {
        public override bool CanAbsolutelyDetect(Detectable detectable)
        {
            return true;
        }
    }
}
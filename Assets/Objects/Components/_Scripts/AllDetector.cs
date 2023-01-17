using Framework.Variables;

namespace DarkFrontier.Objects.Components
{
    public class AllDetector : Detector
    {
        public BoolReference ignoreSector = new();
        
        public override bool CanAbsolutelyDetect(Detectable detectable)
        {
            return ignoreSector.Value || detectable.obj.Sector == obj.Sector;
        }
    }
}
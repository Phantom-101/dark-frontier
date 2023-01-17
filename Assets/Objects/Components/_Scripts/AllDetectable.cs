using Framework.Variables;

namespace DarkFrontier.Objects.Components
{
    public class AllDetectable : Detectable
    {
        public BoolReference ignoreSector = new();
        
        public override bool CanAbsolutelyBeDetected(Detector detector)
        {
            return ignoreSector.Value || detector.obj.Sector == obj.Sector;
        }
    }
}
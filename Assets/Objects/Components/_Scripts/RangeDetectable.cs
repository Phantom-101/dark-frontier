using Framework.Variables;

namespace DarkFrontier.Objects.Components
{
    public class RangeDetectable : Detectable
    {
        public FloatReference range = new();
        public BoolReference multiply = new();

        public override bool CanAbsolutelyBeDetected(Detector detector)
        {
            return detector.obj.Sector == obj.Sector && (detector.transform.position - transform.position).sqrMagnitude <= range.Value * (multiply.Value && detector is RangeDetector casted ? casted.range.Value : range.Value);
        }
    }
}
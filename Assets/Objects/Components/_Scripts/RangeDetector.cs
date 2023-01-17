using Framework.Variables;

namespace DarkFrontier.Objects.Components
{
    public class RangeDetector : Detector
    {
        public FloatReference range = new();
        public BoolReference multiply = new();

        public override bool CanAbsolutelyDetect(Detectable detectable)
        {
            return detectable.obj.Sector == obj.Sector && (detectable.transform.position - transform.position).sqrMagnitude <= range.Value * (multiply.Value && detectable is RangeDetectable casted ? casted.range.Value : range.Value);
        }
    }
}
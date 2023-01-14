namespace DarkFrontier.Items.Equipment.Weapons.Lasers
{
    public class BeamLaserAlphaProvider : ILaserAlphaProvider
    {
        public float Alpha
        {
            get
            {
                var prototype = _instance.Prototype;
                var timeNorm = 1 - _instance.Delay / prototype.interval;
                var alpha = prototype.alphaBase.Evaluate(timeNorm);
                var rampNorm = _instance.Multiplier / prototype.multiplier;
                return alpha * prototype.alphaMult.Evaluate(rampNorm);
            }
        }
        
        private readonly BeamLaserInstance _instance;
        
        public BeamLaserAlphaProvider(BeamLaserInstance instance)
        {
            _instance = instance;
        }
    }
}
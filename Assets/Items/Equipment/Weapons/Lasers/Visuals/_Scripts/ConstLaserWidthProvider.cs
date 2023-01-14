namespace DarkFrontier.Items.Equipment.Weapons.Lasers
{
    public class ConstLaserWidthProvider : ILaserWidthProvider
    {
        public float Width => _instance.Prototype.width;

        private readonly BeamLaserInstance _instance;

        public ConstLaserWidthProvider(BeamLaserInstance instance)
        {
            _instance = instance;
        }
    }
}
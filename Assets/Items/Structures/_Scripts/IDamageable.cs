using DarkFrontier.Structures;
using DarkFrontier.UI.Indicators.Selectors;

namespace DarkFrontier.Items.Structures
{
    public interface IDamageable : ISelectable
    {
        public float MaxHp { get; }
        
        public float CurrentHp { get; }
        
        void Inflict(Damage damage);
    }
}
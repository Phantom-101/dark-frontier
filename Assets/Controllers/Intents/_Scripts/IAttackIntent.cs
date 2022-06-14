using DarkFrontier.Items.Structures;

namespace DarkFrontier.Controllers.Intents
{
    public interface IAttackIntent
    {
        void TryAttack(ISelectable target);
    }
}
using DarkFrontier.Items.Structures;

namespace DarkFrontier.Controllers.Intents
{
    public interface IAttackIntent
    {
        void Attack(ISelectable target);
    }
}
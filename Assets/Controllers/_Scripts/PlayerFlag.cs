using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers
{
    public class PlayerFlag : MonoBehaviour
    {
        public void Execute(IdRegistry registry, New.PlayerController controller)
        {
            var component = GetComponent<StructureComponent>();
            if (component != null)
            {
                controller.Player = registry.Get<StructureComponent>(component.Id);
            }
            Destroy(this);
        }
    }
}
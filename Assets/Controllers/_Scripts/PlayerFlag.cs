using DarkFrontier.Game.Essentials;
using DarkFrontier.Items.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers
{
    public class PlayerFlag : MonoBehaviour
    {
        public string id;

        public void Execute(IdRegistry registry, New.PlayerController controller)
        {
            controller.Player = registry.Get<StructureComponent>(id);
            Destroy(this);
        }
    }
}
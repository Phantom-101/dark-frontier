#nullable enable
using DarkFrontier.Input;
using DarkFrontier.Items.Structures;
using UnityEngine;

namespace DarkFrontier.Controllers.New
{
    public class PlayerController : MonoBehaviour
    {
        public StructureComponent? Player { get; set; }

        private InputActions _inputActions = null!;

        private void Awake()
        {
            _inputActions = new InputActions();
        }
        
        private void OnEnable()
        {
            _inputActions.Gameplay.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Gameplay.Disable();
        }

        public void Tick()
        {
            if(Player == null || Player.Instance == null) return;
            
            var strafe = _inputActions.Gameplay.Strafe.ReadValue<Vector2>();
            Player.Adaptor!.linearTarget = new Vector3(strafe.x, strafe.y, _inputActions.Gameplay.Accelerate.ReadValue<float>());
            var turn = _inputActions.Gameplay.Turn.ReadValue<Vector2>();
            Player.Adaptor.angularTarget = new Vector3(-turn.y, turn.x, _inputActions.Gameplay.Roll.ReadValue<float>());
        }
    }
}
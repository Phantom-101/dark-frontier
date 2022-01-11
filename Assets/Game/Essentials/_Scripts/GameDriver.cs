using DarkFrontier.Attributes;
using DarkFrontier.Game.Player.Camera;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Game.Essentials
{
    public class GameDriver : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private GameSettings _gameSettings = null!;

        [SerializeField, ReadOnly]
        private CameraSpring? _cameraSpring;

        private void Start()
        {
            InitializeSelf();
            InitializeOthers();
        }

        private void InitializeSelf()
        {
            _gameSettings = ComponentUtils.AddOrGet<GameSettings>(gameObject);
            _cameraSpring = FindObjectOfType<CameraSpring>();
        }

        private void InitializeOthers()
        {
            _gameSettings.Initialize();
            if(_cameraSpring != null)
            {
                _cameraSpring.Initialize();
            }
        }

        private void Update()
        {
            if(_cameraSpring != null)
            {
                _cameraSpring.Tick();
            }
        }
    }
}
#nullable restore
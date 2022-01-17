using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Player.Camera;
using DarkFrontier.Items.Structures;
using DarkFrontier.Positioning.Navigation;
using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable enable
namespace DarkFrontier.Game.Essentials
{
    public class GameDriver : MonoBehaviour
    {
        [SerializeReference, ReadOnly]
        private GameSettings _gameSettings = null!;

        [SerializeReference, ReadOnly]
        private NavigationPathfinder _pathfinder = null!;
        
        [SerializeReference, ReadOnly]
        private CameraSpring? _cameraSpring;

        private void Start()
        {
            InitializeSelf();
            InitializeOthers();
        }

        private void InitializeSelf()
        {
            Singletons.Bind(_gameSettings = ComponentUtils.AddOrGet<GameSettings>(gameObject));
            _pathfinder = ComponentUtils.AddOrGet<NavigationPathfinder>(gameObject);
            _cameraSpring = FindObjectOfType<CameraSpring>();
        }

        private void InitializeOthers()
        {
            _gameSettings.Initialize();
            _pathfinder.Initialize(SceneManager.GetActiveScene());
            var structures = FindObjectsOfType<StructureComponent>();
            for(int i = 0, l = structures.Length; i < l; i++)
            {
                if(!structures[i].Initialize())
                {
                    Destroy(structures[i].gameObject);
                }
            }
            if(_cameraSpring != null)
            {
                _cameraSpring.Initialize();
            }
        }

        private void Update()
        {
            _pathfinder.Tick();
            var finders = FindObjectsOfType<NavigationRouteFinder>();
            for(int i = 0, l = finders.Length; i < l; i++)
            {
                finders[i].Tick();
            }
            if(_cameraSpring != null)
            {
                _cameraSpring.Tick();
            }
        }
    }
}
#nullable restore
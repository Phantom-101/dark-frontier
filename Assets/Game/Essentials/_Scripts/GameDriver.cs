#nullable enable
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Player.Camera;
using DarkFrontier.Items.Structures;
using DarkFrontier.Positioning.Navigation;
using DarkFrontier.Positioning.Sectors;
using DarkFrontier.UI.Indicators.Selectors;
using DarkFrontier.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;


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
            Singletons.Bind(new DetectableRegistry());
            _cameraSpring = FindObjectOfType<CameraSpring>();
        }

        private void InitializeOthers()
        {
            _gameSettings.Initialize();

            _pathfinder.Initialize(SceneManager.GetActiveScene());

            var sectors = FindObjectsOfType<SectorComponent>();
            for(int i = 0, l = sectors.Length; i < l; i++)
            {
                if(!sectors[i].Initialize())
                {
                    Destroy(sectors[i].gameObject);
                }
            }
            
            var structures = FindObjectsOfType<StructureComponent>();
            for(int i = 0, l = structures.Length; i < l; i++)
            {
                if(!structures[i].Initialize())
                {
                    Destroy(structures[i].gameObject);
                }
            }

            if(_cameraSpring != null) _cameraSpring.Initialize();
        }

        private void Update()
        {
            _pathfinder.Tick();
            var finders = FindObjectsOfType<NavigationRouteFinder>();
            for(int i = 0, l = finders.Length; i < l; i++)
            {
                finders[i].Tick();
            }

            if(_cameraSpring != null) _cameraSpring.Tick();
        }
    }
}
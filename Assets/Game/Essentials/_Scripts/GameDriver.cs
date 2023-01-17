#nullable enable
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Player.Camera;
using DarkFrontier.Items._Scripts;
using DarkFrontier.Items.Structures;
using DarkFrontier.Positioning.Sectors;
using DarkFrontier.UI.Indicators.Selectors;
using DarkFrontier.UI.States;
using DarkFrontier.Utils;
using UnityEngine;
using PlayerController = DarkFrontier.Controllers.New.PlayerController;

namespace DarkFrontier.Game.Essentials
{
    public class GameDriver : MonoBehaviour
    {
        [SerializeField]
        private string _universeName = "";
        
        private GameSettings _gameSettings = null!;
        private SerializationDriver _serializationDriver = null!;

        private UIStack _uiStack = null!;

        private IdRegistry _idRegistry = null!;
        private DetectableRegistry _detectableRegistry = null!;
        private StructureRegistry _structureRegistry = null!;
        private SectorRegistry _sectorRegistry = null!;

        private PlayerController _playerController = null!;
        private CameraSpring _cameraSpring = null!;
        private UnityEngine.Camera _camera = null!;

        private void Start()
        {
            Singletons.Bind(_gameSettings = gameObject.AddOrGet<GameSettings>());
            (_serializationDriver = Singletons.Exists<SerializationDriver>() ? Singletons.Get<SerializationDriver>() : new SerializationDriver()).Deserialize();
            Singletons.Bind(_uiStack = ComponentUtils.AddOrGet<UIStack>());
            Singletons.Bind(_idRegistry = new IdRegistry());
            Singletons.Bind(_detectableRegistry = new DetectableRegistry());
            Singletons.Bind(_sectorRegistry = new SectorRegistry());
            Singletons.Bind(_structureRegistry = new StructureRegistry());
            Singletons.Bind(_playerController = ComponentUtils.AddOrGet<PlayerController>());
            if ((_cameraSpring = FindObjectOfType<CameraSpring>()) != null)
            {
                Singletons.Bind(_cameraSpring);
                if (_camera = FindObjectOfType<UnityEngine.Camera>()) Singletons.Bind(_cameraSpring.camera = _camera);
            }

            _gameSettings.Initialize();
            InitializeIdRegistry();
            InitializeSectors();
            InitializeStructures();
            InitializePlayer();
        }

        private void InitializeIdRegistry()
        {
            var itemRegistry = GetComponent<ItemRegistry>();
            if(itemRegistry != null)
            {
                itemRegistry.Register(_idRegistry);
            }
        }

        private static void InitializeSectors()
        {
            var authorings = FindObjectsOfType<SectorAuthoring>();
            for(int i = 0, l = authorings.Length; i < l; i++)
            {
                authorings[i].Author();
            }
            var sectors = FindObjectsOfType<SectorComponent>();
            for(int i = 0, l = sectors.Length; i < l; i++)
            {
                sectors[i].Enable();
            }
        }

        private static void InitializeStructures()
        {
            //var authorings = FindObjectsOfType<StructureAuthoring>();
            //for(int i = 0, l = authorings.Length; i < l; i++)
            //{
            //    authorings[i].Author();
            //}
        }

        private void InitializePlayer()
        {
            var playerFlag = FindObjectOfType<PlayerFlag>();
            if(playerFlag != null)
            {
                playerFlag.Execute(_idRegistry, _playerController);
            }
            if(_cameraSpring != null) _cameraSpring.Initialize();
        }

        private void Update()
        {
            _uiStack.Tick();
            TickSectors();
            _structureRegistry.Tick(UnityEngine.Time.deltaTime);
            TickPlayer();
        }

        private void TickSectors()
        {
            for(int i = 0, l = _sectorRegistry.Registry.Count; i < l; i++)
            {
                _sectorRegistry.Registry[i].Tick(UnityEngine.Time.deltaTime);
            }
        }

        private void TickPlayer()
        {
            _playerController.Tick();
            if(_cameraSpring != null)
            {
                _cameraSpring.target = _playerController.Player == null ? null : _playerController.Player.transform;
                _cameraSpring.Tick();
            }
        }

        private void FixedUpdate()
        {
            _structureRegistry.FixedTick(UnityEngine.Time.fixedDeltaTime);
        }
    }
}
#nullable enable
using DarkFrontier.Attributes;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Game.Player.Camera;
using DarkFrontier.Items.Structures;
using DarkFrontier.Positioning.Sectors;
using DarkFrontier.UI.Indicators.Selectors;
using DarkFrontier.Utils;
using UnityEngine;

namespace DarkFrontier.Game.Essentials
{
    public class GameDriver : MonoBehaviour
    {
        [SerializeField]
        private string _universeName = "";
        
        [SerializeReference, ReadOnly]
        private GameSettings _gameSettings = null!;

        [SerializeReference, ReadOnly]
        private SerializationDriver _serializationDriver = null!;

        [SerializeReference, ReadOnly]
        private DetectableRegistry _detectableRegistry = null!;

        [SerializeReference, ReadOnly]
        private StructureRegistry _structureRegistry = null!;
        
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
            if(Singletons.Exists<SerializationDriver>())
            {
                (_serializationDriver = Singletons.Get<SerializationDriver>()).Deserialize();
            }
            else
            {
                Singletons.Bind(_serializationDriver = new SerializationDriver());
            }
            Singletons.Bind(_structureRegistry = new StructureRegistry());
            Singletons.Bind(_detectableRegistry = new DetectableRegistry());
            _cameraSpring = FindObjectOfType<CameraSpring>();
        }

        private void InitializeOthers()
        {
            _gameSettings.Initialize();

            InitializeSectors();
            
            InitializeStructures();

            if(_cameraSpring != null) _cameraSpring.Initialize();
        }

        private static void InitializeSectors()
        {
            var authorings = FindObjectsOfType<SectorAuthoring>();
            for(int i = 0, l = authorings.Length; i < l; i++)
            {
                authorings[i].Generate();
            }
            
            var sectors = FindObjectsOfType<SectorComponent>();
            for(int i = 0, l = sectors.Length; i < l; i++)
            {
                sectors[i].Enable();
            }
        }

        private static void InitializeStructures()
        {
            var authorings = FindObjectsOfType<StructureAuthoring>();
            for(int i = 0, l = authorings.Length; i < l; i++)
            {
                authorings[i].Generate();
            }
            
            var structures = FindObjectsOfType<StructureComponent>();
            for(int i = 0, l = structures.Length; i < l; i++)
            {
                structures[i].Enable();
            }
        }

        private void Update()
        {
            // TODO Tick sector and structure registries
            if(_cameraSpring != null) _cameraSpring.Tick();
        }
    }
}
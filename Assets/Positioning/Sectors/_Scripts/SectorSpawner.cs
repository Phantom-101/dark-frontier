using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkFrontier.Positioning.Sectors
{
    public static class SectorSpawner
    {
        public static SectorComponent Create(SectorInstance instance)
        {
            var scene = SceneManager.CreateScene(instance.Name, new CreateSceneParameters(LocalPhysicsMode.Physics3D));
            
            var instantiated = new GameObject($"{instance.Name} (SectorComponent)");
            SceneManager.MoveGameObjectToScene(instantiated, scene);
            
            var sector = instantiated.AddComponent<SectorComponent>();
            sector.SetInstance(instance);
            
            return sector;
        }

        public static Scene Create(SectorComponent component)
        {
            var scene = SceneManager.CreateScene(component.Instance?.Name ?? "Unknown Sector", new CreateSceneParameters(LocalPhysicsMode.Physics3D));

            SceneManager.MoveGameObjectToScene(component.gameObject, scene);

            return scene;
        }
    }
}

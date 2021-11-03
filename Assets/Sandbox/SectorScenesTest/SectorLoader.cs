using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkFrontier.Sandbox.SectorScenesTest {
    public class SectorLoader : ComponentBehavior {
        public string[] uSceneNames;

        private Scene[] iScenes;

        public override void Enable () {
            iScenes = new Scene[uSceneNames.Length];
            for (var i = 0; i < uSceneNames.Length; i++) {
                iScenes[i] = SceneManager.LoadScene (uSceneNames[i], new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });
            }
            Singletons.Get<BehaviorTimekeeper> ().Subscribe (this);
        }

        public override void FixedTick (object aTicker, float aDt) {
            foreach (var lScene in iScenes) {
                var lPhysicsScene = lScene.GetPhysicsScene ();
                lPhysicsScene.Simulate (aDt);
            }
        }
    }
}
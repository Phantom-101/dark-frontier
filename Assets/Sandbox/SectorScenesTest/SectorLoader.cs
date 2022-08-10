using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkFrontier.Sandbox.SectorScenesTest {
    public class SectorLoader : MonoBehaviour {
        public string[] uSceneNames;

        private Scene[] iScenes;

        private void Start () {
            iScenes = new Scene[uSceneNames.Length];
            for (var i = 0; i < uSceneNames.Length; i++) {
                iScenes[i] = SceneManager.LoadScene (uSceneNames[i], new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });
            }
        }

        private void Update() {
            foreach (var lScene in iScenes) {
                var lPhysicsScene = lScene.GetPhysicsScene ();
                lPhysicsScene.Simulate (UnityEngine.Time.deltaTime);
            }
        }
    }
}
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using UnityEngine;

namespace DarkFrontier.Installers {
    public class BaseInstaller : Installer {
        public override void InstallBindings () {
            if (!Singletons.Exists<BehaviorTimekeeper> ()) {
                var lGameObject = new GameObject ("[Service] Behavior Timekeeper");
                Singletons.Bind (lGameObject.AddComponent<BehaviorTimekeeper> ());
                DontDestroyOnLoad (lGameObject);
            }

            if (!Singletons.Exists<BehaviorManager> ()) {
                Singletons.Bind (new BehaviorManager ());
            }

            if (!Singletons.Exists<BehaviorPooler>()) {
                var lGameObject = new GameObject ("[Service] Behavior Pooler");
                Singletons.Bind (lGameObject.AddComponent<BehaviorPooler> ());
                DontDestroyOnLoad (lGameObject);
            }
        }
    }
}
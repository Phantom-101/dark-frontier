using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Services;
using UnityEngine;

namespace DarkFrontier.Installers {
    public class GameInstaller : Installer {
        public override void InstallBindings () {
            if (!Singletons.Exists<PlayerController>()) {
                var lGameObject = new GameObject("[Service] Player Controller");
                Singletons.Bind (lGameObject.AddComponent<PlayerController>());
                DontDestroyOnLoad (lGameObject);
            }
        }
    }
}

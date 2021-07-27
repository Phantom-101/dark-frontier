using UnityEngine;
using Zenject;

namespace DarkFrontier.Installers {
    [CreateAssetMenu (menuName = "Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {
        public override void InstallBindings () {
        }
    }
}

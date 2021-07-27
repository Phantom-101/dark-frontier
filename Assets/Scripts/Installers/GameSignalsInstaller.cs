using Zenject;

namespace DarkFrontier.Installers {
    public class GameSignalsInstaller : Installer<GameSignalsInstaller> {
        public override void InstallBindings () {
            SignalBusInstaller.Install (Container);
        }
    }
}

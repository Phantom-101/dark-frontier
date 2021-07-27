using DarkFrontier.Structures;
using Zenject;

namespace DarkFrontier.Installers {
    public class GameInstaller : MonoInstaller {
        public override void InstallBindings () {
            Container.Bind<StructureRegistry> ().AsSingle ();
            Container.Bind<StructureManager> ().FromNewComponentOnNewGameObject ().AsSingle ();
        }
    }
}

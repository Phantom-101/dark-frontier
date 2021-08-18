using DarkFrontier.Factions;
using DarkFrontier.Structures;
using Zenject;

namespace DarkFrontier.Installers {
    public class GameInstaller : MonoInstaller {
        public override void InstallBindings () {
            Container.Bind<FactionRegistry> ().AsSingle ();
            Container.Bind<FactionManager> ().FromNewComponentOnNewGameObject ().AsSingle ();

            Container.Bind<StructureRegistry> ().AsSingle ();
            Container.Bind<StructureManager> ().FromNewComponentOnNewGameObject ().AsSingle ();
        }
    }
}

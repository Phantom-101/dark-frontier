using UnityEngine;

namespace DarkFrontier.Foundation.Services {
    public class Installer : MonoBehaviour {
        private void Awake () => InstallBindings ();
        public virtual void InstallBindings () { }
    }
}
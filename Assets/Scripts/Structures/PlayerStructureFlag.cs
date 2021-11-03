using System;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Services;
using UnityEngine;

namespace DarkFrontier.Structures {
    public class PlayerStructureFlag : MonoBehaviour {
        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
    
        private void Awake () {
            Structure structure = GetComponent<Structure> ();
            if (structure != null) iPlayerController.Value.UPlayer = structure;
            Destroy (this);
        }
    }
}

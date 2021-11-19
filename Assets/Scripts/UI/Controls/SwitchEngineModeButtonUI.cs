using System;
using System.Linq;
using DarkFrontier.Controllers;
using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Services;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Controls {
    public class SwitchEngineModeButtonUI : MonoBehaviour {
        [SerializeField]
        private Button _switchEngineModeButton;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
    
        private void Awake () {
            _switchEngineModeButton.onClick.AddListener (Toggle);
        }

        private void Toggle () {
            // Get player
            var lPlayer = iPlayerController.Value.UPlayer;
            // Get engines
            var lEngines = lPlayer.UEquipment.States<EnginePrototype.State> ();
            var lCount = lEngines.Count;
            // Calculate target state
            var lTargetState = false;
            if (lCount > 0) {
                lTargetState = !lEngines[0].ManagedPropulsion;
            }
            // Apply target states
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                lEngines[lIndex].ManagedPropulsion = lTargetState;
            }
        }
    }
}

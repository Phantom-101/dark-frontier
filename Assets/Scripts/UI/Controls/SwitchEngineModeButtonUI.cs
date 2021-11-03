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
            var lEngines = lPlayer.GetEquipmentStates<EnginePrototype.State> ().ToList();
            // Calculate target state
            var lCurState = lEngines.Any() && lEngines[0].ManagedPropulsion;
            var lTargetState = !lCurState;
            // Apply target states
            foreach (var lEngine in lEngines) {
                lEngine.ManagedPropulsion = lTargetState;
            }
        }
    }
}

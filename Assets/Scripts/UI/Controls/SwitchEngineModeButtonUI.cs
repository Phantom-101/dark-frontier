using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchEngineModeButtonUI : MonoBehaviour {
    [SerializeField]
    private Button _switchEngineModeButton;

    private void Awake () {
        _switchEngineModeButton.onClick.AddListener (Toggle);
    }

    private void Toggle () {
        // Get player
        Structure player = PlayerController.Instance.Player;
        // Get engines
        List<EngineSlotData> engines = player.GetEquipmentData<EngineSlotData> ();
        // Calculate target state
        bool curState = engines.Count > 0 && engines[0].ManagedPropulsion;
        bool targetState = !curState;
        // Apply target states
        engines.ForEach (engine => engine.ManagedPropulsion = targetState);
    }
}

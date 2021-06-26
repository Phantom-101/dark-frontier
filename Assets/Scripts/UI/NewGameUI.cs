using System.Collections.Generic;
using UnityEngine;

public class NewGameUI : UIStateView {
    [SerializeField] private List<GamePresetSO> _presets;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _preset;
    [SerializeField] private List<GamePresetUI> _instPresets;

    public void PresetSelected (GamePresetSO selected) {
        SceneUtils.Instance.LoadScene (selected.SceneName);
    }

    protected override void StateChanged () {
        // Destroy instantiated prefabs
        _instPresets.ForEach (e => Destroy (e.gameObject));
        _instPresets = new List<GamePresetUI> ();

        if (IsShown) {
            // Instantiate preset prefabs
            foreach (GamePresetSO preset in _presets) {
                GameObject pgo = Instantiate (_preset, _content);
                GamePresetUI pinfo = pgo.GetComponent<GamePresetUI> ();
                pinfo.Button.onClick.AddListener (() => PresetSelected (preset));
                pinfo.Preset = preset;
                _instPresets.Add (pinfo);
            }
        }
    }
}

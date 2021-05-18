using System.Collections.Generic;
using UnityEngine;

public class NewGameUI : MonoBehaviour {

    [SerializeField] private List<GamePresetSO> _presets;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _preset;
    [SerializeField] private List<GamePresetUI> _instPresets;
    [SerializeField] private VoidEventChannelSO _changed;

    private UIStateManager _uiStateManager;

    private void Start () {

        _uiStateManager = UIStateManager.GetInstance ();

        _changed.OnEventRaised += OnUIStateChanged;

    }

    public void PresetSelected (GamePresetSO selected) {

        SceneUtils.Instance.LoadScene (selected.SceneName);

    }

    private void OnUIStateChanged () {

        if (_uiStateManager == null) {
            Debug.Log ("UI state manager not found");
            return;
        }

        if (_uiStateManager.GetState () == UIState.NewGame) Build ();

    }

    private void Build () {

        SaveManager sm = SaveManager.GetInstance ();

        while (_instPresets.Count > 0) {

            Destroy (_instPresets[0].gameObject);
            _instPresets.RemoveAt (0);

        }

        int offset = 0;
        foreach (GamePresetSO preset in _presets) {

            GameObject pgo = Instantiate (_preset, _content);
            pgo.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -300 * offset);
            GamePresetUI pinfo = pgo.GetComponent<GamePresetUI> ();
            pinfo.Button.onClick.AddListener (() => {
                PresetSelected (preset);
            });
            pinfo.Preset = preset;
            _instPresets.Add (pinfo);
            offset++;

        }
        _content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 300 * offset);

    }

}

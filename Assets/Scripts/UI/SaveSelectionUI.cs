using System.Collections.Generic;
using UnityEngine;

public class SaveSelectionUI : MonoBehaviour {

    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _universe;
    [SerializeField] private List<UniverseInfoUI> _instUniverses;
    [SerializeField] private GameObject _save;
    [SerializeField] private List<SaveInfoUI> _instSaves;
    [SerializeField] private List<string> _expanded = new List<string> ();
    [SerializeField] private VoidEventChannelSO _changed;

    private UIStateManager _uiStateManager;

    private void Start () {

        _uiStateManager = UIStateManager.GetInstance ();

        _changed.OnEventRaised += OnUIStateChanged;

    }

    public void SaveSelected (string universe, string name) {

        SceneUtils.Instance.LoadScene ("Game");

        SaveManager.GetInstance ().Load (universe, name);

    }

    private void OnUIStateChanged () {

        if (_uiStateManager == null) {
            Debug.Log ("UI state manager not found");
            return;
        }

        if (_uiStateManager.GetState () == UIState.SaveSelection) Build ();

    }

    private void Build () {

        SaveManager sm = SaveManager.GetInstance ();

        while (_instUniverses.Count > 0) {

            Destroy (_instUniverses[0].gameObject);
            _instUniverses.RemoveAt (0);

        }
        while (_instSaves.Count > 0) {

            Destroy (_instSaves[0].gameObject);
            _instSaves.RemoveAt (0);

        }

        string[] universes = sm.GetAllUniverses ();
        int offset = 0;
        foreach (string universe in universes) {

            GameObject ugo = Instantiate (_universe, _content);
            ugo.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -150 * offset);
            UniverseInfoUI uinfo = ugo.GetComponent<UniverseInfoUI> ();
            uinfo.GetButton ().onClick.AddListener (() => {
                if (_expanded.Contains (universe)) _expanded.Remove (universe);
                else _expanded.Add (universe);
                Build ();
            });
            uinfo.SetName (universe);
            _instUniverses.Add (uinfo);
            offset++;

            if (_expanded.Contains (universe)) {

                string[] saves = sm.GetAllSaves (universe);
                foreach (string save in saves) {

                    GameObject sgo = Instantiate (_save, _content);
                    sgo.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -150 * offset);
                    SaveInfoUI sinfo = sgo.GetComponent<SaveInfoUI> ();
                    sinfo.GetButton ().onClick.AddListener (() => {

                        UIStateManager.GetInstance ().RemoveState ();
                        SaveSelected (universe, save);

                    });
                    sinfo.SetName (universe);
                    sinfo.SetTime (save);
                    _instSaves.Add (sinfo);
                    offset++;

                }

            }

        }
        _content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 150 * offset);

    }

}

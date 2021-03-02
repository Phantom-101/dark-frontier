using System.Collections.Generic;
using UnityEngine;

public class SaveSelectionUI : MonoBehaviour {

    [SerializeField] private CanvasGroup _group;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _universe;
    [SerializeField] private List<UniverseInfoUI> _instUniverses;
    [SerializeField] private GameObject _save;
    [SerializeField] private List<SaveInfoUI> _instSaves;
    [SerializeField] private List<string> _expanded = new List<string> ();
    [SerializeField] private float _curAlpha = -1;

    private void Start () {

        UIState current = UIStateManager.GetInstance ().GetState ();
        bool shouldShow = current == UIState.SaveSelection;

        if (!shouldShow) {

            if (_curAlpha != 0) {

                _curAlpha = 0;
                SwitchOutImmediately ();

            }

        }

    }

    private void Update () {

        UIState current = UIStateManager.GetInstance ().GetState ();
        bool shouldShow = current == UIState.SaveSelection;

        if (!shouldShow) {

            if (_curAlpha != 0) {
                _curAlpha = 0;
                SwitchOut ();

            }
            return;

        }

        if (_curAlpha == 0) {

            _curAlpha = 1;
            SwitchIn ();

        }

    }

    public void SaveSelected (string universe, string name) {

        SceneUtils.Instance.LoadScene ("Game");

        SaveManager.GetInstance ().Load (universe, name);

    }

    private void SwitchIn () {

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
                SwitchIn ();
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

        LeanTween.alphaCanvas (_group, 1, 0.2f).setIgnoreTimeScale (true);
        _group.interactable = true;
        _group.blocksRaycasts = true;

    }

    private void SwitchOut () {

        LeanTween.alphaCanvas (_group, 0, 0.2f).setIgnoreTimeScale (true);
        _group.interactable = false;
        _group.blocksRaycasts = false;

    }

    private void SwitchOutImmediately () {

        LeanTween.alphaCanvas (_group, 0, 0).setIgnoreTimeScale (true);
        _group.interactable = false;
        _group.blocksRaycasts = false;

    }

}

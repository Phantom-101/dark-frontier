using UnityEngine;

public class LogUI : MonoBehaviour {

    [SerializeField] private CanvasGroup _group;
    [SerializeField] private int _tab = 0;
    [SerializeField] private LogTabUI[] _tabs;
    [SerializeField] private float _curAlpha = -1;

    private void Start () {

        UIState current = UIStateManager.GetInstance ().GetState ();
        bool shouldShow = current == UIState.Log;

        if (!shouldShow) {

            if (_curAlpha != 0) {
                _curAlpha = 0;
                DisableAll ();
                TweenToCurAlphaImmediately ();

            }

        }

        foreach (LogTabUI tab in _tabs) tab.SwitchOutImmediately ();

    }

    private void Update () {

        UIState current = UIStateManager.GetInstance ().GetState ();
        bool shouldShow = current == UIState.Log;

        if (!shouldShow) {

            if (_curAlpha != 0) {
                _curAlpha = 0;
                DisableAll ();
                TweenToCurAlpha ();

            }
            return;

        }

        if (_curAlpha == 0) {

            EnableAll ();
            _curAlpha = 1;
            TweenToCurAlpha ();

        }

    }

    public int GetTab () { return _tab; }

    public void SetTab (int tab) {

        _tabs[_tab].SwitchOut ();
        _tab = tab;
        _tabs[_tab].SwitchIn ();

    }

    void TweenToCurAlpha () {

        LeanTween.alphaCanvas (_group, _curAlpha, 0.2f).setIgnoreTimeScale (true);

    }

    void TweenToCurAlphaImmediately () {

        LeanTween.alphaCanvas (_group, _curAlpha, 0).setIgnoreTimeScale (true);

    }

    void DisableAll () {

        _group.interactable = false;
        _group.blocksRaycasts = false;

    }

    void EnableAll () {

        _group.interactable = true;
        _group.blocksRaycasts = true;

    }

}

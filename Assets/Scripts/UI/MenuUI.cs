using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {

    [SerializeField] private CanvasGroup _group;
    [SerializeField] protected float _curAlpha = -1;

    private void Start () {

        UIState current = UIStateManager.GetInstance ().GetState ();
        bool shouldShow = current == UIState.Menu;

        if (!shouldShow) {

            if (_curAlpha != 0) {
                _curAlpha = 0;
                DisableAll ();
                TweenToCurAlphaImmediately ();

            }

        }

    }

    private void Update () {

        UIState current = UIStateManager.GetInstance ().GetState ();
        bool shouldShow = current == UIState.Menu;

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

using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {

    [SerializeField] private Image[] _images;
    [SerializeField] private Text[] _texts;
    [SerializeField] private Selectable[] _selectables;
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

        foreach (Image img in _images) LeanTween.alpha (img.rectTransform, _curAlpha, 0.2f);
        foreach (Text txt in _texts) LeanTween.alphaText (txt.rectTransform, _curAlpha, 0.2f);

    }

    void TweenToCurAlphaImmediately () {

        foreach (Image img in _images) LeanTween.alpha (img.rectTransform, _curAlpha, 0);
        foreach (Text txt in _texts) LeanTween.alphaText (txt.rectTransform, _curAlpha, 0);

    }

    void DisableAll () {

        foreach (Selectable selectable in _selectables) {

            selectable.targetGraphic.raycastTarget = false;
            selectable.interactable = false;

        }

    }

    void EnableAll () {

        foreach (Selectable selectable in _selectables) {

            selectable.targetGraphic.raycastTarget = true;
            selectable.interactable = true;

        }

    }

}

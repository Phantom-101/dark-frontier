using UnityEngine;
using UnityEngine.UI;

public class ForwardSliderUI : MonoBehaviour {

    [SerializeField] private Slider _slider;
    [SerializeField] private Image _outline;
    [SerializeField] private Image _shade;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _handle;
    [SerializeField] protected float _curAlpha = -1;

    private void Update () {

        UIState current = UIStateManager.GetInstance ().GetState ();
        bool shouldShow = current == UIState.InSpace;

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

        PlayerController.GetInstance ().SetFwd (_slider.value);

    }

    void TweenToCurAlpha () {

        LeanTween.alpha (_outline.rectTransform, _curAlpha, 0.2f);
        LeanTween.alpha (_shade.rectTransform, _curAlpha, 0.2f);
        LeanTween.alpha (_fill.rectTransform, _curAlpha, 0.2f);
        LeanTween.alpha (_handle.rectTransform, _curAlpha, 0.2f);

    }

    void DisableAll () {

        _slider.interactable = false;
        _handle.raycastTarget = false;

    }

    void EnableAll () {

        _slider.interactable = true;
        _handle.raycastTarget = true;

    }

}

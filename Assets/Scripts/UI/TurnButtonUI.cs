using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnButtonUI : MonoBehaviour {

    [SerializeField] private Image _image;
    [SerializeField] private EventTrigger _eventTrigger;
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

    }

    void TweenToCurAlpha () {

        LeanTween.alpha (_image.rectTransform, _curAlpha, 0.2f);

    }

    void DisableAll () {

        _image.raycastTarget = false;
        _eventTrigger.enabled = false;

    }

    void EnableAll () {

        _image.raycastTarget = true;
        _eventTrigger.enabled = true;

    }

}

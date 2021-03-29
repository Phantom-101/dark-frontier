using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CanvasGroup))]
public class UIStateFader : MonoBehaviour {

    [SerializeField] private List<UIState> _shown;
    [SerializeField] private VoidEventChannelSO _changed;

    private CanvasGroup _group;
    private UIStateManager _uiStateManager;
    private float _target = -1;

    private void Start () {

        _group = GetComponent<CanvasGroup> ();
        _uiStateManager = UIStateManager.GetInstance ();

        _changed.OnEventRaised += OnUIStateChanged;

        Initialize ();

    }

    private void OnDestroy () {

        _changed.OnEventRaised -= OnUIStateChanged;

    }

    private void Initialize () {

        if (_group == null) {
            Debug.Log ($"Canvas group not found on gameObject {gameObject.name}");
            return;
        }
        if (_uiStateManager == null) {
            Debug.Log ("UI state manager not found");
            return;
        }

        if (_shown.Contains (_uiStateManager.GetState ())) Show (true);
        else Hide (true);

    }

    private void OnUIStateChanged () {

        if (_group == null) {
            Debug.Log ($"Canvas group not found on gameObject {gameObject.name}");
            return;
        }
        if (_uiStateManager == null) {
            Debug.Log ("UI state manager not found");
            return;
        }

        if (_shown.Contains (_uiStateManager.GetState ())) Show ();
        else Hide ();

    }

    private void Show (bool immediate = false) {

        if (immediate) SetAlpha (1);
        else TweenAlpha (1);
        _group.blocksRaycasts = true;
        _group.interactable = true;

    }

    private void Hide (bool immediate = false) {

        if (immediate) SetAlpha (0);
        else TweenAlpha (0);
        _group.blocksRaycasts = false;
        _group.interactable = false;

    }

    private void TweenAlpha (float target) {

        if (_target == target) return;
        _target = target;

        LeanTween.cancel (gameObject);
        LeanTween.alphaCanvas (_group, target, 0.2f).setIgnoreTimeScale (true);

    }

    private void SetAlpha (float target) {

        LeanTween.cancel (gameObject);
        _group.alpha = target;

    }

}

using System;
using UnityEngine;
using UnityEngine.UI;

public class SetupUI : MonoBehaviour {

    [SerializeField] private CanvasGroup _group;
    [SerializeField] private bool _shown;
    [SerializeField] private VoidEventChannelSO _decreaseScale;
    [SerializeField] private VoidEventChannelSO _increaseScale;
    [SerializeField] private Text _scaleText;
    [SerializeField] private float _scale;

    UIStateManager _uiSM;

    private void Start () {

        _scale = PlayerPrefs.HasKey ("UIScale") ? PlayerPrefs.GetFloat ("UIScale") : 1;
        _scaleText.text = $"{Math.Round (_scale, 1)}x";

        _decreaseScale.OnEventRaised += () => { ChangeUIScale (-0.1f); };
        _increaseScale.OnEventRaised += () => { ChangeUIScale (0.1f); };

        _uiSM = UIStateManager.GetInstance ();
        if (!PlayerPrefs.HasKey ("InitialSetupDone") || PlayerPrefs.GetInt ("InitialSetupDone") != 1) {

            _uiSM.AddState (UIState.Setup);

        }

    }

    private void Update () {

        UIState current = _uiSM.GetState ();
        bool shouldShow = current == UIState.Setup;

        if (!shouldShow) {

            if (_shown) {

                _shown = false;
                DisableAll ();
                TweenAlpha ();

            }
            return;

        }

        if (!_shown) {

            EnableAll ();
            _shown = true;
            TweenAlpha ();

        }

    }

    public void SetupDone () {

        _uiSM.RemoveState ();
        PlayerPrefs.SetInt ("InitialSetupDone", 1);

    }

    void ChangeUIScale (float change) {

        _scale = Mathf.Clamp (_scale + change, 0.5f, 2.0f);
        _scaleText.text = $"{Math.Round (_scale, 1)}x";
        PlayerPrefs.SetFloat ("UIScale", _scale);

    }

    void TweenAlpha () {

        LeanTween.alphaCanvas (_group, _shown ? 1 : 0, 0.2f).setIgnoreTimeScale (true);

    }

    void DisableAll () {

        _group.blocksRaycasts = false;
        _group.interactable = false;

    }

    void EnableAll () {

        _group.blocksRaycasts = true;
        _group.interactable = true;

    }

}

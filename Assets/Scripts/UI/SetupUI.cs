﻿using UnityEngine;

public class SetupUI : MonoBehaviour {

    [SerializeField] private CanvasGroup _group;
    [SerializeField] private bool _shown;

    UIStateManager _uiStateManager;

    private void Start () {

        _uiStateManager = UIStateManager.GetInstance ();
        if (!PlayerPrefs.HasKey ("InitialSetupDone") || PlayerPrefs.GetInt ("InitialSetupDone") != 1) {

            _uiStateManager.AddState (UIState.Setup);

        }

    }

    private void Update () {

        UIState current = _uiStateManager.GetState ();
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

        _uiStateManager.RemoveState ();
        PlayerPrefs.SetInt ("InitialSetupDone", 1);

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
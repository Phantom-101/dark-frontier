﻿using UnityEngine;
using UnityEngine.UI;

public class DockUndockButtonUI : MonoBehaviour {

    [SerializeField] private CanvasGroup _group;
    [SerializeField] private Image _dock;
    [SerializeField] private Image _undock;
    [SerializeField] private Button _button;
    [SerializeField] protected float _curAlpha = -1;

    private PlayerController _pc;

    private void Start () {

        _pc = PlayerController.GetInstance ();

        _button.onClick.AddListener (Toggle);

        _group.alpha = 0;

    }

    private void Update () {

        bool shouldShow = _pc.GetPlayer ().CanDockTarget () || _pc.GetPlayer ().CanUndock ();

        if (!shouldShow) {

            if (_curAlpha != 0) {

                _curAlpha = 0;
                DisableAll ();
                TweenToCurAlpha ();

            }
            return;

        }

        if (_pc.GetPlayer ().CanDockTarget ()) {
            _dock.enabled = true;
            _undock.enabled = false;
        } else {
            _dock.enabled = false;
            _undock.enabled = true;
        }

        if (_curAlpha == 0) {

            EnableAll ();
            _curAlpha = 1;
            TweenToCurAlpha ();

        }

    }

    void Toggle () {

        if (_pc.GetPlayer ().IsDocked ()) Undock ();
        else Dock ();

    }

    void Dock () {

        _pc.GetPlayer ().DockTarget ();

    }

    void Undock () {

        _pc.GetPlayer ().Undock ();

    }

    void TweenToCurAlpha () {

        LeanTween.alphaCanvas (_group, _curAlpha, 0.2f).setIgnoreTimeScale (true);

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
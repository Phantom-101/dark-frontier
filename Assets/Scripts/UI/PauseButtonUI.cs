using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUI : MonoBehaviour {

    [SerializeField] private Button _button;
    [SerializeField] private Image _pause;
    [SerializeField] private Image _play;
    [SerializeField] private bool _paused;

    private static PauseButtonUI _instance;

    private void Awake () {

        _instance = this;

        _button.onClick.AddListener (Toggle);

        if (_paused) {

            _pause.enabled = false;
            _play.enabled = true;

        } else {

            _pause.enabled = true;
            _play.enabled = false;

        }

    }

    void Toggle () {

        if (_paused) {

            _pause.enabled = true;
            _play.enabled = false;

            _paused = false;
            NotificationUI.GetInstance ().AddNotification ("Unpaused");

        } else {

            _pause.enabled = false;
            _play.enabled = true;

            _paused = true;
            NotificationUI.GetInstance ().AddNotification ("Paused");

        }

    }

    public bool IsPaused () { return _paused; }

    public static PauseButtonUI GetInstance () { return _instance; }

}

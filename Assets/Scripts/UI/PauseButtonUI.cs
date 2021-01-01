using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUI : MonoBehaviour {

    [SerializeField] private Button _button;
    [SerializeField] private Image _pause;
    [SerializeField] private Image _play;

    private void Awake () {

        _button.onClick.AddListener (Toggle);

        if (Time.timeScale == 0) {

            _pause.enabled = false;
            _play.enabled = true;

        } else {

            _pause.enabled = true;
            _play.enabled = false;

        }

    }

    void Toggle () {

        if (Time.timeScale == 0) {

            _pause.enabled = true;
            _play.enabled = false;

            Time.timeScale = 1;
            NotificationUI.GetInstance ().AddNotification ("Unpaused");

        } else {

            _pause.enabled = false;
            _play.enabled = true;

            Time.timeScale = 0;
            NotificationUI.GetInstance ().AddNotification ("Paused");

        }

    }

}

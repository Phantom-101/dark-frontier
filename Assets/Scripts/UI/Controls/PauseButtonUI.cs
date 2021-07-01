using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUI : MonoBehaviour {
    [SerializeField] private Button _button;
    [SerializeField] private Image _pause;
    [SerializeField] private Image _play;

    private void Start () {
        _button.onClick.AddListener (Toggle);

        if (TimescaleManager.Instance.Paused) {
            _pause.enabled = false;
            _play.enabled = true;
        } else {
            _pause.enabled = true;
            _play.enabled = false;
        }
    }

    void Toggle () {
        if (TimescaleManager.Instance.Paused) {
            _pause.enabled = true;
            _play.enabled = false;

            TimescaleManager.Instance.Paused = false;
            NotificationUI.GetInstance ().AddNotification ("Unpaused");
        } else {
            _pause.enabled = false;
            _play.enabled = true;

            TimescaleManager.Instance.Paused = true;
            NotificationUI.GetInstance ().AddNotification ("Paused");
        }
    }
}

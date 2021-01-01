using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour {

    private Queue<string> _notifications = new Queue<string> ();
    [SerializeField] private float _timer;
    [SerializeField] private float _timePerNotification;
    [SerializeField] private AnimationCurve _fadeCurve;
    [SerializeField] private Text _text;

    private static NotificationUI _instance;

    private void Awake () {

        _instance = this;

    }

    private void Update () {

        if (_timer <= 0) {

            if (_notifications.Count > 0) {

                _timer = _timePerNotification;
                _text.text = _notifications.Dequeue ();

            } else _text.text = "";

        } else {

            _timer -= Time.unscaledDeltaTime;
            _text.color = new Color (1, 1, 1, _fadeCurve.Evaluate ((_timePerNotification - _timer) / _timePerNotification));

        }

    }

    public static NotificationUI GetInstance () { return _instance; }

    public Queue<string> GetNotifications () { return _notifications; }

    public void AddNotification (string message) { _notifications.Enqueue (message); }

}

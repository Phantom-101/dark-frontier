using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnJoystickUI : MonoBehaviour {

    [SerializeField] private RectTransform _offsetTransform;
    [SerializeField] private RectTransform _buttonTransform;
    [SerializeField] private float _radius;
    [SerializeField] private bool _held;
    [SerializeField] private VoidEventChannelSO _down;
    [SerializeField] private VoidEventChannelSO _up;

    PlayerController pc;

    private void Awake () {

        _down.OnEventRaised += () => { _held = true; };
        _up.OnEventRaised += () => { _held = false; };

    }

    private void Start () {

        pc = PlayerController.GetInstance ();

    }

    private void Update () {

        if (_held) {

            Vector2 targetPos = new Vector2 (Input.mousePosition.x - _offsetTransform.anchoredPosition.x, Input.mousePosition.y - _offsetTransform.anchoredPosition.y);
            float mag = targetPos.magnitude;
            if (mag <= _radius) _buttonTransform.anchoredPosition = targetPos;
            else _buttonTransform.anchoredPosition = targetPos.normalized * _radius;

        } else {

            _buttonTransform.anchoredPosition = Vector2.zero;

        }

        pc.SetYaw (_buttonTransform.anchoredPosition.x / _radius);
        pc.SetPitch (_buttonTransform.anchoredPosition.y / _radius);

    }

}

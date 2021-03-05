using UnityEngine;

public class PlayerTurnJoystickUI : MonoBehaviour {

    [SerializeField] private RectTransform _areaTransform;
    [SerializeField] private RectTransform _buttonTransform;
    [SerializeField] private bool _held;
    [SerializeField] private Touch _touch;
    [SerializeField] private bool _mouseTouch;
    [SerializeField] private VoidEventChannelSO _down;
    [SerializeField] private VoidEventChannelSO _up;

    PlayerController pc;

    private void Awake () {

        _down.OnEventRaised += () => { _held = true; _touch = GetTouch (); };
        _up.OnEventRaised += () => { _held = false; };

    }

    private void Start () {

        pc = PlayerController.GetInstance ();

    }

    private void Update () {

        float radius = _areaTransform.sizeDelta.x / 2;

        if (_held) {

            if (_mouseTouch) _touch = new Touch {
                position = Input.mousePosition
            };
            Vector2 targetPos = new Vector2 (_touch.position.x - _areaTransform.position.x, _touch.position.y - _areaTransform.position.y);
            float mag = targetPos.magnitude;
            if (mag <= radius) _buttonTransform.anchoredPosition = targetPos;
            else _buttonTransform.anchoredPosition = targetPos.normalized * radius;

        } else {

            _buttonTransform.anchoredPosition = Vector2.zero;

        }

        pc.SetYaw (_buttonTransform.anchoredPosition.x / radius);
        pc.SetPitch (_buttonTransform.anchoredPosition.y / radius);

    }

    private Touch GetTouch () {

        if (Input.GetMouseButtonDown (0)) {

            Touch touch = new Touch {
                position = Input.mousePosition
            };
            _mouseTouch = true;
            return touch;

        }
        _mouseTouch = false;
        return Input.touches[Input.touches.Length - 1];

    }

}

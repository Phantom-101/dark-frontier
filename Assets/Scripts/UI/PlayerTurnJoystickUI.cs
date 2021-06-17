using UnityEngine;

public class PlayerTurnJoystickUI : MonoBehaviour {

    [SerializeField] private RectTransform _areaTransform;
    [SerializeField] private RectTransform _buttonTransform;
    [SerializeField] private RectTransform _topTransform;
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

        pc = PlayerController.Instance;

    }

    private void Update () {

        float radius = Vector3.Distance (transform.position, _topTransform.position);
        float localRadius = _topTransform.anchoredPosition.y;

        if (_held) {

            if (_mouseTouch) _touch = new Touch {
                position = Input.mousePosition
            };
            Vector2 targetPos = new Vector2 (_touch.position.x - _areaTransform.position.x, _touch.position.y - _areaTransform.position.y);
            targetPos /= radius / localRadius;
            float mag = targetPos.magnitude;
            if (mag <= localRadius) _buttonTransform.anchoredPosition = targetPos;
            else _buttonTransform.anchoredPosition = targetPos.normalized * localRadius;

        } else {

            _buttonTransform.anchoredPosition = Vector2.zero;

        }

        pc.SetYaw (_buttonTransform.anchoredPosition.x / radius);
        pc.SetPitch (-_buttonTransform.anchoredPosition.y / radius);

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

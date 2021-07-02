using UnityEngine;

public class PlayerTurnJoystick : MonoBehaviour {
    [SerializeField]
    private RectTransform _areaTransform;
    [SerializeField]
    private RectTransform _buttonTransform;
    [SerializeField]
    private RectTransform _topTransform;
    [SerializeField]
    private bool _held;
    [SerializeField]
    private Touch _touch;
    [SerializeField]
    private bool _mouseTouch;

    PlayerController _pc;

    private void Awake () {
        _pc = PlayerController.Instance;
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

        _pc.SetYaw (_buttonTransform.anchoredPosition.x / radius);
        _pc.SetPitch (-_buttonTransform.anchoredPosition.y / radius);
    }

    public void PointerDown () {
        _held = true;
        _touch = GetTouch ();
    }

    public void PointerUp () {
        _held = false;
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

using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnJoystick : MonoBehaviour {
    // Transforms
    [SerializeField] private RectTransform areaTransform;
    [SerializeField] private RectTransform buttonTransform;
    [SerializeField] private RectTransform topTransform;
    // Variables for keeping track of touches
    [SerializeField] private bool held;
    [SerializeField] private bool shouldGetNewTouch;
    [SerializeField] private Touch touch;

    PlayerController playerController;
    TouchManager touchManager;

    private void Awake () {
        playerController = PlayerController.Instance;
        touchManager = TouchManager.Instance;
    }

    private void Update () {
        float radius = Vector3.Distance (transform.position, topTransform.position);
        float localRadius = topTransform.anchoredPosition.y;

        if (held) {
            if (shouldGetNewTouch) {
                List<Touch> beganTouches = touchManager.GetBeganTouches ();
                if (beganTouches.Count > 0) {
                    touch = beganTouches[0];
                    shouldGetNewTouch = false;
                }
            }
            touch = touchManager.GetNextTouch (touch);
            Vector2 targetPos = new Vector2 (touch.position.x - areaTransform.position.x, touch.position.y - areaTransform.position.y);
            targetPos /= radius / localRadius;
            float mag = targetPos.magnitude;
            if (mag <= localRadius) buttonTransform.anchoredPosition = targetPos;
            else buttonTransform.anchoredPosition = targetPos.normalized * localRadius;
        } else {
            buttonTransform.anchoredPosition = Vector2.zero;
        }

        playerController.SetYaw (buttonTransform.anchoredPosition.x / radius);
        playerController.SetPitch (-buttonTransform.anchoredPosition.y / radius);
    }

    public void PointerDown () {
        held = true;
        shouldGetNewTouch = true;
    }

    public void PointerUp () {
        held = false;
    }
}

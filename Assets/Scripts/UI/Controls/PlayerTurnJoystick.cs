using System;
using System.Collections.Generic;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Input;
using UnityEngine;

namespace DarkFrontier.UI.Controls {
    public class PlayerTurnJoystick : MonoBehaviour {
        // Transforms
        [SerializeField] private RectTransform areaTransform;
        [SerializeField] private RectTransform buttonTransform;
        [SerializeField] private RectTransform topTransform;
        // Variables for keeping track of touches
        [SerializeField] private bool held;
        [SerializeField] private bool shouldGetNewTouch;
        private Touch touch;

        TouchManager touchManager;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        
        private void Awake () {
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

            iPlayerController.Value.SetYaw (buttonTransform.anchoredPosition.x / radius);
            iPlayerController.Value.SetPitch (-buttonTransform.anchoredPosition.y / radius);
        }

        public void PointerDown () {
            held = true;
            shouldGetNewTouch = true;
        }

        public void PointerUp () {
            held = false;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Foundation;
using UnityEngine;

namespace DarkFrontier.Input {
    public class TouchManager : SingletonBase<TouchManager> {
        public List<Touch> Touches {
            get => touches;
        }
        [SerializeField] private List<Touch> touches;

        // Raw positions
        [SerializeField] private Vector2 leftClickRawMousePosition;
        [SerializeField] private Vector2 rightClickRawMousePosition;
        [SerializeField] private Vector2 middleClickRawMousePosition;
        // Last frame position for calculating delta position
        [SerializeField] private Vector2 lastFrameMousePosition;
        // Move margins
        [SerializeField] private float moveMarginPercentage = 0.001f;
        [SerializeField] private float moveMargin;
        [SerializeField] private float sqrMoveMargin;
        // Delta position
        [SerializeField] private Vector2 mouseDeltaPosition;
        // Did the mouse move since last frame
        [SerializeField] private bool didMouseMoveSinceLastFrame;
        // Delta time since last move
        [SerializeField] private float deltaTimeSinceLastMouseMove;
        // Touch phases
        [SerializeField] private TouchPhase leftClickTouchPhase;
        [SerializeField] private TouchPhase rightClickTouchPhase;
        [SerializeField] private TouchPhase middleClickTouchPhase;

        public bool HasNextTouch (Touch touch) => touches.Any (t => t.fingerId == touch.fingerId);
        public Touch GetNextTouch (Touch touch) => touches.Find (t => t.fingerId == touch.fingerId);
        public int GetNextTouchIndex (Touch touch) => touches.FindIndex (t => t.fingerId == touch.fingerId);
        public List<Touch> GetBeganTouches () => touches.FindAll (t => t.phase == TouchPhase.Began);
        public List<Touch> GetMovedTouches () => touches.FindAll (t => t.phase == TouchPhase.Moved);
        public List<Touch> GetStationaryTouches () => touches.FindAll (t => t.phase == TouchPhase.Stationary);
        public List<Touch> GetEndedTouches () => touches.FindAll (t => t.phase == TouchPhase.Ended);
        public List<Touch> GetCancelledTouches () => touches.FindAll (t => t.phase == TouchPhase.Canceled);

        private void Awake () {
            // Initialize margins
            moveMargin = Mathf.Min (Screen.width * moveMarginPercentage, Screen.height * moveMarginPercentage);
            sqrMoveMargin = moveMargin * moveMargin;
        }

        private void Update () {
            // Get mouse delta position
            GetMouseDeltaPosition ();

            // Tick delta time
            deltaTimeSinceLastMouseMove += UnityEngine.Time.deltaTime;

            // Record raw mouse positions
            RecordRawMousePositions ();

            // Set touch phases
            SetTouchPhases ();

            // Set touches
            touches = GetTouches ();

            // Record last frame mouse position
            lastFrameMousePosition = UnityEngine.Input.mousePosition;
        }

        private void GetMouseDeltaPosition () {
            // Get mouse delta position
            mouseDeltaPosition = new Vector2 (UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y) - lastFrameMousePosition;
            // If position delta is within margin, set it to zero
            if (mouseDeltaPosition.sqrMagnitude <= sqrMoveMargin) {
                mouseDeltaPosition = Vector2.zero;
                didMouseMoveSinceLastFrame = false;
            }
            // If not, reset delta time since last mouse move
            else {
                deltaTimeSinceLastMouseMove = 0;
                didMouseMoveSinceLastFrame = true;
            }
        }

        private void RecordRawMousePositions () {
            // Left click
            if (UnityEngine.Input.GetMouseButtonDown (0)) leftClickRawMousePosition = UnityEngine.Input.mousePosition;
            // Right click
            if (UnityEngine.Input.GetMouseButtonDown (1)) rightClickRawMousePosition = UnityEngine.Input.mousePosition;
            // Middle click
            if (UnityEngine.Input.GetMouseButtonDown (2)) middleClickRawMousePosition = UnityEngine.Input.mousePosition;
        }

        private void SetTouchPhases () {
            // Left click
            if (UnityEngine.Input.GetMouseButtonDown (0)) leftClickTouchPhase = TouchPhase.Began;
            else if (UnityEngine.Input.GetMouseButtonUp (0)) leftClickTouchPhase = TouchPhase.Ended;
            else if (UnityEngine.Input.GetMouseButton (0)) leftClickTouchPhase = didMouseMoveSinceLastFrame ? TouchPhase.Moved : TouchPhase.Stationary;
            // Right click
            if (UnityEngine.Input.GetMouseButtonDown (1)) rightClickTouchPhase = TouchPhase.Began;
            else if (UnityEngine.Input.GetMouseButtonUp (1)) rightClickTouchPhase = TouchPhase.Ended;
            else if (UnityEngine.Input.GetMouseButton (1)) rightClickTouchPhase = didMouseMoveSinceLastFrame ? TouchPhase.Moved : TouchPhase.Stationary;
            // Middle click
            if (UnityEngine.Input.GetMouseButtonDown (2)) middleClickTouchPhase = TouchPhase.Began;
            else if (UnityEngine.Input.GetMouseButtonUp (2)) middleClickTouchPhase = TouchPhase.Ended;
            else if (UnityEngine.Input.GetMouseButton (2)) middleClickTouchPhase = didMouseMoveSinceLastFrame ? TouchPhase.Moved : TouchPhase.Stationary;
        }

        private List<Touch> GetTouches () {
            // Get finger touches
            List<Touch> ret = UnityEngine.Input.touches.ToList ();

            // Convert mouse clicks to touches

            // Left click
            if (UnityEngine.Input.GetMouseButton (0)) ret.Add (GetLeftClickTouch ());
            // Right click
            if (UnityEngine.Input.GetMouseButton (1)) ret.Add (GetRightClickTouch ());
            // Middle click
            if (UnityEngine.Input.GetMouseButton (2)) ret.Add (GetMiddleClickTouch ());

            // Return list of touches
            return ret;
        }

        private Touch GetLeftClickTouch () {
            return new Touch {
                fingerId = -1,
                position = UnityEngine.Input.mousePosition,
                rawPosition = leftClickRawMousePosition,
                deltaPosition = mouseDeltaPosition,
                deltaTime = deltaTimeSinceLastMouseMove,
                tapCount = 1,
                phase = leftClickTouchPhase,
                type = TouchType.Direct,
            };
        }

        private Touch GetRightClickTouch () {
            return new Touch {
                fingerId = -2,
                position = UnityEngine.Input.mousePosition,
                rawPosition = rightClickRawMousePosition,
                deltaPosition = mouseDeltaPosition,
                deltaTime = deltaTimeSinceLastMouseMove,
                tapCount = 2,
                phase = rightClickTouchPhase,
                type = TouchType.Direct,
            };
        }

        private Touch GetMiddleClickTouch () {
            return new Touch {
                fingerId = -3,
                position = UnityEngine.Input.mousePosition,
                rawPosition = middleClickRawMousePosition,
                deltaPosition = mouseDeltaPosition,
                deltaTime = deltaTimeSinceLastMouseMove,
                tapCount = 3,
                phase = middleClickTouchPhase,
                type = TouchType.Direct,
            };
        }
    }
}

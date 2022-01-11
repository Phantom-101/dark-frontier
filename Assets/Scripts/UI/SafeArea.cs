using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DarkFrontier.UI {
    [RequireComponent (typeof (Canvas))]
    public class SafeArea : MonoBehaviour {

        [SerializeField] private RectTransform _safeAreaTransform;

        private static readonly List<SafeArea> _safeAreas = new List<SafeArea> ();

        public static UnityEvent OnResolutionOrOrientationChanged = new UnityEvent ();

        private static bool _screenChangeVarsInitialized = false;
        private static ScreenOrientation _lastOrientation = ScreenOrientation.LandscapeLeft;
        private static Vector2 _lastResolution = Vector2.zero;
        private static Rect _lastSafeArea = Rect.zero;

        private Canvas _canvas;


        void Awake () {

            if (!_safeAreas.Contains (this))
                _safeAreas.Add (this);

            _canvas = GetComponent<Canvas> ();

            if (!_screenChangeVarsInitialized) {
                _lastOrientation = Screen.orientation;
                _lastResolution.x = Screen.width;
                _lastResolution.y = Screen.height;
                _lastSafeArea = Screen.safeArea;

                _screenChangeVarsInitialized = true;
            }

            ApplySafeArea ();

        }

        void Update () {

            if (_safeAreas[0] != this)
                return;

            if (Application.isMobilePlatform && Screen.orientation != _lastOrientation)
                OrientationChanged ();

            if (Screen.safeArea != _lastSafeArea)
                SafeAreaChanged ();

            if (Screen.width != _lastResolution.x || Screen.height != _lastResolution.y)
                ResolutionChanged ();

        }

        void ApplySafeArea () {

            if (_safeAreaTransform == null)
                return;

            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= _canvas.pixelRect.width;
            anchorMin.y /= _canvas.pixelRect.height;
            anchorMax.x /= _canvas.pixelRect.width;
            anchorMax.y /= _canvas.pixelRect.height;

            _safeAreaTransform.anchorMin = anchorMin;
            _safeAreaTransform.anchorMax = anchorMax;

        }

        void OnDestroy () {

            if (_safeAreas != null && _safeAreas.Contains (this))
                _safeAreas.Remove (this);

        }

        private static void OrientationChanged () {

            _lastOrientation = Screen.orientation;
            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;

            OnResolutionOrOrientationChanged.Invoke ();

        }

        private static void ResolutionChanged () {

            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;

            OnResolutionOrOrientationChanged.Invoke ();

        }

        private static void SafeAreaChanged () {

            _lastSafeArea = Screen.safeArea;

            for (int i = 0; i < _safeAreas.Count; i++) {
                _safeAreas[i].ApplySafeArea ();
            }

        }
    }
}

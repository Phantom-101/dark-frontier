using UnityEngine;

namespace DarkFrontier.Time {
    public class TimescaleManager : MonoBehaviour {

        [SerializeField] private float _targetFPS;
        [SerializeField] private float _currentTimescale;

        public bool Paused { get; set; }
        public static TimescaleManager Instance { get; private set; }

        float deltaTime = 0.0f;

        private void Awake () {

            if (Instance != null) {

                Destroy (gameObject);
                return;

            }
            Instance = this;

        }

        private void Update () {

            deltaTime += (UnityEngine.Time.unscaledDeltaTime - deltaTime) * 0.1f;

            if (Paused) {

                _currentTimescale = 0;
                UnityEngine.Time.timeScale = 0;

            } else {

                float fps = 1 / deltaTime;
                float ts = Mathf.Min (Mathf.Sqrt (fps / _targetFPS), 1);
                _currentTimescale = ts;
                UnityEngine.Time.timeScale = ts;

            }

        }

    }
}

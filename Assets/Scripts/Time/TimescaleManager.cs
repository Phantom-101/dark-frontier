using UnityEngine;

public class TimescaleManager : MonoBehaviour {

    [SerializeField] private float _targetFPS;
    [SerializeField] private float _currentTimescale;

    public bool Paused { get; set; }
    public static TimescaleManager Instance { get; private set; }

    private void Awake () {

        if (Instance != null) {

            Destroy (gameObject);
            return;

        }
        Instance = this;

    }

    private void Update () {

        if (Paused) {

            _currentTimescale = 0;
            Time.timeScale = 0;

        } else {

            float fps = 1 / Time.smoothDeltaTime;
            float ts = Mathf.Min (Mathf.Sqrt (fps / _targetFPS), 1);
            _currentTimescale = ts;
            Time.timeScale = ts;

        }

    }

}

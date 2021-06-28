using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour {
    private static bool _shuttingDown = false;
    private static readonly object _lock = new object ();
    private static T _instance;

    public static T Instance {
        get {
            if (_shuttingDown) {
                Debug.LogWarning ($"[SingletonBase] Instance '{typeof (T)}' already destroyed, returning null.");
                return null;
            }
            lock (_lock) {
                if (_instance == null) {
                    // Search for existing instance
                    _instance = FindObjectOfType<T> ();
                    // Create new instance if one doesn't already exist
                    if (_instance == null) {
                        // Need to create a new GameObject to attach the singleton to
                        var singletonObject = new GameObject ();
                        _instance = singletonObject.AddComponent<T> ();
                        singletonObject.name = typeof (T).ToString () + " (Singleton)";
                        // Make instance persistent
                        DontDestroyOnLoad (singletonObject);
                    }
                }
                return _instance;
            }
        }
    }

    private void OnApplicationQuit () {
        _shuttingDown = true;
    }

    private void OnDestroy () {
        _shuttingDown = true;
    }
}

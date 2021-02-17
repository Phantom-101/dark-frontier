using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtils : MonoBehaviour {

    private static SceneUtils _instance;

    public SceneUtils Instance { get => _instance; }

    private void Awake () {

        if (_instance != null) {

            Destroy (gameObject);
            return;

        }
        _instance = this;

    }

    public void LoadScene (string name) {

        SceneManager.LoadScene (name);

    }

    public void Quit () {

        Application.Quit ();

    }

}

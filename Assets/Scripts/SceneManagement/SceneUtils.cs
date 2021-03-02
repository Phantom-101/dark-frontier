using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtils : MonoBehaviour {
    public static SceneUtils Instance { get; private set; }

    private void Awake () {

        if (Instance != null) {

            Destroy (gameObject);
            return;

        }
        Instance = this;

    }

    public void LoadScene (string name) {

        Debug.Log ($"Loading scene {name}");

        SceneManager.LoadScene (name);

    }

    public void LoadScene (int id) {

        Debug.Log ($"Loading scene #{id}");

        SceneManager.LoadScene (id);

    }

    public void Quit () {

        Application.Quit ();

    }

}

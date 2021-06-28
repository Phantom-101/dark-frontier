using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtils : SingletonBase<SceneUtils> {
    public void LoadScene (string name) {
        UIStateManager.Instance.PurgeStates ();
        SceneManager.LoadScene (name);
    }

    public void LoadScene (int id) {
        UIStateManager.Instance.PurgeStates ();
        SceneManager.LoadScene (id);
    }

    public void Quit () {
        Application.Quit ();
    }
}

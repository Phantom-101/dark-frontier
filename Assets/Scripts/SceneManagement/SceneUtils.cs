using DarkFrontier.Foundation;
using DarkFrontier.UI.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkFrontier.SceneManagement {
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
}

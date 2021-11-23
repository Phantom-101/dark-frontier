using DarkFrontier.Foundation;
using DarkFrontier.UI.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkFrontier.SceneManagement {
    public class SceneUtils : SingletonBase<SceneUtils> {
        public void LoadScene (string aName) {
            UIStateManager.Instance.PurgeStates ();
            SceneManager.LoadScene (aName);
        }

        public void LoadScene (int aId) {
            UIStateManager.Instance.PurgeStates ();
            SceneManager.LoadScene (aId);
        }

        public AsyncOperation LoadSceneAsync(string aName) {
            UIStateManager.Instance.PurgeStates ();
            return SceneManager.LoadSceneAsync(aName);
        }

        public AsyncOperation LoadSceneAsync(int aId) {
            UIStateManager.Instance.PurgeStates ();
            return SceneManager.LoadSceneAsync(aId);
        }

        public void Quit () {
            Application.Quit ();
        }
    }
}

using DarkFrontier.UI.States;
using UnityEngine;

namespace DarkFrontier.UI.Settings {
    public class SetupUI : UIStateView {
        private void Start () {
            if (PlayerPrefs.GetInt ("InitialSetupDone", 0) == 0) {
                UIStateManager.AddState (Group);
                PlayerPrefs.SetInt ("InitialSetupDone", 1);
                PlayerPrefs.Save ();
            }
        }
    }
}

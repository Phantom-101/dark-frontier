using UnityEngine;

public class SetupUI : UIStateView {
    private void Start () {
        if (PlayerPrefs.GetInt ("InitialSetupDone", 0) == 0) {
            UIStateManager.AddState (Group);
        }
    }
}

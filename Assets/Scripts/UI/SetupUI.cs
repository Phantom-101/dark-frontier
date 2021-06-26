using UnityEngine;

public class SetupUI : UIStateView {
    private void Start () {
        if (!PlayerPrefs.HasKey ("InitialSetupDone") || PlayerPrefs.GetInt ("InitialSetupDone") != 1) {
            UIStateManager.AddState (Group);
        }
    }
}

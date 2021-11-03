using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Settings {
    public class PostProcessingSetting : MonoBehaviour {
        [SerializeField]
        private Toggle _toggle;

        private void Start () {
            // Initialize toggle state
            _toggle.isOn = PlayerPrefs.GetInt ("PostProcessingEnabled", 1) == 1;
            // Add listener
            _toggle.onValueChanged.AddListener (ChangePostProcessingEnabled);
        }

        void ChangePostProcessingEnabled (bool value) {
            // Change player prefs setting
            PlayerPrefs.SetInt ("PostProcessingEnabled", value ? 1 : 0);
            PlayerPrefs.Save ();
        }
    }
}

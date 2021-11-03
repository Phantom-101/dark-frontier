using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Settings {
    public class VSyncSetting : MonoBehaviour {
        [SerializeField]
        private Button _increaseVSyncCountButton;
        [SerializeField]
        private Button _decreaseVSyncCountButton;
        [SerializeField]
        private Text _vSyncCountText;
        [SerializeField]
        private int _vSyncCount;

        private void Start () {
            // Retrieve settings
            _vSyncCount = PlayerPrefs.GetInt ("VSyncCount", 1);
            // Update
            VSyncCountChanged ();
            // Set up button listeners
            _increaseVSyncCountButton.onClick.AddListener (() => ChangeVSyncCount (1));
            _decreaseVSyncCountButton.onClick.AddListener (() => ChangeVSyncCount (-1));
        }

        void ChangeVSyncCount (int change) {
            // Clamp vsync count
            _vSyncCount = Mathf.Clamp (_vSyncCount + change, 0, 4);
            // Update
            VSyncCountChanged ();
            // Save settings
            PlayerPrefs.SetInt ("VSyncCount", _vSyncCount);
            PlayerPrefs.Save ();
        }

        private void VSyncCountChanged () {
            // Change quality settings
            QualitySettings.vSyncCount = _vSyncCount;
            // Update text
            _vSyncCountText.text = _vSyncCount.ToString ();
        }
    }
}

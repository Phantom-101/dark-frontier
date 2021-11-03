using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Settings {
    public class TargetFPSSetting : MonoBehaviour {
        [SerializeField]
        private Button _increaseTargetFPSButton;
        [SerializeField]
        private Button _decreaseTargetFPSButton;
        [SerializeField]
        private Text _frameRateText;
        [SerializeField]
        private int _targetFrameRate;

        private void Start () {
            // Retrieve setting
            _targetFrameRate = PlayerPrefs.GetInt ("TargetFrameRate", 30);
            // Update
            TargetFrameRateChanged ();
            // Set up button listeners
            _increaseTargetFPSButton.onClick.AddListener (() => ChangeTargetFrameRate (15));
            _decreaseTargetFPSButton.onClick.AddListener (() => ChangeTargetFrameRate (-15));
        }

        private void ChangeTargetFrameRate (int change) {
            // Clamp target FPS
            _targetFrameRate = Mathf.Clamp (_targetFrameRate + change, 15, 120);
            // Update
            TargetFrameRateChanged ();
            // Save settings
            PlayerPrefs.SetInt ("TargetFrameRate", _targetFrameRate);
            PlayerPrefs.Save ();
        }

        private void TargetFrameRateChanged () {
            // Update application settings
            Application.targetFrameRate = _targetFrameRate;
            // Update text
            _frameRateText.text = _targetFrameRate.ToString ();
        }
    }
}

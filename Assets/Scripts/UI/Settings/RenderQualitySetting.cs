using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RenderQualitySetting : MonoBehaviour {
    [SerializeField]
    private Button _increaseQualityButton;
    [SerializeField]
    private Button _decreaseQualityButton;
    [SerializeField]
    private Text _qualityText;

    private void Start () {
        // Retrieve quality settings from player prefs
        QualitySettings.SetQualityLevel (PlayerPrefs.GetInt ("RenderQuality", 0));
        QualitySettingsChanged ();
        // Setup on click listeners
        _increaseQualityButton.onClick.AddListener (() => {
            QualitySettings.IncreaseLevel (true);
            QualitySettingsChanged ();
            SaveQualitySettings ();
        });
        _decreaseQualityButton.onClick.AddListener (() => {
            QualitySettings.DecreaseLevel (true);
            QualitySettingsChanged ();
            SaveQualitySettings ();
        });
    }

    private void SaveQualitySettings () {
        // Save quality settings to player prefs
        PlayerPrefs.SetInt ("RenderQuality", QualitySettings.GetQualityLevel ());
    }

    private void QualitySettingsChanged () {
        // Apply changes to graphics settings
        GraphicsSettings.renderPipelineAsset = QualitySettings.renderPipeline;
        // Update text
        _qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel ()];
    }
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RenderQualitySettingUI : MonoBehaviour {

    [SerializeField] private VoidEventChannelSO _decreaseQuality;
    [SerializeField] private VoidEventChannelSO _increaseQuality;
    [SerializeField] private Text _qualityText;

    private void Start () {

        QualitySettings.SetQualityLevel (PlayerPrefs.HasKey ("RenderQuality") ? PlayerPrefs.GetInt ("RenderQuality") : 0);
        GraphicsSettings.renderPipelineAsset = QualitySettings.renderPipeline;
        _qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel ()];

        _decreaseQuality.OnEventRaised += () => {
            QualitySettings.DecreaseLevel (true);
            GraphicsSettings.renderPipelineAsset = QualitySettings.renderPipeline;
            _qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel ()];
            PlayerPrefs.SetInt ("RenderQuality", QualitySettings.GetQualityLevel ());
        };
        _increaseQuality.OnEventRaised += () => {
            QualitySettings.IncreaseLevel (true);
            GraphicsSettings.renderPipelineAsset = QualitySettings.renderPipeline;
            _qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel ()];
            PlayerPrefs.SetInt ("RenderQuality", QualitySettings.GetQualityLevel ());
        };

    }

}

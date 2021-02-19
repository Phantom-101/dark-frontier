using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class RenderQualitySettingUI : MonoBehaviour {

    [SerializeField] private VoidEventChannelSO _decreaseQuality;
    [SerializeField] private VoidEventChannelSO _increaseQuality;
    [SerializeField] private RenderPipelineAsset[] _qualities;
    [SerializeField] private Text _qualityText;
    [SerializeField] private int _quality;

    private void Start () {

        _quality = PlayerPrefs.HasKey ("RenderQuality") ? PlayerPrefs.GetInt ("RenderQuality") : 1;
        _qualityText.text = GetQualityString ();

        _decreaseQuality.OnEventRaised += () => { ChangeQuality (-1); };
        _increaseQuality.OnEventRaised += () => { ChangeQuality (1); };

    }

    private void Update () {

        GraphicsSettings.renderPipelineAsset = _qualities[_quality];

    }

    void ChangeQuality (int change) {

        _quality = Mathf.Clamp (_quality + change, 0, 2);
        _qualityText.text = GetQualityString ();
        PlayerPrefs.SetInt ("RenderQuality", _quality);

    }

    string GetQualityString () {

        if (_quality == 0) return "Low";
        if (_quality == 1) return "Medium";
        if (_quality == 2) return "High";
        return "Unknown";

    }

}

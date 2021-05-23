using UnityEngine;
using UnityEngine.UI;

public class PostProcessingSettingUI : MonoBehaviour {

    [SerializeField] private VoidEventChannelSO _postProcessingEnabledToggled;
    [SerializeField] private Toggle _toggle;
    [SerializeField] private bool _on;

    private void Start () {

        _on = PlayerPrefs.HasKey ("PostProcessingEnabled") ? PlayerPrefs.GetInt ("PostProcessingEnabled") == 1 : true;
        _toggle.isOn = _on;

        _postProcessingEnabledToggled.OnEventRaised += ChangePostProcessingEnabled;

    }

    void ChangePostProcessingEnabled () {

        _on = _toggle.isOn;
        PlayerPrefs.SetInt ("PostProcessingEnabled", _on ? 1 : 0);

    }

}

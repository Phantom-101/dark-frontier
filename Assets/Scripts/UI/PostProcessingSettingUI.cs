using UnityEngine;
using UnityEngine.UI;

public class PostProcessingSettingUI : MonoBehaviour {

    [SerializeField] private Toggle _toggle;
    [SerializeField] private bool _on;

    private void Start () {

        _on = PlayerPrefs.HasKey ("PostProcessingEnabled") ? PlayerPrefs.GetInt ("PostProcessingEnabled") == 1 : true;
        _toggle.isOn = _on;

        _toggle.onValueChanged.AddListener (ChangePostProcessingEnabled);

    }

    void ChangePostProcessingEnabled (bool target) {

        _on = target;
        PlayerPrefs.SetInt ("PostProcessingEnabled", _on ? 1 : 0);

    }

}

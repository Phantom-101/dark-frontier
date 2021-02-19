using System;
using UnityEngine;
using UnityEngine.UI;

public class UIScaleSettingUI : MonoBehaviour {

    [SerializeField] private VoidEventChannelSO _decreaseScale;
    [SerializeField] private VoidEventChannelSO _increaseScale;
    [SerializeField] private Text _scaleText;
    [SerializeField] private float _scale;

    private void Start () {

        _scale = PlayerPrefs.HasKey ("UIScale") ? PlayerPrefs.GetFloat ("UIScale") : 1;
        _scaleText.text = $"{Math.Round (_scale, 1)}x";

        _decreaseScale.OnEventRaised += () => { ChangeUIScale (-0.1f); };
        _increaseScale.OnEventRaised += () => { ChangeUIScale (0.1f); };

    }

    void ChangeUIScale (float change) {

        _scale = Mathf.Clamp (_scale + change, 0.5f, 2.0f);
        _scaleText.text = $"{Math.Round (_scale, 1)}x";
        PlayerPrefs.SetFloat ("UIScale", _scale);

    }

}

using System;
using UnityEngine;
using UnityEngine.UI;

public class UIScaleSetting : MonoBehaviour {
    [SerializeField]
    private Button _increaseScaleButton;
    [SerializeField]
    private Button _decreaseScaleButton;
    [SerializeField]
    private Text _scaleText;
    [SerializeField]
    private float _scale;

    private void Start () {
        // Retrieve setting
        _scale = PlayerPrefs.GetFloat ("UIScale", 1);
        // Update
        UIScaleChanged ();
        // Set up button listeners
        _increaseScaleButton.onClick.AddListener (() => ChangeUIScale (0.1f));
        _decreaseScaleButton.onClick.AddListener (() => ChangeUIScale (-0.1f));
    }

    private void ChangeUIScale (float change) {
        // Clamp scale
        _scale = Mathf.Clamp (_scale + change, 0.5f, 2.0f);
        // Update
        UIScaleChanged ();
        // Save setting
        PlayerPrefs.SetFloat ("UIScale", _scale);
    }

    private void UIScaleChanged () {
        // Update text
        _scaleText.text = $"{Math.Round (_scale, 1)}x";
    }
}

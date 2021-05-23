﻿using UnityEngine;
using UnityEngine.UI;

public class GamePresetUI : MonoBehaviour {

    public Button Button;
    public Text NameText;
    public Text DescriptionText;
    public GamePresetSO Preset;

    private void Start () {

        NameText.text = Preset.PresetName;
        DescriptionText.text = Preset.Description;

    }

}
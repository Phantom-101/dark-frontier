﻿using UnityEngine;
using UnityEngine.UI;

public class WealthUI : MonoBehaviour {

    [SerializeField] private Text _text;

    private void Update () {

        _text.text = (PlayerController.GetInstance ().GetPlayer ().Faction?.GetWealth ().ToString () ?? "0") + " Cr";

    }

}

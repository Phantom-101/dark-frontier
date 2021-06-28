using System;
using UnityEngine;
using UnityEngine.UI;

public class SaveInfoUI : MonoBehaviour {
    [SerializeField] private Button _button;
    public Button Button {
        get { return _button; }
    }
    [SerializeField] private Text _nameText;
    public string Name {
        set { _nameText.text = value; }
    }
    [SerializeField] private Text _factionText;
    [SerializeField] private Text _dateText;
    public string Time {
        set { _dateText.text = DateTimeOffset.FromUnixTimeMilliseconds (long.Parse (value)).ToString (); }
    }
    [SerializeField] private Text _wealthText;
}

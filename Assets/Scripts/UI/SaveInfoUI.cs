using System;
using UnityEngine;
using UnityEngine.UI;

public class SaveInfoUI : MonoBehaviour {

    [SerializeField] private Button _button;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _factionText;
    [SerializeField] private Text _dateText;
    [SerializeField] private Text _wealthText;
    [SerializeField] private string _name;
    [SerializeField] private string _time;

    public Button GetButton () { return _button; }

    public string GetName () { return _name; }

    public void SetName (string name) { _name = name; }

    public string GetTime () { return _time; }

    public void SetTime (string time) { _time = time; }

    private void Update () {

        _nameText.text = _name;
        _dateText.text = DateTimeOffset.FromUnixTimeMilliseconds (long.Parse (_time)).ToString ();

    }

}

using UnityEngine;
using UnityEngine.UI;

public class UniverseInfoUI : MonoBehaviour {

    [SerializeField] private Button _button;
    [SerializeField] private Text _nameText;
    [SerializeField] private string _name;

    public Button GetButton () { return _button; }

    public string GetName () { return _name; }

    public void SetName (string name) { _name = name; }

    private void Update () {

        _nameText.text = _name;

    }

}

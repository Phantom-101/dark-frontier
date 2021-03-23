using UnityEngine;

public class LogUI : MonoBehaviour {

    [SerializeField] private int _tab = 0;
    [SerializeField] private LogTabUI[] _tabs;

    private void Start () {

        foreach (LogTabUI tab in _tabs) tab.SwitchOutImmediately ();

    }

    public int GetTab () { return _tab; }

    public void SetTab (int tab) {

        _tabs[_tab].SwitchOut ();
        _tab = tab;
        _tabs[_tab].SwitchIn ();

    }

}

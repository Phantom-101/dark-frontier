using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIStateDebugUI : MonoBehaviour {

    [SerializeField] private Text _text;

    UIStateManager _uiSM;

    private void Start () {

        _uiSM = UIStateManager.GetInstance ();

    }

    private void Update () {

        StringBuilder sb = new StringBuilder ();
        sb.Append ("UI State Stack: ");
        UIState[] states = _uiSM.GetStates ();
        foreach (UIState state in states) sb.Append (state.ToString () + "/");
        sb.Remove (sb.Length - 1, 1);
        _text.text = sb.ToString ();

    }

}

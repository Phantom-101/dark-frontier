using System;
using System.Collections.Generic;
using UnityEngine;

public class UIStateManager : MonoBehaviour {

    [SerializeField] private UIState _initial;
    private Stack<UIState> _state = new Stack<UIState> ();
    [SerializeField] private UIStateAddedEventChannelSO _added;
    [SerializeField] private VoidEventChannelSO _removed;

    private static UIStateManager _instance;

    private void Awake () {

        _instance = this;

        _added.OnUIStateAdded += AddState;
        _removed.OnEventRaised += RemoveState;

        _state.Push (_initial);

    }

    public UIState GetState () { return _state.Peek (); }

    public void AddState (UIState state) { if (!_state.Contains (state)) _state.Push (state); }

    public void RemoveState () { _state.Pop (); }

    public UIState[] GetStates () { return _state.ToArray (); }

    public static UIStateManager GetInstance () { return _instance; }

}

[Serializable]
public enum UIState {

    MainMenu,
    Setup,
    InSpace,
    Docked,
    Menu,
    Log,
    SaveSelection,

}
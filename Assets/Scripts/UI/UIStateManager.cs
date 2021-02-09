using System;
using System.Collections.Generic;
using UnityEngine;

public class UIStateManager : MonoBehaviour {

    private Stack<UIState> _state = new Stack<UIState> ();
    [SerializeField] private UIStateAddedEventChannelSO _added;
    [SerializeField] private VoidEventChannelSO _removed;

    private static UIStateManager _instance;

    private void Awake () {

        _instance = this;

        _added.OnUIStateAdded += AddState;
        _removed.OnEventRaised += RemoveState;

        _state.Push (UIState.InSpace);

    }

    public UIState GetState () { return _state.Peek (); }

    public void AddState (UIState state) { if (!_state.Contains (state)) _state.Push (state); }

    public void RemoveState () { _state.Pop (); }

    public static UIStateManager GetInstance () { return _instance; }

}

[Serializable]
public enum UIState {

    InSpace,
    Docked,
    Menu,
    Log,
    SaveSelection,

}
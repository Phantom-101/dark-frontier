using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIStateManager : SingletonBase<UIStateManager> {
    [SerializeField] private List<UIState> _states = new List<UIState> ();
    private readonly Dictionary<UIState, UIStateInfo> _infos = new Dictionary<UIState, UIStateInfo> ();
    public EventHandler StatesChanged;

    public bool IsShown (CanvasGroup group) {
        // Iterate through states
        foreach (UIState state in _states) {
            // See if it is currently being shown
            if (state.Group == group && _infos[state].State) return true;
        }
        return false;
    }

    public bool AddState (CanvasGroup group, string name = null, bool showBelow = false, bool alwaysShow = false, bool ephemeral = false) {
        // Ignore if canvas group is null
        // We need to do this here even if AddState (UIState) already does so because we need to retrieve the name of the game object the group is on
        // Also, this saves some execution time if group is indeed null
        if (group == null) return false;
        // Create UI state
        UIState state = new UIState {
            Name = name ?? group.gameObject.name,
            Group = group,
            ShowBelow = showBelow,
            AlwaysShow = alwaysShow,
            Ephemeral = ephemeral,
        };
        // Add the UI state
        return AddState (state);
        // No need to notify listeners here as AddState (UIState) already does so
    }

    public bool AddState (UIState state) {
        // Ignore states without a canvas group
        if (state.Group == null) return false;
        // Remove previous references
        _states.RemoveAll (s => s == state);
        // Add the state
        _states.Add (state);
        // Create its associated info
        _infos[state] = new UIStateInfo ();
        // Disable the group
        DisableGroup (state, 0);
        // Recalculate shown
        RecalculateShown ();
        // Notify listeners
        StatesChanged?.Invoke (this, EventArgs.Empty);
        return true;
    }

    public bool AddState (GameObject target, string name = null, bool showBelow = false, bool alwaysShow = false, bool ephemeral = false) {
        // Ignore null game objects
        if (target == null) return false;
        // Ignore game objects without a canvas group
        CanvasGroup group = target.GetComponent<CanvasGroup> ();
        if (group == null) return false;
        // Add state
        return AddState (group, name, showBelow, alwaysShow, ephemeral);
    }

    public GameObject AddStateFromPrefab (GameObject prefab, string name = null, bool showBelow = false, bool alwaysShow = false, bool ephemeral = true) {
        // Ignore null prefabs
        if (prefab == null) return null;
        // Ignore prefabs without a canvas group
        if (prefab.GetComponent<CanvasGroup> () == null) return null;
        // Instantiate
        GameObject instantiated = Instantiate (prefab, PrefabViewRoot.Instance.transform);
        CanvasGroup group = instantiated.GetComponent<CanvasGroup> ();
        AddState (group, name, showBelow, alwaysShow, ephemeral);
        return instantiated;
    }

    public void RemoveState () {
        // Remove the state and its associated info
        RemoveState (_states.Last ());
        // No need to notify listeners here as RemoveState (UIState) already does so
    }

    public void PurgeStates () {
        // Remove all states
        while (_states.Count > 0) {
            RemoveState ();
        }
    }

    private void RemoveState (UIState state) {
        // Disable the state
        DisableGroup (state);
        // Remove the state
        _states.RemoveAll (s => s == state);
        // Remove its associated info
        _infos.Remove (state);
        // Recalculate shown
        RecalculateShown ();
        // Notify listeners
        StatesChanged?.Invoke (this, EventArgs.Empty);
    }

    private void EnableGroup (UIState state, float transition = 0.2f) {
        // Get the state info
        UIStateInfo info = _infos[state];
        // Change the state
        info.State = true;
        // Change group parameters
        state.Group.blocksRaycasts = true;
        state.Group.interactable = true;
        // Tween alpha
        if (transition == 0) state.Group.alpha = 1;
        else {
            // Cancel preexisting tween if there is one
            LeanTween.cancel (state.Group.gameObject, info.TweenId);
            // Create new tween
            info.TweenId = LeanTween.value (state.Group.gameObject, state.Group.alpha, 1, 0.2f).setOnUpdate (value => state.Group.alpha = value).id;
        }
    }

    private void DisableGroup (UIState state, float transition = 0.2f) {
        // Get the state info
        UIStateInfo info = _infos[state];
        // Change the state
        info.State = false;
        // Change group parameters
        state.Group.blocksRaycasts = false;
        state.Group.interactable = false;
        // Tween alpha
        if (transition == 0) state.Group.alpha = 0;
        else {
            // Cancel preexisting tween if there is one
            LeanTween.cancel (state.Group.gameObject, info.TweenId);
            // Create new tween
            info.TweenId = LeanTween.value (state.Group.gameObject, state.Group.alpha, 0, 0.2f)
                .setOnUpdate (value => state.Group.alpha = value)
                .setOnComplete (() => {
                    if (state.Ephemeral) Destroy (state.Group.gameObject);
                }).id;
        }
    }

    private void AdjustState (UIState state, bool target, float transition = 0.2f) {
        // Adjust the state of the UI state based on the target boolean
        if (target) EnableGroup (state, transition);
        else DisableGroup (state, transition);
    }

    private void RecalculateShown () {
        // Check data
        CheckData ();
        // Initialize index and show boolean
        int i = _states.Count - 1;
        bool show = true;
        // Iterate through canvas groups
        while (i >= 0) {
            // Get state and its associated info
            UIState state = _states[i];
            UIStateInfo info = _infos[state];
            // If a change is necessary, perform it
            if (info.State != (show || state.AlwaysShow)) AdjustState (state, show || state.AlwaysShow);
            // Update show boolean
            show = show && state.ShowBelow;
            // Decrement index to move on to a lower layer
            i--;
        }
    }

    private void CheckData () {
        // Ensure that all data are safe to work on
        _states.RemoveAll (s => s.Group == null);
        _infos.Keys.Where (s => !_states.Contains (s)).ToList ().ForEach (s => _infos.Remove (s));
    }

    private void Update () {
        // Not needed, just there for runtime adjustments
        RecalculateShown ();
    }
}

[Serializable]
public class UIState {
    /// <summary>
    /// Name of the UI state. Optional.
    /// </summary>
    public string Name;
    /// <summary>
    /// Canvas group associated with the UI state.
    /// </summary>
    public CanvasGroup Group;
    /// <summary>
    /// Whether or not UI states below this will be shown.
    /// </summary>
    public bool ShowBelow;
    /// <summary>
    /// Whether or not this UI state will always show regardless of higher UI states.
    /// </summary>
    public bool AlwaysShow;
    /// <summary>
    /// Whether or not this UI state should be destroyed after it is exited out of.
    /// </summary>
    public bool Ephemeral;
}

[Serializable]
class UIStateInfo {
    /// <summary>
    /// Whether or not its associated UIState is being tweened to show.
    /// </summary>
    public bool State;
    /// <summary>
    /// The id of the tweening action.
    /// </summary>
    public int TweenId;
}
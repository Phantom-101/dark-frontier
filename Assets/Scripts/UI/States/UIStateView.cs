using System;
using UnityEngine;

[RequireComponent (typeof (CanvasGroup))]
public class UIStateView : MonoBehaviour {
    private CanvasGroup _group;
    public CanvasGroup Group {
        get {
            if (_group == null) _group = GetComponent<CanvasGroup> ();
            return _group;
        }
    }
    public bool IsShown {
        get { return UIStateManager.IsShown (Group); }
    }
    private UIStateManager _uiStateManager;
    public UIStateManager UIStateManager {
        get {
            if (_uiStateManager == null) _uiStateManager = UIStateManager.Instance;
            return _uiStateManager;
        }
    }

    private void Awake () {
        if (UIStateManager != null) {
            UIStateManager.StatesChanged += HandleStateChange;
        }
    }

    private void Update () {
        UpdateUI ();
    }

    private void OnDestroy () {
        if (UIStateManager != null) {
            UIStateManager.StatesChanged -= HandleStateChange;
        }
    }

    private void HandleStateChange (object sender, EventArgs args) { StateChanged (); }
    protected virtual void StateChanged () { }
    protected virtual void UpdateUI () { }
}

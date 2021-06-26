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
    private UIStateManager _uIStateManager;
    public UIStateManager UIStateManager {
        get {
            if (_uIStateManager == null) _uIStateManager = UIStateManager.Instance;
            return _uIStateManager;
        }
    }

    private void Awake () {
        UIStateManager.StatesChanged += (_, __) => StateChanged ();
    }

    private void Update () {
        UpdateUI ();
    }

    protected virtual void StateChanged () { }
    protected virtual void UpdateUI () { }
}

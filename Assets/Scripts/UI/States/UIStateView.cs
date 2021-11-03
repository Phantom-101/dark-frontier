using System;
using UnityEngine;

namespace DarkFrontier.UI.States {
    [RequireComponent (typeof (CanvasGroup))]
    public class UIStateView : MonoBehaviour {
        public CanvasGroup Group {
            get {
                if (_group == null) _group = GetComponent<CanvasGroup> ();
                return _group;
            }
        }
        private CanvasGroup _group;

        public bool IsShown { get => UIStateManager.IsShown (Group); }

        public UIStateManager UIStateManager {
            get {
                if (_uiStateManager == null) _uiStateManager = UIStateManager.Instance;
                return _uiStateManager;
            }
        }
        private UIStateManager _uiStateManager;

        private void Awake () {
            if (UIStateManager != null) {
                UIStateManager.StatesChanged += HandleStateChange;
            }
        }

        private void Update () {
            OnUpdate ();
        }

        private void OnDestroy () {
            if (UIStateManager != null) {
                UIStateManager.StatesChanged -= HandleStateChange;
            }
        }

        private void HandleStateChange (object sender, EventArgs args) { OnStateChanged (); }
        protected virtual void OnStateChanged () { }
        protected virtual void OnUpdate () { }
    }
}

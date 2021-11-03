using UnityEngine;

namespace DarkFrontier.UI.States {
    [CreateAssetMenu (menuName = "References/UIStateManager/Base")]
    public class UIStateManagerReference : ScriptableObject {
        private UIStateManager _uiStateManager;
        public UIStateManager UIStateManager {
            get {
                if (_uiStateManager == null) _uiStateManager = UIStateManager.Instance;
                return _uiStateManager;
            }
        }

        public bool IsShown (CanvasGroup group) { return UIStateManager.IsShown (group); }
        public void AddState (CanvasGroup group) { UIStateManager.AddState (group); }
        public void AddStateShowBelow (CanvasGroup group) { UIStateManager.AddState (group, showBelow: true); }
        public void AddStateAlwaysShow (CanvasGroup group) { UIStateManager.AddState (group, alwaysShow: true); }
        public void AddStateShowBelowAndAlwaysShow (CanvasGroup group) { UIStateManager.AddState (group, showBelow: true, alwaysShow: true); }
        public void AddState (UIState state) { UIStateManager.AddState (state); }
        public void RemoveState () { UIStateManager.RemoveState (); }
        public void PurgeStates () { UIStateManager.PurgeStates (); }
    }
}

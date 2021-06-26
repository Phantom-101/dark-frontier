using UnityEngine;

[RequireComponent (typeof (CanvasGroup))]
public class UIInitialStateInjector : MonoBehaviour {
    private void Awake () {
        UIStateManager.Instance.AddState (GetComponent<CanvasGroup> ());
    }
}

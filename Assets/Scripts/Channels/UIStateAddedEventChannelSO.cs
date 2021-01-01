using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Events/UI State Added Event Channel")]
public class UIStateAddedEventChannelSO : ScriptableObject {

    public UnityAction<UIState> OnUIStateAdded;

    public void RaiseEvent (UIState newState) {

        if (OnUIStateAdded != null) OnUIStateAdded.Invoke (newState);

    }

    public void RaiseEvent (UIStateSO newState) {

        if (OnUIStateAdded != null) OnUIStateAdded.Invoke (newState.State);

    }

}

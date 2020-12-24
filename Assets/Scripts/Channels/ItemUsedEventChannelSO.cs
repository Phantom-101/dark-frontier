using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Events/Item Used Event Channel")]
public class ItemUsedEventChannelSO : ScriptableObject {

    public UnityAction<Structure> OnItemUsed;

    public void RaiseEvent (Structure user) {

        if (OnItemUsed != null) OnItemUsed.Invoke (user);

    }

}


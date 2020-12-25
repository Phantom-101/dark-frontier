using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (menuName = "Events/Item Used Event Channel")]
public class ItemUsedEventChannelSO : ScriptableObject {

    public UnityAction<ItemSO, Structure> OnItemUsed;

    public void RaiseEvent (ItemSO itemUsed, Structure itemUser) {

        if (OnItemUsed != null) OnItemUsed.Invoke (itemUsed, itemUser);

    }

}


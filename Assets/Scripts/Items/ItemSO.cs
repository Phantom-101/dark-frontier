using System;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Item")]
public class ItemSO : ScriptableObject {

    [Header ("Information")]
    public string Name;
    public string Description;
    public Texture2D Icon;
    public double Size;

    [Header ("Usage")]
    public ItemUsedEventChannelSO OnUseChannel;

    private void Awake () {

        if (OnUseChannel != null) OnUseChannel.OnItemUsed += OnUse;

    }

    protected virtual void OnUse (ItemSO itemUsed, Structure itemUser) {

        if (itemUsed != this) return;

        if (OnUseChannel == null) throw new Exception ("OnUse called on unusable item " + Name);

    }

    public virtual bool CanUse (Structure user) {

        return OnUseChannel != null;

    }

}


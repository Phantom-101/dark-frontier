using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemSO : ScriptableObject {

    [Header ("Information")]
    public string Name;
    public string Description;
    public Texture2D Icon;
    public double Size;

    [Header ("Usage")]
    public ItemUsedEventChannelSO OnUsedChannel;

    private void Awake () {

        if (OnUsedChannel != null) OnUsedChannel.OnItemUsed += OnUsed;

    }

    protected virtual void OnUsed (Structure user) {

        if (OnUsedChannel == null) throw new Exception ("OnUsed called on unusable item " + Name);

    }

    public virtual bool CanUse (Structure user) {

        return OnUsedChannel != null;

    }

}


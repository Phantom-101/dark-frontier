using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Item")]
public class ItemSO : ScriptableObject {
    public string Name;
    public string Description;
    public string Id;
    public Sprite Icon;
    public float Volume;
    public List<ItemTag> Tags;
    public bool Usable;

    public virtual void OnUse (Structure user) {
        if (!Usable) return;
    }

    public virtual bool CanUse () {
        return Usable;
    }
}


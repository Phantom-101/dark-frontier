using UnityEngine;

[CreateAssetMenu (menuName = "Items/Item")]
public class ItemSO : ScriptableObject {

    [Header ("Information")]
    public string Name;
    public string Description;
    public Texture2D Icon;
    public double Size;
    public bool Usable;

    public virtual void OnUse (Structure user) {

        if (!Usable) return;

    }

    public virtual bool CanUse () {

        return Usable;

    }

}


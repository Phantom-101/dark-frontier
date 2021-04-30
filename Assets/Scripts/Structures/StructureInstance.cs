using UnityEngine;

public class StructureInstance : IStructure, ISavable<IStructure> {
    public string Id {
        get;
        private set;
    }
    public string Name {
        get;
        set;
    }
    public string Description {
        get;
        set;
    }
    public IEntity Controller {
        get;
        set;
    }
    public float Hitpoints {
        get;
        private set;
    }
    public StructureStat SignatureSize {
        get;
        private set;
    }
    public StructureStat MaxHitpoints {
        get;
        private set;
    }
    public event OnDestroyedEventHandler OnDestroyed;

    public bool CanInitialize () {
        return true;
    }

    public ISaveData<IStructure> GetSaveData () {
        var data = new InterfacedStructureSaveData ();
        data.Type = "structure";
        return data;
    }

    public void Initialize () {
        if (!CanInitialize ()) return;
    }

    public void LoadSaveData (ISaveData<IStructure> data) { }

    public void TakeDamage (float amount, IInfo damager) {
        Hitpoints = Mathf.Clamp (Hitpoints - amount, 0, MaxHitpoints.GetAppliedValue ());
        if (Hitpoints == 0) OnDestroyed?.Invoke (this, damager);
    }
}


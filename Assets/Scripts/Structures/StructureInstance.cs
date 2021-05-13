using UnityEngine;

public class StructureInstance : IStructure, ISerializable<IStructure> {
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
    public StatInstance SignatureSize {
        get;
        private set;
    }
    public StatInstance MaxHitpoints {
        get;
        private set;
    }
    public event OnDestroyedEventHandler OnDestroyed;

    public StructureInstance (InterfacedStructureSaveData serialized) {
        Id = serialized.Id;
        Name = serialized.Name;
        Description = serialized.Description;
        // TODO find controller with id
        Hitpoints = serialized.Hitpoints;
        SignatureSize = StatSaveDataType.Parse (serialized.SignatureSize) as StatInstance;
        MaxHitpoints = StatSaveDataType.Parse (serialized.MaxHitpoints) as StatInstance;
    }

    public bool CanInitialize () {
        return true;
    }

    public ISerialized<IStructure> GetSerialized () { return new InterfacedStructureSaveData (this); }

    public void Initialize () {
        if (!CanInitialize ()) return;
    }

    public void TakeDamage (float amount, IInfo damager) {
        // TODO change to applied value
        Hitpoints = Mathf.Clamp (Hitpoints - amount, 0, MaxHitpoints.BaseValue);
        if (Hitpoints == 0) OnDestroyed?.Invoke (this, damager);
    }
}


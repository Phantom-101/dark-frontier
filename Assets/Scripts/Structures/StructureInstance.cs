using System;
using System.Collections.Generic;
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
    public List<IStructureSegment> Segments {
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
        Segments = serialized.Segments.ConvertAll ((e) => StructureSegmentSaveDataType.Parse (e) as IStructureSegment);
    }

    public ISerialized<IStructure> GetSerialized () { return new InterfacedStructureSaveData (this); }

    public void Initialize () {
        if (string.IsNullOrWhiteSpace (Id)) Id = Guid.NewGuid ().ToString ();
        if (SignatureSize == null) SignatureSize = new StatInstance (0, "Signature Size", "The size of the cross section on a scanner.");
        if (MaxHitpoints == null) MaxHitpoints = new StatInstance (0, "Max Hitpoints", "The maximum number of hitpoints a structure can have.");
        if (Segments == null) Segments = new List<IStructureSegment> ();
    }

    public void TakeDamage (float amount, IInfo damager) {
        Hitpoints = Mathf.Clamp (Hitpoints - amount, 0, MaxHitpoints.AppliedValue);
        if (Hitpoints == 0) OnDestroyed?.Invoke (this, damager);
    }
}


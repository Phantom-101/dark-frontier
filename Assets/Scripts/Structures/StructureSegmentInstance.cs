using System;
using System.Collections.Generic;
using UnityEngine;

public class StructureSegmentInstance : IStructureSegment, ISerializable<IStructureSegment> {
    public string Id {
        get;
        private set;
    }
    public IStat MaxHitpoints {
        get;
        private set;
    }
    public float Hitpoints {
        get;
        private set;
    }
    public IStructure Structure {
        get;
        private set;
    }
    public List<IEquipmentSlot> EquipmentSlots {
        get;
        private set;
    }

    public event OnDestroyedEventHandler OnDestroyed;

    public StructureSegmentInstance (StructureSegmentSerialized serialized) {
        Id = serialized.Id;
        MaxHitpoints = StatSerializedParser.Parse (serialized.MaxHitpoints) as IStat;
        Hitpoints = serialized.Hitpoints;
        // TODO get structure from id
        EquipmentSlots = serialized.EquipmentSlots.ConvertAll (e => EquipmentSlotSerializedParser.Parse (e) as IEquipmentSlot);
    }

    public void Initialize () {
        if (string.IsNullOrWhiteSpace (Id)) Id = Guid.NewGuid ().ToString ();
        if (MaxHitpoints == null) MaxHitpoints = new StatInstance (0, "Max Hitpoints", "The maximum number of hitpoints a segment can have.");
        if (EquipmentSlots == null) EquipmentSlots = new List<IEquipmentSlot> ();
    }

    public void TakeDamage (float amount, IInfo damager) {
        float temp = Hitpoints;
        Hitpoints = Mathf.Clamp (Hitpoints - amount, 0, MaxHitpoints.AppliedValue);
        if (Hitpoints == 0) OnDestroyed?.Invoke (this, damager);
        Structure?.TakeDamage (temp - Hitpoints, damager);
    }

    public ISerialized<IStructureSegment> GetSerialized () { return new StructureSegmentSerialized (this); }
}


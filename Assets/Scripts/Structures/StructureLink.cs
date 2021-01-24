using System;
using UnityEngine;

[Serializable]
public class StructureLink {

    [SerializeField] private string _aId;
    [SerializeField] private string _bId;
    [SerializeField] private Structure _a;
    [SerializeField] private Structure _b;
    [SerializeField] private StructureLinkType _type;

    public StructureLink (string aId, string bId, StructureLinkType type) {
        _aId = aId;
        _bId = bId;
        _type = type;
    }

    public string GetAId () { return _aId; }

    public string GetBId () { return _bId; }

    public Structure GetA () {

        if (_a == null) _a = StructureManager.GetInstance ().GetStructure (_aId);
        return _a;

    }

    public Structure GetB () {

        if (_b == null) _b = StructureManager.GetInstance ().GetStructure (_bId);
        return _b;

    }

    public StructureLinkType GetLinkType () { return _type; }

}

public enum StructureLinkType {

    Docking,
    Attachment,
    Sync,

}

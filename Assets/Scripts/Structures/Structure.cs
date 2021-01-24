using System;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {

    [SerializeField] private StructureSO _profile;
    [SerializeField] private string _id;

    [SerializeField] private StringToStructureStatDictionary _stats = new StringToStructureStatDictionary ();
    [SerializeField] private float _hull;
    [SerializeField] private Faction _faction;
    [SerializeField] private FactionSO _initialFaction;

    [SerializeField] private List<EquipmentSlot> _equipmentSlots = new List<EquipmentSlot> ();
    [SerializeField] private ItemSOToIntDictionary _inventory = new ItemSOToIntDictionary ();

    [SerializeReference] private AI _ai;
    [SerializeField] private AISO _initialAI;
    [SerializeField] private Structure _target;

    [SerializeField] private Sector _sector;
    [SerializeField] private List<StructureLink> _links;

    private void Awake () {

        if (transform.parent != null) {

            _sector = transform.parent.GetComponent<Sector> ();
            if (_sector != null) _sector.Entered (this);

        }

    }

    private void Start () {

        if (StructureManager.GetInstance () != null) StructureManager.GetInstance ().AddStructure (this);

        if (string.IsNullOrEmpty (_id)) _id = Guid.NewGuid ().ToString ();

        if (_stats.Keys.Count == 0) {

            // Initialize structure stats

            _stats.Add ("sensor_strength", new StructureStat ("sensor_strength", _profile.SensorStrength));
            _stats.Add ("detectability", new StructureStat ("detectability", _profile.Detectability));
            _stats.Add ("docking_bay_size", new StructureStat ("docking_bay_size", _profile.DockingBaySize));
            _stats.Add ("damage_multiplier", new StructureStat ("damage_multiplier", 1));
            _stats.Add ("recharge_multiplier", new StructureStat ("recharge_multiplier", 1));
            _stats.Add ("speed_multiplier", new StructureStat ("speed_multiplier", 1));
            _stats.Add ("agility_multiplier", new StructureStat ("agility_multiplier", 1));

        }

        if (_initialFaction != null) _faction = FactionManager.GetInstance ().GetFaction (_initialFaction.Id);
        if (_initialAI != null) _ai = _initialAI.GetAI (this);

        if (_faction != null) _faction.AddProperty (this);

    }

    public StructureSO GetProfile () { return _profile; }

    public string GetId () { return _id; }

    public void SetId (string id) { _id = id; }

    public float GetHull () { return _hull; }

    public Faction GetFaction () { return _faction; }

    public void SetFaction (Faction faction) { _faction = faction; }

    public void ChangeHull (float amount) {

        _hull += amount;

        if (_hull <= 0) {

            if (_faction != null) _faction.RemoveProperty (this);
            _profile.OnDestroyedChannel.RaiseEvent (this);

        }

    }

    public void SetHull (float target) {

        _hull = target;

        if (_hull <= 0) {

            if (_faction != null) _faction.RemoveProperty (this);
            _profile.OnDestroyedChannel.RaiseEvent (this);

        }

    }

    public List<EquipmentSlot> GetEquipment () { return _equipmentSlots; }

    public List<T> GetEquipment<T> () where T : EquipmentSlot {

        List<T> equipment = new List<T> ();

        _equipmentSlots.ForEach (slot => {

            if (slot is T) equipment.Add (slot as T);

        });

        return equipment;

    }

    public ItemSOToIntDictionary GetInventory () { return _inventory; }

    public void SetInventory (ItemSOToIntDictionary inventory) { _inventory = inventory ?? new ItemSOToIntDictionary (); }

    public int GetInventoryCount (ItemSO item) { return _inventory.ContainsKey (item) ? _inventory[item] : 0; }

    public void SetInventoryCount (ItemSO item, int count) { _inventory[item] = count; }

    public void ChangeInventoryCount (ItemSO item, int delta) { SetInventoryCount (item, GetInventoryCount (item) + delta); }

    public bool HasInventoryCount (ItemSO item, int condition) { return GetInventoryCount (item) >= condition; }

    public float GetTotalInventorySize () { return _profile.InventorySize; }

    public float GetUsedInventorySize () {

        float used = 0;
        foreach (ItemSO item in _inventory.Keys) used += item.Size * _inventory[item];
        return used;

    }

    public float GetFreeInventorySize () { return GetTotalInventorySize () - GetUsedInventorySize (); }

    public bool CanAddInventoryItem (ItemSO item, int count) { return GetFreeInventorySize () >= item.Size * count; }

    public AI GetAI () { return _ai; }

    public void SetAI (AI controller) { _ai = controller; }

    public Structure GetTarget () { return _target; }

    public void SetTarget (Structure target) {

        _target = target;

        if (PlayerController.GetInstance ().GetPlayer () == this) PlayerController.GetInstance ().TargetChangedChannel.OnEventRaised ();

    }

    public Sector GetSector () { return _sector; }

    public void SetSector (Sector sector) { _sector = sector; }

    public List<StructureLink> GetLinks () { return _links; }

    public void SetLinks (List<StructureLink> links) { _links = links; }

    public void AddLink (StructureLink link) { if (!_links.Contains (link)) _links.Add (link); }

    public void RemoveLink (StructureLink link) { _links.Remove (link); }

    public List<Structure> GetDockedStructures () {

        List<Structure> docked = new List<Structure> ();
        foreach (StructureLink link in _links)
            if (link.GetLinkType () == StructureLinkType.Docking)
                if (link.GetAId () == _id)
                    docked.Add (link.GetB ());

        return docked;

    }

    public int GetDockedStructuresCount () {

        int count = 0;
        foreach (StructureLink link in _links)
            if (link.GetLinkType () == StructureLinkType.Docking)
                if (link.GetAId () == _id)
                    count++;

        return count;

    }

    public bool IsDocked () {

        foreach (StructureLink link in _links)
            if (link.GetLinkType () == StructureLinkType.Docking)
                if (link.GetBId () == _id)
                    return true;

        return false;

    }

    public bool CanReceiveDocker (Structure docker) {

        return false;

    }

    public bool ReceiveDocker (Structure docker) {

        if (!CanReceiveDocker (docker)) return false;

        return false;

    }

    public void Tick () {

        if (_ai == null) _ai = new AI (this);
        _ai.Tick ();

        foreach (EquipmentSlot slot in _equipmentSlots) slot.Tick ();

    }

    public void FixedTick () {

        foreach (EquipmentSlot slot in _equipmentSlots) slot.FixedTick ();

    }

    public void TakeDamage (float amount, Vector3 from) {

        ShieldSlot shield = GetEquipment<ShieldSlot> ()[0];
        float leftOver = amount;
        if (shield != null) {
            int sector = shield.GetStrengths ().GetSectorTo (from);
            float strength = shield.GetStrengths ().GetSectorStrength (sector);
            float shieldDmg = Mathf.Min (amount, strength);
            leftOver -= shieldDmg;
            shield.GetStrengths ().ChangeSectorStrength (sector, -shieldDmg);
        }
        ChangeHull (-leftOver);

    }

    public float GetAngleTo (Vector3 to) {

        Vector3 heading = to - transform.localPosition;
        return Vector3.SignedAngle (transform.forward, heading, transform.up);

    }

    public float GetElevationTo (Vector3 to) {

        Vector3 heading = to - transform.localPosition;
        return Vector3.SignedAngle (transform.forward, heading, -transform.right);

    }

    public StructureSaveData GetSaveData () {

        StructureSaveData data = new StructureSaveData {

            Name = gameObject.name,
            Position = transform.localPosition,
            Rotation = transform.localRotation,
            Hull = _hull

        };
        if (_profile != null) data.ProfileId = _profile.Id;
        if (_faction != null) data.FactionId = _faction.GetId ();
        _equipmentSlots.ForEach (slot => { data.Equipment.Add (slot.GetSaveData ()); });
        if (_sector != null) data.SectorId = _sector.GetId ();
        return data;

    }

    public void SetSaveData (StructureSaveData saveData) {

        gameObject.name = saveData.Name;
        transform.localPosition = saveData.Position;
        transform.localRotation = saveData.Rotation;
        _hull = saveData.Hull;
        _faction = FactionManager.GetInstance ().GetFaction (saveData.FactionId);
        for (int i = 0; i < _equipmentSlots.Count; i++) _equipmentSlots[i].LoadSaveData (saveData.Equipment[i]);
        _sector = SectorManager.GetInstance ().GetSector (saveData.SectorId);
        _sector.Entered (this);

    }

}

[Serializable]
public class StructureSaveData {

    public string Name;
    public Vector3 Position;
    public Quaternion Rotation;
    public string ProfileId;
    public string Id;
    public float Hull;
    public string FactionId;
    public List<EquipmentSlotSaveData> Equipment = new List<EquipmentSlotSaveData> ();
    public string SectorId;

}
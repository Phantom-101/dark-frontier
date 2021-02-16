﻿using UnityEngine;

[CreateAssetMenu (menuName = "Items/Structure")]
public class StructureSO : ItemSO {

    [Header ("Graphics")]
    public GameObject Prefab;
    public float ApparentSize;
    public Sprite HullWireframe;
    public float WorldToUIScale;
    public GameObject DestructionEffect;

    [Header ("Stats")]
    public float Hull;
    public float InventorySize;
    public float SensorStrength;
    public float Detectability;
    public float DockingBaySize;
    public StructureDestroyedEventChannelSO OnDestroyedChannel;
    public float DropPercentage;
    public int MaxEquipmentTier;
    public bool SnapToPlane;

}

using UnityEngine;

[CreateAssetMenu (menuName = "Items/Structure")]
public class StructureSO : ItemSO {
    [Header ("Graphics")]
    public GameObject Prefab;
    public float ApparentSize;
    public Sprite HullWireframe;
    public float WorldToUIScale;
    public bool ShowBlip;
    public GameObject DestructionEffect;

    [Header ("Stats")]
    public float Hull;
    public float InventorySize;
    public float SensorStrength;
    public float ScannerStrength;
    public int MaxLocks;
    public float Detectability;
    public float SignatureSize;
    public float DockingBaySize;
    public float DropPercentage;
    public bool SnapToPlane;
    public StructureStatTypeToStructureStatDictionary Stats;
}

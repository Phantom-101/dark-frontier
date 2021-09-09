using UnityEngine;

[CreateAssetMenu (menuName = "Items/Structure")]
public class StructureSO : ItemPrototype {
    [Header ("Graphics")]
    public GameObject Prefab;
    public float ApparentSize;
    public Sprite HullWireframe;
    public bool ShowBlip;
    public GameObject DestructionEffect;
    public bool SnapToPlane;

    [Header ("Stats")]
    public StatList Stats;
    public DockingBayList DockingBays;
}

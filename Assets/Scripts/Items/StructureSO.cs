using UnityEngine;

[CreateAssetMenu (menuName = "Items/Structure")]
public class StructureSO : ItemSO {

    [Header ("Graphics")]
    public GameObject Prefab;
    public float ApparentSize;
    public Sprite HullWireframe;

    [Header ("Stats")]
    public float Hull;
    public float InventorySize;
    public StructureDestroyedEventChannelSO OnDestroyedChannel;
    public float DropPercentage;
    public int MaxMeta;

}

using UnityEngine;

[CreateAssetMenu (menuName = "Items/Structure")]
public class StructureSO : ItemSO {

    [Header ("Graphics")]
    public GameObject Prefab;

    [Header ("Stats")]
    public StructureDestroyedEventChannelSO OnDestroyedChannel;
    public float DropPercentage;
    public int MaxMeta;

}

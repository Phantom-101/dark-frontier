using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSO : ScriptableObject {

    [Header ("Graphics")]
    public GameObject Prefab;

    [Header ("Stats")]
    public StructureDestroyedEventChannelSO OnDestroyedChannel;
    public double DropPercentage;
    public int MaxMeta;

}

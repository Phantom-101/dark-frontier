using Unity.Mathematics;
using UnityEngine;

public class EngineSlot : EquipmentSlot {

    [SerializeField] protected float3x2 _settings;

    public EngineSO Engine { get { return _equipment as EngineSO; } }
    public float3x2 Settings { get => _settings; set { _settings = new float3x2 (Mathf.Clamp (value.c0.x, -1, 1), Mathf.Clamp (value.c1.x, -1, 1), Mathf.Clamp (value.c0.y, -1, 1), Mathf.Clamp (value.c1.y, -1, 1), Mathf.Clamp (value.c0.z, -1, 1), Mathf.Clamp (value.c1.z, -1, 1)); } }

    public override void ResetValues () {

        base.ResetValues ();

        Settings = new float3x2 ();

    }
    public override bool CanEquip (EquipmentSO equipment) {

        return equipment == null || (base.CanEquip (equipment) && equipment is EngineSO);

    }

    public void SetSetting (int i, int j, float t) {

        float3x2 s = Settings;
        s[i][j] = t;
        Settings = s;

    }

}

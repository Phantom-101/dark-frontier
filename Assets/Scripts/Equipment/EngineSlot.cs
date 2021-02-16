using UnityEngine;

public class EngineSlot : EquipmentSlot {

    [SerializeField] protected float _forwardSetting;
    [SerializeField] protected float _yawSetting;
    [SerializeField] protected float _pitchSetting;

    public EngineSO Engine { get { return _equipment as EngineSO; } }
    public float ForwardSetting { get => _forwardSetting; set { _forwardSetting = Mathf.Clamp (value, -0.5f, 1); } }
    public float YawSetting { get => _yawSetting; set { _yawSetting = Mathf.Clamp (value, -1, 1); } }
    public float PitchSetting { get => _pitchSetting; set { _pitchSetting = Mathf.Clamp (value, -1, 1); } }

    public override void ResetValues () {

        base.ResetValues ();

        ForwardSetting = 0;
        YawSetting = 0;
        PitchSetting = 0;

    }
    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is EngineSO && equipment.Tier <= Equipper.GetProfile ().MaxEquipmentTier);

    }

}

using UnityEngine;

public class EngineSlot : EquipmentSlot {

    [SerializeField] protected float _forwardSetting;
    [SerializeField] protected float _turnSetting;
    [SerializeField] protected float _pitchSetting;

    public EngineSO Engine { get { return _equipment as EngineSO; } }
    public float ForwardSetting { get => _forwardSetting; set { _forwardSetting = Mathf.Clamp01 (value); } }
    public float TurnSetting { get => _turnSetting; set { _turnSetting = Mathf.Clamp (value, -1, 1); } }
    public float PitchSetting { get => _pitchSetting; set { _pitchSetting = Mathf.Clamp (value, -1, 1); } }

    public override void ResetValues () {

        base.ResetValues ();

        ForwardSetting = 0;
        TurnSetting = 0;
        PitchSetting = 0;

    }
    public override bool CanEquip (EquipmentSO equipment) {

        return base.CanEquip (equipment) || (equipment is EngineSO && equipment.Tier <= Equipper.GetProfile ().MaxEquipmentTier);

    }

}

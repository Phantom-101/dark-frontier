using UnityEngine;

[CreateAssetMenu (menuName = "AI/Missile")]
public class MissileAISO : AISO {

    public override AI GetAI (Structure structure) { return new MissileAI (structure); }

}

using UnityEngine;

[CreateAssetMenu (menuName = "AI/Dummy")]
public class AISO : ScriptableObject {

    public virtual AI GetAI (Structure structure) { return new AI (structure); }

}

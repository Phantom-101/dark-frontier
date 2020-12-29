using UnityEngine;

[CreateAssetMenu (menuName = "Controllers/Dummy")]
public class ControllerSO : ScriptableObject {

    public virtual Controller GetController () { return new Controller (); }

}

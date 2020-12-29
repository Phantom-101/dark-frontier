using UnityEngine;

[CreateAssetMenu (menuName = "Controllers/Player")]
public class PlayerControllerSO : ControllerSO {

    public override Controller GetController () { return new PlayerController (); }

}

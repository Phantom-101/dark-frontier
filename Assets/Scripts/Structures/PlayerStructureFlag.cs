using DarkFrontier.Structures;
using UnityEngine;

public class PlayerStructureFlag : MonoBehaviour {
    private void Awake () {
        Structure structure = GetComponent<Structure> ();
        if (structure != null) PlayerController.Instance.Player = structure;
        Destroy (this);
    }
}

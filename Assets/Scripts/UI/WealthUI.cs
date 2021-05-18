using UnityEngine;
using UnityEngine.UI;

public class WealthUI : MonoBehaviour {

    [SerializeField] private Text _text;

    private void Update () {

        if (PlayerController.GetInstance ().GetPlayer () == null) return;
        _text.text = (PlayerController.GetInstance ().GetPlayer ().Faction?.GetWealth ().ToString () ?? "0") + " Cr";

    }

}

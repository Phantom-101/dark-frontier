using UnityEngine;
using UnityEngine.UI;

public class LocationUI : MonoBehaviour {

    [SerializeField] private Text _text;

    private void Update () {

        Structure player = PlayerController.GetInstance ().GetPlayer ();

        _text.text = player.Profile.Name + " \"" + player.gameObject.name + "\" (" + player.Sector.gameObject.name + ")";

    }

}

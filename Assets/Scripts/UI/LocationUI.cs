using UnityEngine;
using UnityEngine.UI;

public class LocationUI : MonoBehaviour {

    [SerializeField] private Text _text;

    private void Update () {

        _text.text = PlayerController.GetInstance ().GetPlayer ().GetSector ().gameObject.name;

    }

}

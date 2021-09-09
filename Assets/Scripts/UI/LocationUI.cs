using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;

public class LocationUI : MonoBehaviour {
    [SerializeField] private Text _text;

    private void Update () {
        Structure player = PlayerController.Instance.Player;
        if (player == null) return;
        _text.text = $"{player.Profile.Name} \"{player.gameObject.name}\" ({player.Sector.Value.gameObject.name})";
    }
}

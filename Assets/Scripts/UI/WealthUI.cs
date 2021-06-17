using UnityEngine;
using UnityEngine.UI;

public class WealthUI : MonoBehaviour {
    [SerializeField] private Text _text;

    private void Update () {
        Structure player = PlayerController.Instance.Player;
        if (player == null) return;
        if (player.Faction == null) _text.text = "0 Cr";
        else _text.text = $"{PlayerController.Instance.Player.Faction.Wealth} Cr";
    }
}

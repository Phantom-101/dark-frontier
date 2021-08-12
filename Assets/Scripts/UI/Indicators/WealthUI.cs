using UnityEngine;
using UnityEngine.UI;

public class WealthUI : MonoBehaviour {
    [SerializeField] private Text _text;

    private void Update () {
        Structure player = PlayerController.Instance.Player;
        if (player == null) return;
        _text.text = $"{PlayerController.Instance.Player.Faction.Value (FactionManager.Instance.GetFaction)?.Wealth ?? 0} Cr";
    }
}

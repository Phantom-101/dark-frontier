using DarkFrontier.Factions;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WealthUI : MonoBehaviour {
    [SerializeField] private Text _text;

    private FactionManager factionManager;

    [Inject]
    public void Construct (FactionManager factionManager) {
        this.factionManager = factionManager;
    }

    private void Update () {
        Structure player = PlayerController.Instance.Player;
        if (player == null) return;
        _text.text = $"{PlayerController.Instance.Player.Faction.Value (factionManager.GetFaction)?.Wealth ?? 0} Cr";
    }
}

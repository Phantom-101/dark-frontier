using DarkFrontier.Factions;
using UnityEngine;
using UnityEngine.UI;

public class FactionInfoUI : MonoBehaviour {
    [SerializeField] private Text _name;
    [SerializeField] private Text _state;
    [SerializeField] private GameObject _relationsBg;
    [SerializeField] private Image _negative;
    [SerializeField] private Image _positive;
    [SerializeField] private Faction _faction;

    public Faction GetFaction () { return _faction; }

    public void SetFaction (Faction faction) { _faction = faction; }

    public void Initialize () {
        _name.text = _faction.Name;
        string playerFaction = PlayerController.Instance.Player.Faction.Id.Value;
        if (playerFaction == _faction.Id) {
            _relationsBg.SetActive (false);
            _state.text = "State: Self";
        } else {
            float relation = _faction.GetRelation (playerFaction);
            if (relation > 0) {
                LeanTween.size (_positive.rectTransform, new Vector2 (500 * (relation / 1), 30), 0.2f).setIgnoreTimeScale (true);
                _state.text = "State: " + (_faction.IsAlly (playerFaction) ? "Allied" : "Friendly");
            } else {
                LeanTween.size (_negative.rectTransform, new Vector2 (500 * (-relation / 1), 30), 0.2f).setIgnoreTimeScale (true);
                _state.text = "State: " + (_faction.IsEnemy (playerFaction) ? "War" : "Cautious");
            }
        }
    }
}

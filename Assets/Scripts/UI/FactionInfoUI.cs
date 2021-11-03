using System;
using DarkFrontier.Controllers;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Services;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI {
    public class FactionInfoUI : MonoBehaviour {
        [SerializeField] private Text _name;
        [SerializeField] private Text _state;
        [SerializeField] private GameObject _relationsBg;
        [SerializeField] private Image _negative;
        [SerializeField] private Image _positive;
        [SerializeField] private Faction _faction;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        
        public Faction GetFaction () { return _faction; }

        public void SetFaction (Faction faction) { _faction = faction; }

        public void Initialize () {
            _name.text = _faction.Name;
            string playerFaction = iPlayerController.Value.UPlayer.UFaction.UId.Value;
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
}

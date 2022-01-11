using System;
using System.Collections.Generic;
using DarkFrontier.Controllers;
using DarkFrontier.Factions;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.UI {
    public class PropertyUI : LogTabUI {
        [SerializeField] private Faction faction;
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<PropertyInfoUI> _instantiated = new List<PropertyInfoUI> ();

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        
        public override void SwitchIn () {
            foreach (PropertyInfoUI info in _instantiated) Destroy (info.gameObject);
            _instantiated = new List<PropertyInfoUI> ();

            faction = iPlayerController.Value.UPlayer.uFaction.UValue;

            if (faction == null) {
                base.SwitchIn ();
                return;
            }

            List<Structure> property = faction.Property.UStructures;
            int index = 0;
            foreach (Structure s in property) {
                GameObject go = Instantiate (_prefab, _content);
                go.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -150 * index);
                PropertyInfoUI info = go.GetComponent<PropertyInfoUI> ();
                info.SetStructure (s);
                info.Initialize ();
                _instantiated.Add (info);
                index++;
            }
            _content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 150 * index);

            base.SwitchIn ();
        }
    }
}

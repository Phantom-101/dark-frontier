using DarkFrontier.Factions;
using DarkFrontier.Structures;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PropertyUI : LogTabUI {
    [SerializeField] private Faction _faction;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private List<PropertyInfoUI> _instantiated = new List<PropertyInfoUI> ();

    private FactionManager factionManager;

    [Inject]
    public void Construct (FactionManager factionManager) {
        this.factionManager = factionManager;
    }

    public override void SwitchIn () {
        foreach (PropertyInfoUI info in _instantiated) Destroy (info.gameObject);
        _instantiated = new List<PropertyInfoUI> ();

        _faction = PlayerController.Instance.Player.Faction.Value (factionManager.GetFaction);

        if (_faction == null) {
            base.SwitchIn ();
            return;
        }

        List<Structure> property = _faction.GetProperty ();
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

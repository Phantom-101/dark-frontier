using System.Collections.Generic;
using UnityEngine;

public class FactionsUI : LogTabUI {

    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private List<FactionInfoUI> _instantiated = new List<FactionInfoUI> ();

    public override void SwitchIn () {

        foreach (FactionInfoUI info in _instantiated) Destroy (info.gameObject);
        _instantiated = new List<FactionInfoUI> ();

        int index = 0;
        foreach (Faction f in FactionManager.Instance.Factions) {

            GameObject go = Instantiate (_prefab, _content);
            go.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -150 * index);
            FactionInfoUI info = go.GetComponent<FactionInfoUI> ();
            info.SetFaction (f);
            info.Initialize ();
            _instantiated.Add (info);
            index++;

        }
        _content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 150 * index);

        base.SwitchIn ();

    }

}

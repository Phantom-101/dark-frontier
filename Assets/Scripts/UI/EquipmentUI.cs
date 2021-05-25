﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentUI : MonoBehaviour {
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _indicatorPrefab;

    private readonly Dictionary<EquipmentSlot, EquipmentIndicatorUI> _indicators = new Dictionary<EquipmentSlot, EquipmentIndicatorUI> ();

    private void Update () {

        Structure player = PlayerController.GetInstance ().GetPlayer ();
        if (player == null) return;

        foreach (EquipmentSlot key in _indicators.Keys.ToArray ()) {
            if (!player.Equipment.Contains (key)) {
                _indicators.Remove (key);
                Destroy (key.gameObject);
            }
        }

        foreach (EquipmentSlot slot in player.Equipment)
            if (slot.Data.Equipment != null && slot.Data.Equipment.ShowUI && !_indicators.ContainsKey (slot))
                _indicators[slot] = null;

        float height = _indicatorPrefab.GetComponent<RectTransform> ().sizeDelta.y;
        foreach (EquipmentSlot key in _indicators.Keys.ToArray ()) {
            if (key.Data.Equipment != null && key.Data.Equipment.ShowUI) {
                if (_indicators[key] == null) {
                    GameObject indicator = Instantiate (_indicatorPrefab, _parent);
                    RectTransform rect = indicator.GetComponent<RectTransform> ();
                    rect.anchoredPosition = new Vector2 (0, height * (player.Equipment.Count - player.Equipment.IndexOf (key) - 1));
                    EquipmentIndicatorUI comp = indicator.GetComponent<EquipmentIndicatorUI> ();
                    comp.Slot = key;
                    _indicators[key] = comp;
                }
            }
        }
    }
}

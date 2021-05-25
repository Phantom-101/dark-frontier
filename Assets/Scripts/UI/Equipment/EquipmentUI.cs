using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentUI : MonoBehaviour {
    [SerializeField] private Transform _parent;

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
            if (slot.Data.Equipment != null && slot.Data.Equipment.ButtonPrefab != null && !_indicators.ContainsKey (slot))
                _indicators[slot] = null;

        float y = 0;
        foreach (EquipmentSlot key in _indicators.Keys.ToArray ()) {
            if (key.Data.Equipment != null && key.Data.Equipment.ButtonPrefab != null) {
                if (_indicators[key] == null) {
                    GameObject indicator = Instantiate (key.Data.Equipment.ButtonPrefab, _parent);
                    RectTransform rect = indicator.GetComponent<RectTransform> ();
                    rect.anchoredPosition = new Vector2 (0, y);
                    y += rect.sizeDelta.y;
                    EquipmentIndicatorUI comp = indicator.GetComponent<EquipmentIndicatorUI> ();
                    comp.Slot = key;
                    comp.Initialize ();
                    _indicators[key] = comp;
                }
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentUI : MonoBehaviour {

    [SerializeField] private Transform _canvas;
    [SerializeField] private GameObject _indicatorPrefab;

    private readonly Dictionary<EquipmentSlot, EquipmentIndicator> _indicators = new Dictionary<EquipmentSlot, EquipmentIndicator> ();

    private void Update () {

        Structure player = PlayerController.GetInstance ().GetPlayer ();

        foreach (EquipmentSlot key in _indicators.Keys.ToArray ())
            if (!player.GetEquipment ().Contains (key)) {

                _indicators.Remove (key);
                Destroy (key.gameObject);

            }

        foreach (EquipmentSlot slot in player.GetEquipment ())
            if (!_indicators.ContainsKey (slot))
                _indicators[slot] = null;

        foreach (EquipmentSlot key in _indicators.Keys.ToArray ())
            if (_indicators[key] == null) {

                GameObject indicator = Instantiate (_indicatorPrefab, _canvas);
                RectTransform rect = indicator.GetComponent<RectTransform> ();
                rect.anchoredPosition = new Vector2 (0, 100 * (player.GetEquipment ().Count - player.GetEquipment ().IndexOf (key) - 1));
                EquipmentIndicator comp = indicator.GetComponent<EquipmentIndicator> ();
                comp.SetSlot (key);
                _indicators[key] = comp;

            }

    }

}

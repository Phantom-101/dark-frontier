using System;
using DarkFrontier.Controllers;
using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Indicators {
    public class PlayerInfoUI : MonoBehaviour {
        [SerializeField] private Transform _hpIndicators;
        [SerializeField] private Image _hull;
        [SerializeField] private Gradient _hullGradient;
        [SerializeField] private Image _shield;
        [SerializeField] private Gradient _shieldGradient;
        [SerializeField] private Transform _direction;
        [SerializeField] private RectTransform _capFill;
        [SerializeField] private Image _capImg;
        [SerializeField] private RectTransform _capOutline;
        [SerializeField] private Gradient _capGradient;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController> ());
    
        private void Update () {
            var lPlayer = iPlayerController.Value.UPlayer;

            if (lPlayer == null) return;

            // Hull wireframe indicator
            _hull.sprite = lPlayer.UPrototype.HullWireframe;
            _hull.color = _hullGradient.Evaluate (lPlayer.UHull / lPlayer.UStats.UValues.MaxHull);

            // Shield bubble indicator
            float lCurStrength = 0, lTotalStrength = 0;
            {
                var lShields = lPlayer.UEquipment.States<ShieldPrototype.State>();
                var lCount = lShields.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lShield = lShields[lIndex];
                    lCurStrength += lShield.Strength;
                    lTotalStrength += ((ShieldPrototype) lShield.Slot.Equipment).MaxStrength;
                }
            }
            _shield.color = _shieldGradient.Evaluate (lCurStrength / (lTotalStrength == 0 ? 1 : lTotalStrength));

            // Velocity indicator
            //Rigidbody rb = _structure.GetComponent<Rigidbody> ();
            //if (rb == null) _velocity.text = "0 m/s";
            //else _velocity.text = rb.velocity.magnitude.ToString ("F2") + " m/s";

            // Capacitor bar indicator
            float lStoredCap = 0, lTotalCap = 0;
            {
                var lCapacitors = lPlayer.UEquipment.States<CapacitorPrototype.State>();
                var lCount = lCapacitors.Count;
                for (var lIndex = 0; lIndex < lCount; lIndex++) {
                    var lCapacitor = lCapacitors[lIndex];
                    lStoredCap += lCapacitor.Charge;
                    lTotalCap += ((CapacitorPrototype) lCapacitor.Slot.Equipment).Capacitance;
                }
            }
            _capFill.sizeDelta = new Vector2 (_capOutline.sizeDelta.x * lStoredCap / (lTotalCap == 0 ? 1 : lTotalCap), _capFill.sizeDelta.y);
            _capImg.color = _capGradient.Evaluate (lStoredCap / (lTotalCap == 0 ? 1 : lTotalCap));

            // Selected direction indicator
            if (lPlayer.USelected.UValue == null) _direction.gameObject.SetActive (false);
            else {
                _direction.gameObject.SetActive (true);
                _direction.rotation = Quaternion.Euler (0, 0, -lPlayer.GetAngleTo (new Location (lPlayer.USelected.UValue.transform)));
            }
        }
    }
}

using System;
using DarkFrontier.Controllers;
using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Indicators {
    public class SelectedInfoUI : MonoBehaviour {
        [SerializeField] private Transform _hpIndicators;
        [SerializeField] private Image _hull;
        [SerializeField] private Gradient _hullGradient;
        [SerializeField] private Image _shield;
        [SerializeField] private Gradient _shieldGradient;
        [SerializeField] private Text _name;
        [SerializeField] private Text _faction;
        [SerializeField] private Text _distance;
        [SerializeField] private Text _velocity;
        [SerializeField] private Transform _direction;
        [SerializeField] private Button _lock;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private float _targetAlpha;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        
        private void Start () {
            _lock.onClick.AddListener (() => iPlayerController.Value.OnLockSelected?.Invoke (this, EventArgs.Empty));
            _targetAlpha = _group.alpha;
        }

        private void Update () {
            Structure selected = iPlayerController.Value.UPlayer;
            if (selected != null) selected = selected.USelected.UValue;

            if (selected == null) {
                TweenTo (0);
                return;
            }

            TweenTo (1);

            _hull.sprite = selected.UPrototype.HullWireframe;
            _hull.color = _hullGradient.Evaluate (selected.UHull / selected.UStats.UValues.MaxHull);

            float lCurStrength = 0, lTotalStrength = 0;
            var lShields = selected.UEquipment.States<ShieldPrototype.State>();
            var lCount = lShields.Count;
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                var lShield = lShields[lIndex];
                lCurStrength += lShield.Strength;
                lTotalStrength += (lShield.Slot.Equipment as ShieldPrototype).MaxStrength;
            }
            _shield.color = _shieldGradient.Evaluate (lCurStrength / (lTotalStrength == 0 ? 1 : lTotalStrength));

            _name.text = selected.gameObject.name;

            _faction.text = selected.UFaction.UValue?.Name ?? "None";

            _distance.text = Vector3.Distance (iPlayerController.Value.UPlayer.transform.position, selected.transform.position).ToString ("F2") + " m";

            Rigidbody rb = selected.GetComponent<Rigidbody> ();
            if (rb == null) _velocity.text = "0 m/s";
            else _velocity.text = rb.velocity.magnitude.ToString ("F2") + " m/s";

            _direction.rotation = Quaternion.Euler (0, 0, -selected.GetAngleTo (new Location (iPlayerController.Value.UPlayer.transform)));
        }

        private void TweenTo (float alpha) {
            if (_targetAlpha != alpha) {
                _targetAlpha = alpha;
                if (alpha == 0) {
                    _group.blocksRaycasts = false;
                    _group.interactable = false;
                } else {
                    _group.blocksRaycasts = true;
                    _group.interactable = true;
                }
                LeanTween.cancel (_group.gameObject);
                LeanTween.alphaCanvas (_group, alpha, 0.2f).setIgnoreTimeScale (true);
            }
        }
    }
}

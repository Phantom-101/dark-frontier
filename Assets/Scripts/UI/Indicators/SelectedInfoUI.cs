using DarkFrontier.Equipment;
using DarkFrontier.Structures;
using System;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start () {
        _lock.onClick.AddListener (() => PlayerController.Instance.OnLockSelected?.Invoke (this, EventArgs.Empty));
        _targetAlpha = _group.alpha;
    }

    private void Update () {
        Structure selected = PlayerController.Instance.Player;
        if (selected != null) selected = selected.Selected;

        if (selected == null) {
            TweenTo (0);
            return;
        }

        TweenTo (1);

        _hull.sprite = selected.Profile.HullWireframe;
        _hull.color = _hullGradient.Evaluate (selected.Hull / selected.Stats.GetBaseValue (StatNames.MaxHull, 1));

        float curStrength = 0, totalStrength = 0;
        selected.GetEquipmentStates<ShieldPrototype.State> ().ForEach (shield => {
            curStrength += shield.Strength;
            totalStrength += (shield.Slot.Equipment as ShieldPrototype).MaxStrength;
        });
        _shield.color = _shieldGradient.Evaluate (curStrength / (totalStrength == 0 ? 1 : totalStrength));

        _name.text = selected.gameObject.name;

        _faction.text = selected.Faction.Value?.Name ?? "None";

        _distance.text = Vector3.Distance (PlayerController.Instance.Player.transform.position, selected.transform.position).ToString ("F2") + " m";

        Rigidbody rb = selected.GetComponent<Rigidbody> ();
        if (rb == null) _velocity.text = "0 m/s";
        else _velocity.text = rb.velocity.magnitude.ToString ("F2") + " m/s";

        _direction.rotation = Quaternion.Euler (0, 0, -selected.GetAngleTo (PlayerController.Instance.Player.transform.position));
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

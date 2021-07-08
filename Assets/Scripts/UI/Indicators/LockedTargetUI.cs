using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockedTargetUI : MonoBehaviour {

    [SerializeField] private Button _button;
    [SerializeField] private Image _hull;
    [SerializeField] private Image _shield;
    [SerializeField] private Image _leftProgress;
    [SerializeField] private Image _rightProgress;
    [SerializeField] private Text _name;
    [SerializeField] private Text _faction;
    [SerializeField] private Text _distance;
    [SerializeField] private Text _velocity;
    [SerializeField] private Transform _direction;

    [SerializeField] protected Structure _structure;

    private PlayerController _pc;

    public Structure Structure { get => _structure; set => _structure = value; }

    private void Start () {

        _pc = PlayerController.Instance;
        _button.onClick.AddListener (() => { _pc.Player.Selected = _structure; });

    }

    private void Update () {

        if (_structure == null) {

            Destroy (gameObject);
            return;

        }

        Structure player = _pc.Player;

        if (!player.Locks.ContainsKey (_structure)) {

            Destroy (gameObject);
            return;

        }

        _hull.fillAmount = _structure.Hull / _structure.Stats.GetBaseValue (StatNames.MaxHull, 1) / 2;
        float strength = 0, maxStrength = 0;
        _structure.GetEquipmentData<ShieldSlotData> ().ForEach (shield => {
            strength += shield.Strength;
            maxStrength += (shield.Equipment as ShieldSO).MaxStrength;
        });
        _shield.fillAmount = strength / (maxStrength == 0 ? 1 : maxStrength) / 2;
        float fa = player.Locks[_structure] / 400;
        _leftProgress.fillAmount = fa;
        _rightProgress.fillAmount = fa;
        _name.text = _structure.gameObject.name;
        _faction.text = _structure.Faction == null ? "" : _structure.Faction.Name;
        _distance.text = Vector3.Distance (PlayerController.Instance.Player.transform.position, _structure.transform.position).ToString ("F0") + " m";
        Rigidbody rb = _structure.GetComponent<Rigidbody> ();
        if (rb == null) _velocity.text = "0 m/s";
        else _velocity.text = rb.velocity.magnitude.ToString ("F0") + " m/s";
        _direction.rotation = Quaternion.Euler (0, 0, -_structure.GetAngleTo (PlayerController.Instance.Player.transform.position));

    }

}

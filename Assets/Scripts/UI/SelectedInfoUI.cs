using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedInfoUI : MonoBehaviour {

    [SerializeField] private Transform _hpIndicators;
    [SerializeField] private Image _hull;
    [SerializeField] private Gradient _hullGradient;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private List<Image> _shields;
    [SerializeField] private Gradient _shieldGradient;
    [SerializeField] private Text _name;
    [SerializeField] private Text _faction;
    [SerializeField] private Text _distance;
    [SerializeField] private Text _velocity;
    [SerializeField] private Transform _direction;
    [SerializeField] private Button _lock;
    [SerializeField] private VoidEventChannelSO _lockSelected;

    [SerializeField] protected Structure _structure;

    private void Start () {

        if (_structure == null) {

            Destroy (gameObject);
            return;

        }

        _lock.onClick.AddListener (_lockSelected.OnEventRaised.Invoke);

        SetupShields ();

    }

    public Structure GetStructure () { return _structure; }

    public void SetStructure (Structure structure) { _structure = structure; }

    private void Update () {

        if (_structure == null) {

            Destroy (gameObject);
            return;

        }

        _hull.sprite = _structure.Profile.HullWireframe;
        _hull.color = _hullGradient.Evaluate (_structure.Hull / _structure.Profile.Hull);

        List<ShieldSlotData> shields = _structure.GetEquipmentData<ShieldSlotData> ();
        for (int i = 0; i < _shields.Count; i++) {
            if (shields[i].Equipment == null) {
                _shields[i].color = Color.clear;
            } else {
                _shields[i].color = _shieldGradient.Evaluate (shields[i].Strength / (shields[i].Equipment as ShieldSO).MaxStrength);
            }
        }

        _name.text = _structure.gameObject.name;
        _faction.text = _structure.Faction?.GetName () ?? "None";
        _distance.text = Vector3.Distance (PlayerController.GetInstance ().GetPlayer ().transform.position, _structure.transform.position).ToString ("F2") + " m";
        Rigidbody rb = _structure.GetComponent<Rigidbody> ();
        if (rb == null) _velocity.text = "0 m/s";
        else _velocity.text = rb.velocity.magnitude.ToString ("F2") + " m/s";
        _direction.rotation = Quaternion.Euler (0, 0, -_structure.GetAngleTo (PlayerController.GetInstance ().GetPlayer ().transform.position));

    }

    private void SetupShields () {
        List<ShieldSlotData> shields = _structure.GetEquipmentData<ShieldSlotData> ();
        foreach (ShieldSlotData shield in shields) {
            GameObject instantiated = Instantiate (_shieldPrefab, _hpIndicators);
            instantiated.transform.SetAsFirstSibling ();
            RectTransform rt = instantiated.GetComponent<RectTransform> ();
            Vector3 scaled = (shield.Slot.transform.position - _structure.transform.position) * _structure.Profile.WorldToUIScale;
            rt.anchoredPosition = new Vector2 (scaled.x, scaled.z);
            rt.sizeDelta = Vector2.one * 100;
            _shields.Add (instantiated.GetComponent<Image> ());
        }
    }

}

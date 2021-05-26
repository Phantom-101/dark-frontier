using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour {
    [SerializeField] private Transform _hpIndicators;
    [SerializeField] private Image _hull;
    [SerializeField] private Gradient _hullGradient;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private List<Image> _shields;
    [SerializeField] private Gradient _shieldGradient;
    [SerializeField] private Transform _direction;
    [SerializeField] private RectTransform _capFill;
    [SerializeField] private Image _capImg;
    [SerializeField] private RectTransform _capOutline;
    [SerializeField] private Gradient _capGradient;
    [SerializeField] private Structure _structure;

    private void Start () {
        _structure = PlayerController.GetInstance ().GetPlayer ();
        if (_structure == null) return;
        SetupShields ();
    }

    public Structure GetStructure () { return _structure; }

    public void SetStructure (Structure structure) { _structure = structure; }

    private void Update () {
        if (_structure != PlayerController.GetInstance ().GetPlayer ()) {
            _structure = PlayerController.GetInstance ().GetPlayer ();
            if (_structure == null) return;
            while (_shields.Count > 0) {
                Destroy (_shields[0].transform.parent.gameObject);
                _shields.RemoveAt (0);
            }
            SetupShields ();
        }

        if (_structure == null) return;

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

        //Rigidbody rb = _structure.GetComponent<Rigidbody> ();
        //if (rb == null) _velocity.text = "0 m/s";
        //else _velocity.text = rb.velocity.magnitude.ToString ("F2") + " m/s";

        float storedCap = 0, totalCap = 0;
        _structure.GetEquipmentData<CapacitorSlotData> ().ForEach (capacitor => {
            storedCap += capacitor.Charge;
            totalCap += (capacitor.Equipment as CapacitorSO).Capacitance;
        });
        _capFill.sizeDelta = new Vector2 (_capOutline.sizeDelta.x * storedCap / (totalCap == 0 ? 1 : totalCap), _capFill.sizeDelta.y);
        _capImg.color = _capGradient.Evaluate (storedCap / (totalCap == 0 ? 1 : totalCap));
        if (_structure.Selected == null) _direction.gameObject.SetActive (false);
        else {
            _direction.gameObject.SetActive (true);
            _direction.rotation = Quaternion.Euler (0, 0, -_structure.GetAngleTo (_structure.Selected.transform.position));
        }
    }

    private void SetupShields () {
        List<ShieldSlotData> shields = _structure.GetEquipmentData<ShieldSlotData> ();
        foreach (ShieldSlotData shield in shields) {
            GameObject instantiated = Instantiate (_shieldPrefab, _hpIndicators);
            instantiated.transform.SetAsFirstSibling ();
            RectTransform rt = instantiated.GetComponent<RectTransform> ();
            Vector3 scaled = (shield.Slot.transform.position - _structure.transform.position) * 100;
            rt.anchoredPosition = new Vector2 (scaled.x, scaled.z);
            rt.sizeDelta = Vector2.one * 100;
            _shields.Add (instantiated.GetComponent<Image> ());
        }
    }
}

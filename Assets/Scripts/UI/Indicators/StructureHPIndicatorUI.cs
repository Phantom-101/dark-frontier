using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureHPIndicatorUI : MonoBehaviour {

    [SerializeField] private Image _hull;
    [SerializeField] private Gradient _hullGradient;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private List<Image> _shields = new List<Image> ();
    [SerializeField] private Gradient _shieldGradient;
    [SerializeField] private Transform _direction;
    [SerializeField] private Structure _structure;
    [SerializeField] private Structure _other;

    private void Start () {

        Initialize ();

    }

    public void Initialize () {

        foreach (Image img in _shields) Destroy (img.gameObject);
        _shields = new List<Image> ();
        /*
        ShieldSlot slot = _structure.GetEquipment<ShieldSlot> ()[0];
        int sectors = slot.GetStrengths ().GetSectorCount ();
        float degrees = slot.GetStrengths ().GetSectorAngle ();

        for (int i = 0; i < sectors; i++) {

            GameObject instantiated = Instantiate (_shieldPrefab, _hull.transform);
            instantiated.GetComponent<RectTransform> ().Rotate (new Vector3 (0, 0, -degrees * i));
            _shields.Add (instantiated.transform.GetChild (0).GetComponent<Image> ());

        }
        */

    }

    public Structure GetStructure () { return _structure; }

    public void SetStructure (Structure structure) { _structure = structure; }

    public Structure GetOther () { return _other; }

    public void SetOther (Structure other) { _other = other; }

    private void Update () {

        _hull.sprite = _structure.Profile.HullWireframe;
        _hull.color = _hullGradient.Evaluate (_structure.Hull / _structure.Profile.Hull);
        /*
        ShieldStrengths strengths = _structure.GetEquipment<ShieldSlot> ()[0].GetStrengths ();
        if (strengths.GetSectorCount () != _shields.Count) Initialize ();
        for (int i = 0; i < _shields.Count; i++) {

            _shields[i].color = _shieldGradient.Evaluate (strengths.GetSectorStrength (i) / strengths.GetSectorMaxStrength (i));

        }
        ShieldSlot slot = _structure.GetEquipment<ShieldSlot> ()[0];
        float degrees = slot.GetStrengths ().GetSectorAngle ();
        if (_direction != null) {

            if (_other == null) _direction.gameObject.SetActive (false);
            else {

                _direction.gameObject.SetActive (true);
                _direction.rotation = Quaternion.Euler (0, 0, slot.GetStrengths ().GetSectorTo (_structure.GetTarget ().gameObject) * -degrees);

            }

        }
        */

    }

}

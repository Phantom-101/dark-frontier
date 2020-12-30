using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureInfoPanel : MonoBehaviour {

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

    [SerializeField] protected Structure _structure;

    private void Start () {

        ShieldSlot slot = _structure.GetEquipment<ShieldSlot> ()[0];
        int sectors = slot.GetStrengths ().GetSectorCount ();
        float degrees = slot.GetStrengths ().GetSectorAngle ();

        for (int i = 0; i < sectors; i++) {

            GameObject instantiated = Instantiate (_shieldPrefab, _hull.transform);
            instantiated.GetComponent<RectTransform> ().Rotate (new Vector3 (0, 0, -degrees * i));
            _shields.Add (instantiated.transform.GetChild (0).GetComponent<Image> ());

        }

    }

    public Structure GetStructure () { return _structure; }

    public void SetStructure (Structure structure) { _structure = structure; }

    private void Update () {

        if (_structure == null) Destroy (this);

        _hull.sprite = _structure.GetProfile ()?.HullWireframe;
        _hull.color = _hullGradient.Evaluate (_structure.GetHull () / _structure.GetProfile ().Hull);
        ShieldStrengths strengths = _structure.GetEquipment<ShieldSlot> ()[0].GetStrengths ();
        for (int i = 0; i < _shields.Count; i++) {

            _shields[i].color = _shieldGradient.Evaluate (strengths.GetSectorStrength (i) / strengths.GetSectorMaxStrength (i));

        }
        _name.text = _structure.gameObject.name;
        _faction.text = _structure.GetFaction ()?.GetName () ?? "None";
        _distance.text = Vector3.Distance (PlayerController.GetInstance ().GetPlayer ().transform.position, _structure.transform.position).ToString ();
        Rigidbody rb = _structure.GetComponent<Rigidbody> ();
        if (rb == null) _velocity.text = "0 m/s";
        else _velocity.text = rb.velocity.magnitude + " m/s";
        ShieldSlot slot = _structure.GetEquipment<ShieldSlot> ()[0];
        float degrees = slot.GetStrengths ().GetSectorAngle ();
        _direction.rotation = Quaternion.Euler (0, 0, slot.GetStrengths ().GetSectorTo (PlayerController.GetInstance ().GetPlayer ().gameObject) * -degrees);

    }

}

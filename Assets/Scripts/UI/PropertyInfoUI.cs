using UnityEngine;
using UnityEngine.UI;

public class PropertyInfoUI : MonoBehaviour {

    [SerializeField] private Text _name;
    [SerializeField] private Text _location;
    [SerializeField] private StructureHPIndicatorUI _hp;
    [SerializeField] private Structure _structure;

    public Structure GetStructure () { return _structure; }

    public void SetStructure (Structure structure) { _structure = structure; }

    public void Initialize () {

        _name.text = _structure.gameObject.name;
        _location.text = _structure.Sector.Value (SectorManager.Instance.GetSector).gameObject.name;
        _hp.SetStructure (_structure);
        _hp.Initialize ();

    }

}

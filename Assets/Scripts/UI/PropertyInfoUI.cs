using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PropertyInfoUI : MonoBehaviour {

    [SerializeField] private Text _name;
    [SerializeField] private Text _location;
    [SerializeField] private StructureHPIndicatorUI _hp;
    [SerializeField] private Structure _structure;

    private SectorManager sectorManager;

    [Inject]
    public void Construct (SectorManager sectorManager) {
        this.sectorManager = sectorManager;
    }

    public Structure GetStructure () { return _structure; }

    public void SetStructure (Structure structure) { _structure = structure; }

    public void Initialize () {

        _name.text = _structure.gameObject.name;
        _location.text = _structure.Sector.Value (sectorManager.Registry.Find).gameObject.name;
        _hp.SetStructure (_structure);
        _hp.Initialize ();

    }

}

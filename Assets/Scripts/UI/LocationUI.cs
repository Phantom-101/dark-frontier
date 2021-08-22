using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LocationUI : MonoBehaviour {

    [SerializeField] private Text _text;

    private SectorManager sectorManager;

    [Inject]
    public void Construct (SectorManager sectorManager) {
        this.sectorManager = sectorManager;
    }

    private void Update () {

        Structure player = PlayerController.Instance.Player;
        if (player == null) return;
        _text.text = player.Profile.Name + " \"" + player.gameObject.name + "\" (" + player.Sector.Value (sectorManager.Registry.Find).gameObject.name + ")";

    }

}

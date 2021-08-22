using DarkFrontier.Locations;
using DarkFrontier.Structures;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BlipManagerUI : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    private readonly Dictionary<Structure, BlipUI> blips = new Dictionary<Structure, BlipUI> ();

    private SectorManager sectorManager;

    [Inject]
    public void Construct (SectorManager sectorManager) {
        this.sectorManager = sectorManager;
    }

    private void Update () {
        Structure player = PlayerController.Instance.Player;
        if (player == null) return;
        foreach (Structure s in player.Sector.Value (sectorManager.Registry.Find).Population) {
            if (s != player && s.Profile.ShowBlip) {
                if (!blips.ContainsKey (s) || blips[s] == null) {
                    GameObject go = Instantiate (prefab, transform);
                    BlipUI ei = go.GetComponent<BlipUI> ();
                    ei.Target = s;
                    blips[s] = ei;
                }
            }
        }
    }
}

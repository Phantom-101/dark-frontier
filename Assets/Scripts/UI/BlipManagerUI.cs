using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlipManagerUI : MonoBehaviour {

    [SerializeField] private GameObject _prefab;
    private Dictionary<Structure, BlipUI> _inst = new Dictionary<Structure, BlipUI> ();

    private void Update () {

        Structure player = PlayerController.GetInstance ().GetPlayer ();
        foreach (Structure s in player.Sector.InSector) {

            if (s != player && s.Profile.ShowBlip) {

                if (!_inst.ContainsKey (s) || _inst[s] == null) {

                    GameObject go = Instantiate (_prefab, transform);
                    BlipUI ei = go.GetComponent<BlipUI> ();
                    ei.Target = s;
                    _inst[s] = ei;

                }

            }

        }

    }

}

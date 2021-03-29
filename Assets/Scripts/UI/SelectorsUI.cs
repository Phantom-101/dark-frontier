using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectorsUI : MonoBehaviour {

    [SerializeField] private GameObject _selector;

    Dictionary<Structure, SelectorUI> _instantiated = new Dictionary<Structure, SelectorUI> ();

    PlayerController _pc;

    private void Start () {

        _pc = PlayerController.GetInstance ();

    }

    private void Update () {

        Structure player = _pc.GetPlayer ();
        if (player == null) return;

        foreach (Structure key in _instantiated.Keys.ToArray ())
            if (key == null || _instantiated[key] == null)
                _instantiated.Remove (key);

        foreach (Structure detected in player.Detected)
            if (detected != null && !_instantiated.ContainsKey (detected)) {

                SelectorUI script = Instantiate (_selector, transform).GetComponent<SelectorUI> ();
                _instantiated[detected] = script;
                script.Structure = detected;

            }

    }

}


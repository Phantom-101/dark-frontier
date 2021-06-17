using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsUI : MonoBehaviour {
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _infoPanelPrefab;

    private List<LockedTargetUI> _instantiated = new List<LockedTargetUI> ();
    private PlayerController _pc;

    private void Start () {
        _pc = PlayerController.Instance;
        _pc.LocksChanged += Rebuild;
    }

    private void Rebuild (object sender, EventArgs args) {
        while (_instantiated.Count > 0) {
            if (_instantiated[0] != null) Destroy (_instantiated[0].gameObject);
            _instantiated.RemoveAt (0);
        }

        Structure player = _pc.Player;
        if (player == null) return;
        int i = 0;
        foreach (Structure target in player.Locks.Keys.ToArray ()) {
            GameObject go = Instantiate (_infoPanelPrefab, _transform);
            go.transform.localPosition = new Vector3 (-i * 150, 0, 0);
            LockedTargetUI ltui = go.GetComponent<LockedTargetUI> ();
            ltui.Structure = target;
            _instantiated.Add (ltui);
            i++;
        }
    }
}

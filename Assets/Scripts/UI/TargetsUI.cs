using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsUI : MonoBehaviour {

    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _infoPanelPrefab;

    private List<LockedTargetUI> _instantiated = new List<LockedTargetUI> ();
    private PlayerController _pc;

    private void Start () {

        _pc = PlayerController.GetInstance ();
        _pc.LocksChangedChannel.OnEventRaised += Rebuild;

    }

    private void Rebuild () {

        while (_instantiated.Count > 0) {

            Destroy (_instantiated[0].gameObject);
            _instantiated.RemoveAt (0);

        }

        Structure player = _pc.GetPlayer ();
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

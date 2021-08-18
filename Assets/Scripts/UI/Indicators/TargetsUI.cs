using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsUI : MonoBehaviour {
    [SerializeField] private Transform _transform;
    [SerializeField] private GameObject _infoPanelPrefab;

    private readonly List<LockedTargetUI> comps = new List<LockedTargetUI> ();
    private PlayerController playerController;

    private void Awake () {
        playerController = PlayerController.Instance;
    }

    private void OnEnable () {
        playerController.OnLocksChanged += Rebuild;
    }

    private void OnDisable () {
        if (playerController != null) playerController.OnLocksChanged -= Rebuild;
    }

    private void Rebuild (object sender, EventArgs args) {
        while (comps.Count > 0) {
            if (comps[0] != null) Destroy (comps[0].gameObject);
            comps.RemoveAt (0);
        }

        Structure player = playerController.Player;
        if (player == null) return;
        int i = 0;
        foreach (Structure target in player.Locks.Keys.ToArray ()) {
            GameObject instantiated = Instantiate (_infoPanelPrefab, _transform);
            instantiated.transform.localPosition = new Vector3 (-i * 150, 0, 0);
            LockedTargetUI comp = instantiated.GetComponent<LockedTargetUI> ();
            comp.Structure = target;
            comps.Add (comp);
            i++;
        }
    }
}

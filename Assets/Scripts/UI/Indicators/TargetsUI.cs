using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsUI : ComponentBehavior {
    [SerializeField] private Transform root;
    [SerializeField] private GameObject prefab;

    private readonly List<LockedTargetUI> comps = new List<LockedTargetUI> ();
    private PlayerController playerController;

    protected override void SingleInitialize () {
        playerController = PlayerController.Instance;
    }

    protected override void InternalSubscribeEventListeners () {
        if (playerController != null) playerController.OnLocksChanged += Rebuild;
    }

    protected override void InternalUnsubscribeEventListeners () {
        if (playerController != null) playerController.OnLocksChanged -= Rebuild;
    }

    private void Rebuild (object sender, EventArgs args) {
        while (comps.Count > 0) {
            if (comps[0] != null) Destroy (comps[0].gameObject);
            comps.RemoveAt (0);
        }

        Structure player = playerController.Player;
        if (player == null) return;
        foreach (Structure target in player.Locks.Keys.ToArray ()) {
            LockedTargetUI instantiated = Instantiate (prefab, root).GetComponent<LockedTargetUI> ();
            instantiated.Structure = target;
            comps.Add (instantiated);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.UI.Indicators {
    public class TargetsUI : ComponentBehavior {
        [SerializeField] private Transform root;
        [SerializeField] private GameObject prefab;

        private readonly List<LockedTargetUI> comps = new List<LockedTargetUI> ();

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);

        public override void Enable () {
            iPlayerController.Value.OnLocksChanged += Rebuild;
        }

        public override void Disable () {
            iPlayerController.Value.OnLocksChanged -= Rebuild;
        }

        private void Rebuild (object sender, EventArgs args) {
            while (comps.Count > 0) {
                if (comps[0] != null) Destroy (comps[0].gameObject);
                comps.RemoveAt (0);
            }

            Structure player = iPlayerController.Value.UPlayer;
            if (player == null) return;
            foreach (StructureGetter lGetter in player.ULocks.Keys.ToArray ()) {
                LockedTargetUI instantiated = Instantiate (prefab, root).GetComponent<LockedTargetUI> ();
                instantiated.Structure = lGetter.UValue;
                comps.Add (instantiated);
            }
        }
    }
}

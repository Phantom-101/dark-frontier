using System;
using System.Collections.Generic;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.UI.Indicators {
    public class BlipManagerUI : ComponentBehavior {
        [SerializeField] private GameObject prefab;
        private readonly Dictionary<Structure, BlipUI> blips = new Dictionary<Structure, BlipUI> ();

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        private readonly Lazy<BehaviorTimekeeper> iBehaviorTimekeeper = new Lazy<BehaviorTimekeeper>(() => Singletons.Get<BehaviorTimekeeper>(), false);
        
        public override void Enable () {
            iBehaviorTimekeeper.Value.Subscribe (this);
        }

        public override void Disable () {
            iBehaviorTimekeeper.Value.Unsubscribe (this);
        }

        public override void Tick (object aTicker, float aDt) {
            Structure player = iPlayerController.Value.UPlayer;
            if (player == null) return;
            foreach (Structure s in player.uSector.UValue.UPopulation.UStructures) {
                if (s != player && s.uPrototype.ShowBlip) {
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
}

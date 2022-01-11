using System;
using System.Collections.Generic;
using System.Linq;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.UI.Indicators {
    public class SelectorsUI : ComponentBehavior {
        [SerializeField] private GameObject _selector;

        private readonly Dictionary<Structure, SelectorUI> iInstantiated = new Dictionary<Structure, SelectorUI> ();

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
    
        public override void Enable () {
            Singletons.Get<BehaviorTimekeeper> ().Subscribe (this);
        }

        public override void Disable () {
            Singletons.Get<BehaviorTimekeeper> ().Unsubscribe (this);
        }

        public override void Tick (object aObject, float aDt) {
            var lPlayer = iPlayerController.Value.UPlayer;
            if (lPlayer == null) return;

            foreach (var lKey in iInstantiated.Keys.ToArray()) {
                if (lKey == null || iInstantiated[lKey] == null) {
                    iInstantiated.Remove(lKey);
                }
            }

            foreach (var lOther in lPlayer.uSector.UValue.UPopulation.UStructures) {
                if (lPlayer == lOther) continue;
                if (!lPlayer.CanDetect(lOther)) continue;
                if (iInstantiated.ContainsKey(lOther)) continue;
                var lScript = Instantiate (_selector, transform).GetComponent<SelectorUI> ();
                iInstantiated[lOther] = lScript;
                lScript.Structure = lOther;
            }
        }
    }
}


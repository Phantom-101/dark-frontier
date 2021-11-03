using System;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI {
    public class LocationUI : ComponentBehavior {
        [SerializeField] private Text _text;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);

        public override void Enable () {
            Singletons.Get<BehaviorTimekeeper> ().Subscribe (this);
        }

        public override void Disable () {
            Singletons.Get<BehaviorTimekeeper> ().Unsubscribe (this);
        }

        public override void Tick (object aTicker, float aDt) {
            var player = iPlayerController.Value.UPlayer;
            if (player == null) return;
            _text.text = $"{player.UPrototype.Name} \"{player.gameObject.name}\" ({player.USector.UValue.gameObject.name})";
        }
    }
}

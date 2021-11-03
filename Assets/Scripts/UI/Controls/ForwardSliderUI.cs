using System;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Controls {
    public class ForwardSliderUI : ComponentBehavior {
        [SerializeField] private Slider _slider;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        private readonly Lazy<BehaviorTimekeeper> iBehaviorTimekeeper = new Lazy<BehaviorTimekeeper>(() => Singletons.Get<BehaviorTimekeeper>(), false);
    
        public override void Enable() {
            iBehaviorTimekeeper.Value.Subscribe(this);
        }

        public override void Disable() {
            iBehaviorTimekeeper.Value.Unsubscribe(this);
        }

        public override void Tick(object aTicker, float aDt) {
            iPlayerController.Value.SetFwd (_slider.value);
        }
    }
}
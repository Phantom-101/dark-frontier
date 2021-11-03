using DarkFrontier.UI.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarkFrontier.UI.Controls {
    public class TurnButtonUI : UIStateView {
        [SerializeField] private Image _image;
        [SerializeField] private EventTrigger _eventTrigger;

        protected override void OnStateChanged () {
            if (IsShown) {
                _image.raycastTarget = true;
                _eventTrigger.enabled = true;
            } else {
                _image.raycastTarget = false;
                _eventTrigger.enabled = false;
            }
        }
    }
}

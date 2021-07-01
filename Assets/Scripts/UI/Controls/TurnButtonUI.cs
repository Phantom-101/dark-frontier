using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnButtonUI : UIStateView {
    [SerializeField] private Image _image;
    [SerializeField] private EventTrigger _eventTrigger;

    protected override void StateChanged () {
        if (IsShown) {
            _image.raycastTarget = true;
            _eventTrigger.enabled = true;
        } else {
            _image.raycastTarget = false;
            _eventTrigger.enabled = false;
        }
    }
}

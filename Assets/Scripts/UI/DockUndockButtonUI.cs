using UnityEngine;
using UnityEngine.UI;

public class DockUndockButtonUI : MonoBehaviour {

    [SerializeField] private Image _dock;
    [SerializeField] private Image _undock;
    [SerializeField] private Button _button;

    private PlayerController _pc;

    private void Start () {

        _pc = PlayerController.GetInstance ();

        _button.onClick.AddListener (Toggle);

    }

    private void Update () {

        if (_pc.GetPlayer () == null) return;
        bool i = _pc.GetPlayer ().CanDockTarget () || _pc.GetPlayer ().CanUndock ();
        _button.interactable = i;

        if (_pc.GetPlayer ().CanDockTarget ()) {
            _dock.enabled = true;
            _undock.enabled = false;
        } else {
            _dock.enabled = false;
            _undock.enabled = true;
        }

    }

    void Toggle () {

        if (_pc.GetPlayer ().IsDocked ()) Undock ();
        else Dock ();

    }

    void Dock () {

        _pc.GetPlayer ().DockTarget ();

    }

    void Undock () {

        _pc.GetPlayer ().Undock ();

    }

}

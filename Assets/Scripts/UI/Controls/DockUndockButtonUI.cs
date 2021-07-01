using UnityEngine;
using UnityEngine.UI;

public class DockUndockButtonUI : MonoBehaviour {

    [SerializeField] private Image _dock;
    [SerializeField] private Image _undock;
    [SerializeField] private Button _button;

    private PlayerController _pc;

    private void Start () {

        _pc = PlayerController.Instance;

        _button.onClick.AddListener (Toggle);

    }

    private void Update () {

        if (_pc.Player == null) return;
        bool i = _pc.Player.CanDockTarget () || _pc.Player.CanUndock ();
        _button.interactable = i;

        if (_pc.Player.CanDockTarget ()) {
            _dock.enabled = true;
            _undock.enabled = false;
        } else {
            _dock.enabled = false;
            _undock.enabled = true;
        }

    }

    void Toggle () {

        if (_pc.Player.IsDocked ()) Undock ();
        else Dock ();

    }

    void Dock () {

        _pc.Player.DockTarget ();

    }

    void Undock () {

        _pc.Player.Undock ();

    }

}

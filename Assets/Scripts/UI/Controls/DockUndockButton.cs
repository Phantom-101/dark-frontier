using UnityEngine;
using UnityEngine.UI;

public class DockUndockButton : MonoBehaviour {
    [SerializeField] private Image _dock;
    [SerializeField] private Image _undock;
    [SerializeField] private Button _button;

    private PlayerController playerController;

    private void Start () {
        playerController = PlayerController.Instance;
        _button.onClick.AddListener (Toggle);
    }

    private void Update () {
        Structure player = playerController.Player;
        if (player == null) return;

        _button.interactable = (player.Selected != null && player.Selected.DockingBays.CanAccept (player)) || (player.IsDocked && player.Dockee.DockingBays.CanRelease (player));

        if (player.Selected != null && player.Selected.DockingBays.CanAccept (player)) {
            _dock.enabled = true;
            _undock.enabled = false;
        } else {
            _dock.enabled = false;
            _undock.enabled = true;
        }
    }

    void Toggle () {
        if (playerController.Player.IsDocked) Undock ();
        else Dock ();
    }

    void Dock () {
        playerController.Player.Selected.DockingBays.TryAccept (playerController.Player);
    }

    void Undock () {
        playerController.Player.Dockee.DockingBays.TryRelease (playerController.Player);
    }
}

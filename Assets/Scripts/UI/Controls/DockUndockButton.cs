using System;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Controls {
    public class DockUndockButton : MonoBehaviour {
        [SerializeField] private Image _dock;
        [SerializeField] private Image _undock;
        [SerializeField] private Button _button;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);

        private void Start () {
            _button.onClick.AddListener (Toggle);
        }

        private void Update () {
            Structure player = iPlayerController.Value.UPlayer;
            if (player == null) return;

            _button.interactable = (player.USelected.UValue != null && player.USelected.UValue.CanAccept (player)) || (player.UIsDocked && player.UDockedAt.UValue.CanRelease (player));

            if (player.USelected.UValue != null && player.USelected.UValue.CanAccept (player)) {
                _dock.enabled = true;
                _undock.enabled = false;
            } else {
                _dock.enabled = false;
                _undock.enabled = true;
            }
        }

        void Toggle () {
            if (iPlayerController.Value.UPlayer.UIsDocked) Undock ();
            else Dock ();
        }

        void Dock () {
            iPlayerController.Value.UPlayer.USelected.UValue.TryAccept (iPlayerController.Value.UPlayer);
        }

        void Undock () {
            iPlayerController.Value.UPlayer.UDockedAt.UValue.TryRelease (iPlayerController.Value.UPlayer);
        }
    }
}

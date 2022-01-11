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

            _button.interactable = (player.uSelected.UValue != null && player.uSelected.UValue.CanAccept (player)) || (player.uIsDocked && player.uDockedAt.UValue.CanRelease (player));

            if (player.uSelected.UValue != null && player.uSelected.UValue.CanAccept (player)) {
                _dock.enabled = true;
                _undock.enabled = false;
            } else {
                _dock.enabled = false;
                _undock.enabled = true;
            }
        }

        void Toggle () {
            if (iPlayerController.Value.UPlayer.uIsDocked) Undock ();
            else Dock ();
        }

        void Dock () {
            iPlayerController.Value.UPlayer.uSelected.UValue.TryAccept (iPlayerController.Value.UPlayer);
        }

        void Undock () {
            iPlayerController.Value.UPlayer.uDockedAt.UValue.TryRelease (iPlayerController.Value.UPlayer);
        }
    }
}

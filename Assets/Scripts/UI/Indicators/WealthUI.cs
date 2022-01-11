using System;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Services;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Indicators {
    public class WealthUI : MonoBehaviour {
        [SerializeField] private Text _text;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        
        private void Update () {
            var lPlayer = iPlayerController.Value.UPlayer;
            if (lPlayer == null) return;
            _text.text = $"{(iPlayerController.Value.UPlayer.uFaction.UValue?.Wealth ?? 0).ToString()} Cr";
        }
    }
}

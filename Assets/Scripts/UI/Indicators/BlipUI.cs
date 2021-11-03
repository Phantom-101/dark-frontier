using System;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Indicators {
    public class BlipUI : MonoBehaviour {
        [SerializeField] private Structure target;
        [SerializeField] private Image _img;
        [SerializeField] private Sprite _big;
        [SerializeField] private Sprite _small;
        [SerializeField] private Color _own;
        [SerializeField] private Gradient _relations;

        public Structure Target { get => target; set => target = value; }

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        
        private void Update () {
            if (target == null) {
                Destroy (gameObject);
                return;
            }

            Structure player = iPlayerController.Value.UPlayer;
            string pf = player.UFaction.UId.Value;

            if (target == player) {
                Destroy (gameObject);
                return;
            }
            if (target.USector.UValue != player.USector.UValue) {
                Destroy (gameObject);
                return;
            }

            if (target.UPrototype.ApparentSize >= player.UPrototype.ApparentSize) _img.sprite = _big;
            else _img.sprite = _small;

            if (target.UFaction.UId.Value == pf) _img.color = _own;
            else {
                float r = player.UFaction.UValue?.GetRelation (target.UFaction.UId.Value) ?? 0;
                _img.color = _relations.Evaluate ((r + 1) / 2);
            }

            transform.localEulerAngles = new Vector3 (0, 0, -player.GetAngleTo (new Location (target.transform)));
        }
    }
}

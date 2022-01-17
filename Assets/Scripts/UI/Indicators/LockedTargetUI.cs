using System;
using System.Linq;
using DarkFrontier.Controllers;
using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Indicators {
    public class LockedTargetUI : ComponentBehavior {
        public Structure Structure;

        [SerializeField] protected Button button;
        [SerializeField] protected Image hull;
        [SerializeField] protected Image shield;
        [SerializeField] protected Image leftLock;
        [SerializeField] protected Image rightLock;
        [SerializeField] protected Text nameText;
        [SerializeField] protected Text factionText;
        [SerializeField] protected Text distanceText;
        [SerializeField] protected Text velocityText;
        [SerializeField] protected Transform direction;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
        
        public override void Enable () {
            Singletons.Get<BehaviorTimekeeper> ().Subscribe (this);
            button.onClick.AddListener (() => { iPlayerController.Value.UPlayer.uSelected.UId.Value = Structure.uId; });
        }

        public override void Disable () {
            Singletons.Get<BehaviorTimekeeper> ().Unsubscribe (this);
        }

        public override void Tick (object aTicker, float aDt) {
            if (Structure == null || !Structure.Enabled) {
                Destroy (gameObject);
                return;
            }

            Structure player = iPlayerController.Value.UPlayer;

            if (!player.uLocks.Keys.Any (lGetter => lGetter.UId.Value == Structure.uId)) {
                Destroy (gameObject);
                return;
            }

            hull.fillAmount = Structure.uHull / Structure.uStats.UValues.MaxHull / 2;
            float lStrength = 0, iMaxStrength = 0;
            var lShields = Structure.uEquipment.States<ShieldPrototype.State>();
            var lCount = lShields.Count;
            for (var lIndex = 0; lIndex < lCount; lIndex++) {
                var lShield = lShields[lIndex];
                lStrength += lShield.Strength;
                iMaxStrength += (lShield.Slot.Equipment as ShieldPrototype).MaxStrength;
            }
            shield.fillAmount = lStrength / (iMaxStrength == 0 ? 1 : iMaxStrength) / 2;
            float lFillAmount = player.uLocks.First (aPair => aPair.Key.UId.Value == Structure.uId).Value / 4;
            leftLock.fillAmount = lFillAmount;
            rightLock.fillAmount = lFillAmount;
            nameText.text = Structure.gameObject.name;
            factionText.text = Structure.uFaction.UValue?.Name ?? "None";
            distanceText.text = Vector3.Distance (iPlayerController.Value.UPlayer.transform.position, Structure.transform.position).ToString ("F0") + " m";
            Rigidbody rb = Structure.GetComponent<Rigidbody> ();
            if (rb == null) velocityText.text = "0 m/s";
            else velocityText.text = rb.velocity.magnitude.ToString ("F0") + " m/s";
            direction.rotation = Quaternion.Euler (0, 0, -Structure.GetAngleTo (new Location (iPlayerController.Value.UPlayer.transform)));
        }
    }
}

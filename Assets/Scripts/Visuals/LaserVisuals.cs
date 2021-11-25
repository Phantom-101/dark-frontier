using System;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Visuals {
    public class LaserVisuals : ComponentBehavior {
        public LineRenderer? uRenderer;
        public Location? uFrom;
        public Location? uTo;
        public Material? uMaterial;
        public AnimationCurve uWidth = AnimationCurve.Constant(0, 1, 1);
        public AnimationCurve uAlpha = AnimationCurve.Constant(0, 1, 1);
        public float uPeriod;
        public bool uRepeat;

        private float iCounter;

        private bool iEnabled;
        
        private readonly Lazy<BehaviorTimekeeper> iTimekeeper = new Lazy<BehaviorTimekeeper>(() => Singletons.Get<BehaviorTimekeeper>(), false);

        public override void Initialize() {
            oBehaviorManager.DequeueEnable(this);
        }

        public override void Enable() {
            if (uRenderer == null) return;
            uRenderer.enabled = true;
            if (uMaterial != null) {
                uRenderer.material = uMaterial;
            }
            iCounter = 0;

            iTimekeeper.Value.Subscribe(this);
            
            iEnabled = true;
        }

        public override void Disable() {
            if (uRenderer == null) return;
            uRenderer.enabled = false;

            iTimekeeper.Value.Unsubscribe(this);
            
            iEnabled = false;
        }

        public override void Tick(object aTicker, float aDt) {
            if (iEnabled) {
                iCounter += aDt;
                if (iCounter > uPeriod) {
                    if (uRepeat) {
                        iCounter %= uPeriod;
                    } else {
                        iCounter = uPeriod;
                        Disable();
                    }
                }
                if (uRenderer != null) {
                    var lProgress = iCounter / uPeriod;
                    
                    var lWidth = uWidth.Evaluate(lProgress);
                    uRenderer.startWidth = uRenderer.endWidth = lWidth;
                    
                    var lAlpha = uAlpha.Evaluate(lProgress);
                    uRenderer.startColor = uRenderer.endColor = new Color(1, 1, 1, lAlpha);
                    
                    if (uFrom != null) {
                        uRenderer.SetPosition(0, uFrom.UPosition);
                    }

                    if (uTo != null) {
                        uRenderer.SetPosition(1, uTo.UPosition);
                    }
                }
            }
        }
    }
}
#nullable restore
using System;
using System.Collections.Generic;
using DarkFrontier.Equipment;
using DarkFrontier.Foundation.Behaviors;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using DarkFrontier.UI.Controls;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarkFrontier.Controllers {
    [Serializable]
    public class PlayerController : ComponentBehavior {
        public Structure UPlayer { get => iPlayer; set => iPlayer = value; }
        [SerializeField] private Structure iPlayer;

        public EventHandler OnSelectedChanged;
        public EventHandler OnLockSelected;
        public EventHandler OnLocksChanged;

        private GraphicRaycaster iGraphicRaycaster;
        private EventSystem iEventSystem;

        private ReverseButton iReverseButtonUI;

        private readonly Lazy<BehaviorTimekeeper> iTimekeeper = new Lazy<BehaviorTimekeeper>(() => Singletons.Get<BehaviorTimekeeper> ());
    
        public override void Initialize() {
            iGraphicRaycaster = FindObjectOfType<GraphicRaycaster> ();
            iEventSystem = EventSystem.current;

            iReverseButtonUI = ReverseButton.Instance;
        }

        public override void Enable () {
            FireAllButton.Instance.OnClicked += FireAll;
            OnLockSelected += LockSelected;
        
            iTimekeeper.Value.Subscribe (this);
        }

        public override void Disable() {
            if (FireAllButton.Instance != null) FireAllButton.Instance.OnClicked -= FireAll;
            OnLockSelected -= LockSelected;
        
            iTimekeeper.Value.Unsubscribe (this);
        }

        public override void Tick (object aTicker, float aDt) {
            if (UPlayer == null) return;

            if (UnityEngine.Input.GetMouseButtonDown (0) && !ClickingUI ()) {
                var lRay = UnityEngine.Camera.main.ScreenPointToRay (UnityEngine.Input.mousePosition);
                if (Physics.Raycast (lRay, out var lHit)) {
                    if (lHit.collider.transform.parent != null) {
                        var lGameObject = lHit.collider.transform.parent.gameObject;
                        var lStructure = lGameObject.GetComponent<Structure> ();
                        if (lStructure != UPlayer && UPlayer.CanDetect (lStructure)) UPlayer.USelected.UId.Value = lStructure.UId;
                    }
                }
            }

            if (iReverseButtonUI.Reversing) SetFwd (-1);
        }

        public void SetFwd (float aSetting) {
            if (UPlayer == null) return;
            if (iReverseButtonUI.Reversing && aSetting != -1) return;
            foreach (var lEngine in UPlayer.GetEquipmentStates<EnginePrototype.State> ()) {
                lEngine.LinearSetting = new Vector3(lEngine.LinearSetting.x, lEngine.LinearSetting.y, aSetting);
            }
        }

        public void SetYaw (float setting) {
            if (UPlayer == null) return;
            foreach (var lEngine in UPlayer.GetEquipmentStates<EnginePrototype.State> ()) {
                lEngine.AngularSetting = new Vector3(lEngine.AngularSetting.x, setting, lEngine.AngularSetting.z);
            }
        }

        public void SetPitch (float setting) {
            if (UPlayer == null) return;
            foreach (var lEngine in UPlayer.GetEquipmentStates<EnginePrototype.State> ()) {
                lEngine.AngularSetting = new Vector3(setting, lEngine.AngularSetting.y, lEngine.AngularSetting.z);
            }
        }

        private void FireAll (object sender, EventArgs args) {
            foreach (var lState in UPlayer.GetEquipmentStates<BeamLaserPrototype.State> ()) {
                lState.Activated = false;
                lState.Slot.Equipment.OnClicked (lState.Slot);
                lState.Activated = true;
            }
            foreach (var lState in UPlayer.GetEquipmentStates<PulseLaserPrototype.State> ()) {
                lState.Activated = false;
                lState.Slot.Equipment.OnClicked (lState.Slot);
                lState.Activated = true;
            }
            foreach (var lState in UPlayer.GetEquipmentStates<LauncherPrototype.State> ()) {
                lState.Activated = false;
                lState.Slot.Equipment.OnClicked (lState.Slot);
                lState.Activated = true;
            }
        }

        private void LockSelected (object sender, EventArgs args) {
            if (UPlayer != null) UPlayer.Lock (UPlayer.USelected.UValue);
        }

        private bool ClickingUI () {
            PointerEventData ped = new PointerEventData (iEventSystem) {
                position = UnityEngine.Input.mousePosition
            };
            List<RaycastResult> res = new List<RaycastResult> ();
            iGraphicRaycaster.Raycast (ped, res);
            return res.Count > 0;
        }
    }
}

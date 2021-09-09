using DarkFrontier.Equipment;
using DarkFrontier.Structures;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class PlayerController : SingletonBase<PlayerController> {
    [SerializeField] private Structure _player;
    public Structure Player { get => _player; set => _player = value; }

    public EventHandler OnSelectedChanged;
    public EventHandler OnLockSelected;
    public EventHandler OnLocksChanged;

    private GraphicRaycaster _graphicRaycaster;
    private EventSystem _eventSystem;

    private ReverseButton _reverseButtonUI;

    private void Start () {
        _graphicRaycaster = FindObjectOfType<GraphicRaycaster> ();
        _eventSystem = EventSystem.current;

        _reverseButtonUI = ReverseButton.Instance;
    }

    private void Update () {
        if (Player == null) return;

        if (Input.GetMouseButtonDown (0) && !ClickingUI ()) {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (ray, out RaycastHit hit)) {
                if (hit.collider.transform.parent != null) {
                    GameObject obj = hit.collider.transform.parent.gameObject;
                    Structure str = obj.GetComponent<Structure> ();
                    if (str != Player && Player.Detected.Contains (str)) Player.Selected = str;
                }
            }
        }

        if (_reverseButtonUI.Reversing) SetFwd (-1);
    }

    private void OnEnable () {
        FireAllButton.Instance.OnClicked += FireAll;
        OnLockSelected += LockSelected;
    }

    private void OnDisable () {
        if (FireAllButton.Instance != null) FireAllButton.Instance.OnClicked -= FireAll;
        OnLockSelected -= LockSelected;
    }

    public void SetFwd (float setting) {
        if (Player == null) return;
        if (!_reverseButtonUI.Reversing || setting == -1) Player.GetEquipmentStates<EnginePrototype.State> ().ForEach (state => state.LinearSetting = new Vector3 (state.LinearSetting.x, state.LinearSetting.y, setting));
    }

    public void SetYaw (float setting) {
        if (Player == null) return;
        Player.GetEquipmentStates<EnginePrototype.State> ().ForEach (state => state.AngularSetting = new Vector3 (state.AngularSetting.x, setting, state.AngularSetting.z));
    }

    public void SetPitch (float setting) {
        if (Player == null) return;
        Player.GetEquipmentStates<EnginePrototype.State> ().ForEach (state => state.AngularSetting = new Vector3 (setting, state.AngularSetting.y, state.AngularSetting.z));
    }

    private void FireAll (object sender, EventArgs args) {
        Player.GetEquipmentStates<BeamLaserPrototype.State> ().ForEach (state => {
            state.Activated = false;
            state.Slot.Equipment.OnClicked (state.Slot);
            state.Activated = true;
        });
        Player.GetEquipmentStates<PulseLaserPrototype.State> ().ForEach (state => {
            state.Activated = false;
            state.Slot.Equipment.OnClicked (state.Slot);
            state.Activated = true;
        });
        Player.GetEquipmentStates<LauncherPrototype.State> ().ForEach (state => {
            state.Activated = false;
            state.Slot.Equipment.OnClicked (state.Slot);
            state.Activated = true;
        });
    }

    private void LockSelected (object sender, EventArgs args) {
        if (Player != null) Player.Lock (Player.Selected);
    }

    private bool ClickingUI () {
        PointerEventData ped = new PointerEventData (_eventSystem) {
            position = Input.mousePosition
        };
        List<RaycastResult> res = new List<RaycastResult> ();
        _graphicRaycaster.Raycast (ped, res);
        return res.Count > 0;
    }
}

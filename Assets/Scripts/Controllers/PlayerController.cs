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
        if (!_reverseButtonUI.Reversing || setting == -1) Player.GetEquipmentData<EngineSlotData> ().ForEach (engine => engine.LinearSetting = new Vector3 (engine.LinearSetting.x, engine.LinearSetting.y, setting));
    }

    public void SetYaw (float setting) {
        if (Player == null) return;
        Player.GetEquipmentData<EngineSlotData> ().ForEach (engine => engine.AngularSetting = new Vector3 (engine.AngularSetting.x, setting, engine.AngularSetting.z));
    }

    public void SetPitch (float setting) {
        if (Player == null) return;
        Player.GetEquipmentData<EngineSlotData> ().ForEach (engine => engine.AngularSetting = new Vector3 (setting, engine.AngularSetting.y, engine.AngularSetting.z));
    }

    private void FireAll (object sender, EventArgs args) {
        Player.GetEquipmentData<BeamLaserSlotData> ().ForEach (data => {
            data.Activated = false;
            data.Equipment.OnClicked (data.Slot);
            data.Activated = true;
        });
        Player.GetEquipmentData<PulseLaserSlotData> ().ForEach (data => {
            data.Activated = false;
            data.Equipment.OnClicked (data.Slot);
            data.Activated = true;
        });
        Player.GetEquipmentData<LauncherSlotData> ().ForEach (data => {
            data.Activated = false;
            data.Equipment.OnClicked (data.Slot);
            data.Activated = true;
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

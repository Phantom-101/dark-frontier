using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class PlayerController : SingletonBase<PlayerController> {
    [SerializeField] private Structure _player;
    public Structure Player { get => _player; set => _player = value; }

    public EventHandler SelectedChanged;
    public EventHandler LocksChanged;
    public EventHandler LockSelected;

    private GraphicRaycaster _graphicRaycaster;
    private EventSystem _eventSystem;

    private ReverseButtonUI _reverseButtonUI;

    private void Awake () {
        _graphicRaycaster = FindObjectOfType<GraphicRaycaster> ();
        _eventSystem = EventSystem.current;

        _reverseButtonUI = ReverseButtonUI.Instance;

        FireAllButtonUI.Instance.FireAll += (sender, args) => {
            Player.GetEquipmentData<BeamLaserSlotData> ().ForEach (data => {
                if (!data.Activated) data.Equipment.OnClicked (data.Slot);
            });
            Player.GetEquipmentData<PulseLaserSlotData> ().ForEach (data => {
                if (!data.Activated) data.Equipment.OnClicked (data.Slot);
            });
            Player.GetEquipmentData<LauncherSlotData> ().ForEach (data => {
                if (!data.Activated) data.Equipment.OnClicked (data.Slot);
            });
        };

        LockSelected += (sender, args) => { if (Player != null) Player.Lock (Player.Selected); };
    }

    private void Update () {
        if (Player == null) return;

        if (Input.GetMouseButtonDown (0) && !ClickingUI ()) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

            if (Physics.Raycast (ray, out hit)) {
                if (hit.collider.transform.parent != null) {
                    GameObject obj = hit.collider.transform.parent.gameObject;
                    Structure str = obj.GetComponent<Structure> ();
                    if (str != Player && Player.Detected.Contains (str)) Player.Selected = str;
                }
            }
        }

        if (_reverseButtonUI.Reversing) SetFwd (-1);
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

    private bool ClickingUI () {
        PointerEventData ped = new PointerEventData (_eventSystem) {
            position = Input.mousePosition
        };
        List<RaycastResult> res = new List<RaycastResult> ();
        _graphicRaycaster.Raycast (ped, res);
        return res.Count > 0;
    }
}

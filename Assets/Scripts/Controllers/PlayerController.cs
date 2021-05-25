﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class PlayerController : MonoBehaviour {
    [SerializeField] private Structure _player;

    public VoidEventChannelSO SelectedChangedChannel;
    public VoidEventChannelSO LocksChangedChannel;

    [SerializeField] private VoidEventChannelSO _revDown;
    [SerializeField] private VoidEventChannelSO _revUp;
    [SerializeField] private VoidEventChannelSO _fireAll;
    [SerializeField] private VoidEventChannelSO _lockSelected;
    [SerializeField] private GraphicRaycaster _graphicsRaycaster;
    [SerializeField] private EventSystem _eventSystem;

    [SerializeField] private bool _rev;

    private static PlayerController _instance;

    private void Awake () {
        _instance = this;

        _revDown.OnEventRaised += () => { _rev = true; };
        _revUp.OnEventRaised += () => { _rev = false; };
        _fireAll.OnEventRaised += () => {
            _player.GetEquipmentData<BeamLaserSlotData> ().ForEach (data => {
                if (!data.Activated) data.Equipment.OnClicked (data.Slot);
            });
            _player.GetEquipmentData<LauncherSlotData> ().ForEach (data => {
                if (!data.Activated) data.Equipment.OnClicked (data.Slot);
            });
        };
        _lockSelected.OnEventRaised += () => { _player.Lock (_player.Selected); };
    }

    private void Update () {
        if (_player == null) return;

        if (Input.GetMouseButtonDown (0) && !ClickingUI ()) {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

            if (Physics.Raycast (ray, out hit)) {

                if (hit.collider.transform.parent != null) {

                    GameObject obj = hit.collider.transform.parent.gameObject;
                    Structure str = obj.GetComponent<Structure> ();
                    if (str != _player && _player.Detected.Contains (str)) _player.Selected = str;

                }

            }

        }

        if (_rev) SetFwd (-1);
    }

    public Structure GetPlayer () { return _player; }

    public void SetPlayer (Structure player) { _player = player; }

    public static PlayerController GetInstance () { return _instance; }

    public void SetFwd (float setting) {
        if (_player == null) return;
        if (!_rev || setting == -1) _player.GetEquipmentData<EngineSlotData> ().ForEach (engine => engine.LinearSetting = new Vector3 (engine.LinearSetting.x, engine.LinearSetting.y, setting));
    }

    public void SetYaw (float setting) {
        if (_player == null) return;
        _player.GetEquipmentData<EngineSlotData> ().ForEach (engine => engine.AngularSetting = new Vector3 (engine.AngularSetting.x, setting, engine.AngularSetting.z));
    }

    public void SetPitch (float setting) {
        if (_player == null) return;
        _player.GetEquipmentData<EngineSlotData> ().ForEach (engine => engine.AngularSetting = new Vector3 (setting, engine.AngularSetting.y, engine.AngularSetting.z));
    }

    private bool ClickingUI () {
        PointerEventData ped = new PointerEventData (_eventSystem) {
            position = Input.mousePosition
        };
        List<RaycastResult> res = new List<RaycastResult> ();
        _graphicsRaycaster.Raycast (ped, res);
        return res.Count > 0;
    }
}

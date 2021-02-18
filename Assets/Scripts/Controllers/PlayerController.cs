using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class PlayerController : MonoBehaviour {

    [SerializeField] private Structure _player;

    public VoidEventChannelSO TargetChangedChannel;

    [SerializeField] private VoidEventChannelSO _revDown;
    [SerializeField] private VoidEventChannelSO _revUp;
    [SerializeField] private VoidEventChannelSO _fireAll;
    [SerializeField] private GraphicRaycaster _graphicsRaycaster;
    [SerializeField] private EventSystem _eventSystem;

    private bool _rev;

    private static PlayerController _instance;

    private void Awake () {

        _instance = this;

        _revDown.OnEventRaised += () => { _rev = true; };
        _revUp.OnEventRaised += () => { _rev = false; };
        _fireAll.OnEventRaised += () => { _player.GetEquipment<WeaponSlot> ().ForEach ((slot) => { slot.TargetState = true; }); };

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
                    if (str != _player) _player.Target = str;

                }

            }

        }

        if (_rev) _player.GetEquipment<EngineSlot> ()[0].ForwardSetting = -0.5f;

    }

    public Structure GetPlayer () { return _player; }

    public void SetPlayer (Structure player) { _player = player; }

    public static PlayerController GetInstance () { return _instance; }

    public void SetFwd (float setting) {

        if (!_rev) _player.GetEquipment<EngineSlot> ()[0].ForwardSetting = setting;

    }

    public void SetYaw (float setting) {

        _player.GetEquipment<EngineSlot> ()[0].YawSetting = setting;

    }

    public void SetPitch (float setting) {

        _player.GetEquipment<EngineSlot> ()[0].PitchSetting = setting;

    }

    private bool ClickingUI () {

        PointerEventData ped = new PointerEventData (_eventSystem);
        ped.position = Input.mousePosition;
        List<RaycastResult> res = new List<RaycastResult> ();
        _graphicsRaycaster.Raycast (ped, res);
        return res.Count > 0;

    }

}

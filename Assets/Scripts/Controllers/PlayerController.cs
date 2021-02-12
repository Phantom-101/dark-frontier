using System;
using UnityEngine;

[Serializable]
public class PlayerController : MonoBehaviour {

    [SerializeField] private Structure _player;

    public VoidEventChannelSO TargetChangedChannel;

    [SerializeField] private VoidEventChannelSO _leftDown;
    [SerializeField] private VoidEventChannelSO _leftUp;
    [SerializeField] private VoidEventChannelSO _rightDown;
    [SerializeField] private VoidEventChannelSO _rightUp;

    private static PlayerController _instance;

    private void Awake () {

        _instance = this;

        _leftDown.OnEventRaised += Left;
        _leftUp.OnEventRaised += Neutral;
        _rightDown.OnEventRaised += Right;
        _rightUp.OnEventRaised += Neutral;

    }

    private void Update () {

        if (_player == null) return;

        if (Input.GetMouseButtonDown (0)) {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

            if (Physics.Raycast (ray, out hit)) {

                if (hit.collider.transform.parent != null) {

                    GameObject obj = hit.collider.transform.parent.gameObject;
                    Structure str = obj.GetComponent<Structure> ();
                    if (str != _player) _player.SetTarget (str);

                }

            }

        }

    }

    public Structure GetPlayer () { return _player; }

    public void SetPlayer (Structure player) { _player = player; }

    public static PlayerController GetInstance () { return _instance; }

    private void Left () {

        _player.GetEquipment<EngineSlot> ()[0].TurnSetting = -1;

    }

    private void Neutral () {

        _player.GetEquipment<EngineSlot> ()[0].TurnSetting = 0;

    }

    private void Right () {

        _player.GetEquipment<EngineSlot> ()[0].TurnSetting = 1;

    }

    private void Fwd () {

        _player.GetEquipment<EngineSlot> ()[0].ForwardSetting = 1;

    }

    private void Stop () {

        _player.GetEquipment<EngineSlot> ()[0].ForwardSetting = 0;

    }

    public void SetFwd (float setting) {

        _player.GetEquipment<EngineSlot> ()[0].ForwardSetting = setting;

    }

}

using System;
using UnityEngine;

[Serializable]
public class PlayerController : MonoBehaviour {

    [SerializeField] private Structure _player;

    public VoidEventChannelSO TargetChangedChannel;

    private static PlayerController _instance;

    private void Awake () {

        _instance = this;

    }

    private void Update () {

        if (_player == null) return;

        if (Input.GetMouseButtonDown (0)) {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

            if (Physics.Raycast (ray, out hit)) {

                GameObject obj = hit.collider.transform.parent.gameObject;
                Structure str = obj.GetComponent<Structure> ();
                if (str != _player) _player.SetTarget (str);

            }

        }

    }

    public Structure GetPlayer () { return _player; }

    public void SetPlayer (Structure player) { _player = player; }

    public static PlayerController GetInstance () { return _instance; }

}

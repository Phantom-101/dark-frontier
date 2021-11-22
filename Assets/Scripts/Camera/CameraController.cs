using System;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Locations;
using DarkFrontier.Structures;
using UnityEngine;

namespace DarkFrontier.Camera {
    public class CameraController : MonoBehaviour {

        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private Location _anchor;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Location _target;
        [SerializeField] private float _followSpeed;

        private static CameraController _instance;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
    
        public UnityEngine.Camera Camera { get => _camera; }

        private void Awake () {
            _instance = this;
        }

        private void FixedUpdate () {

            Structure player = iPlayerController.Value.UPlayer;
            if (player == null) return;
            Transform pt = _anchor?.UTransform == null ? player.transform : _anchor.UTransform;

            if (_target.UTransform == null) {

                Vector3 scOff = _offset * player.UPrototype.ApparentSize;
                Vector3 targetPos = pt.position + pt.rotation * scOff;
                Vector3 offset = targetPos - transform.position;
                Debug.DrawLine (transform.position, targetPos, Color.red);
                transform.Translate (offset * (_followSpeed * UnityEngine.Time.deltaTime), Space.World);
                transform.LookAt (pt.position + pt.rotation * Vector3.forward * (player.UPrototype.ApparentSize * 2), player.transform.up);

            } else {

                Vector3 difVec = _target.UPosition - pt.position;
                difVec.y = 0;
                Vector3 norVec = difVec.normalized;
                Vector3 scOff = _offset * player.UPrototype.ApparentSize;
                Vector3 targetOffset = new Vector3 (scOff.x * norVec.x, scOff.y, scOff.z * norVec.z);
                Vector3 targetPos = pt.position + pt.rotation * targetOffset;
                Vector3 offset = targetPos - transform.position;
                Debug.DrawLine (transform.position, targetPos, Color.red);
                transform.Translate (offset * (_followSpeed * UnityEngine.Time.deltaTime), Space.World);
                transform.LookAt (_target.UPosition, player.transform.up);

            }

        }

        public void SetAnchor (Location anchor) { _anchor = anchor; }

        public void RemoveAnchor () { _anchor = null; }

        public void SetTarget (Location target) { _target = target; }

        public void RemoveTarget () { _target = null; }

        public static CameraController GetInstance () { return _instance; }

    }
}

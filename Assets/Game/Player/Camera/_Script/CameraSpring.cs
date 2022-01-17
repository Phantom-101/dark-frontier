using DarkFrontier.Positioning.Navigation;
using DarkFrontier.Utils;
using UnityEngine;

#nullable enable
namespace DarkFrontier.Game.Player.Camera
{
    public class CameraSpring : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Camera? _camera;

        [SerializeField]
        private Transform? _lookAt;

        [SerializeField]
        private Renderer? _bounds;

        [SerializeField, Range(0, 1)]
        private float _screenRelativeSize = .5f;

        [SerializeField]
        private float _distance;

        [SerializeField]
        private float _angle = 30;

        [SerializeField]
        private float _force = 10;

        [SerializeField]
        private float _damp = 5;

        [SerializeField]
        private float _drag = 1;

        private GameObject _anchor = null!;
        private SpringJoint _spring = null!;

        public void Initialize()
        {
            _anchor = new GameObject("Anchor");
            _anchor.transform.SetParent(transform);
                
            _spring = _anchor.AddComponent<SpringJoint>();
            _spring.autoConfigureConnectedAnchor = false;
            _spring.connectedAnchor = Vector3.zero;
            _anchor.GetComponent<Rigidbody>().isKinematic = true;
        }

        public void Tick()
        {
            if (_camera == null) return;
            
            if(_bounds != null)
            {
                _distance = Navigation.Chebyshev(_bounds.bounds.size) / _screenRelativeSize;
            }
            _anchor.transform.localPosition = new Vector3(0, Mathf.Sin(_angle * Mathf.Deg2Rad), -Mathf.Cos(_angle * Mathf.Deg2Rad)) * _distance;
            
            Rigidbody cameraRigidbody = ComponentUtils.AddOrGet<Rigidbody>(_camera.gameObject);
            cameraRigidbody.drag = _drag;
            
            _spring.connectedBody = cameraRigidbody;
            _spring.spring = _force;
            _spring.damper = _damp;
            
            _camera.transform.LookAt(_lookAt);
        }
    }
}
#nullable restore
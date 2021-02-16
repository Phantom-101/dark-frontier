using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private Camera _camera;
    [SerializeField] private Location _anchor;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Location _target;
    [SerializeField] private float _followSpeed;

    private PlayerController _pc;

    private static CameraController _instance;

    public Camera Camera { get => _camera; }

    private void Awake () {

        _instance = this;

    }

    private void Start () {

        _pc = PlayerController.GetInstance ();

    }

    private void FixedUpdate () {

        Structure player = _pc.GetPlayer ();
        Transform pt = _anchor?.GetTransform () == null ? player.transform : _anchor.GetTransform ();

        if (_target.GetTransform () == null) {

            Vector3 scOff = _offset * player.Profile.ApparentSize;
            Vector3 targetPos = pt.position + pt.rotation * scOff;
            Vector3 offset = targetPos - transform.position;
            Debug.DrawLine (transform.position, targetPos, Color.red);
            transform.Translate (offset * _followSpeed * Time.deltaTime, Space.World);
            transform.LookAt (pt.position + pt.rotation * Vector3.forward * player.Profile.ApparentSize * 2, player.transform.up);

        } else {

            Vector3 difVec = _target.GetPosition () - pt.position;
            difVec.y = 0;
            Vector3 norVec = difVec.normalized;
            Vector3 scOff = _offset * player.Profile.ApparentSize;
            Vector3 targetOffset = new Vector3 (scOff.x * norVec.x, scOff.y, scOff.z * norVec.z);
            Vector3 targetPos = pt.position + pt.rotation * targetOffset;
            Vector3 offset = targetPos - transform.position;
            Debug.DrawLine (transform.position, targetPos, Color.red);
            transform.Translate (offset * _followSpeed * Time.deltaTime, Space.World);
            transform.LookAt (_target.GetPosition (), player.transform.up);

        }

    }

    public void SetAnchor (Location anchor) { _anchor = anchor; }

    public void RemoveAnchor () { _anchor = null; }

    public void SetTarget (Location target) { _target = target; }

    public void RemoveTarget () { _target = null; }

    public static CameraController GetInstance () { return _instance; }

}

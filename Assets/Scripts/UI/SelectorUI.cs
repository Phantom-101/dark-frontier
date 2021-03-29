using UnityEngine;
using UnityEngine.UI;

public class SelectorUI : MonoBehaviour {

    public Structure Structure;
    [SerializeField] private GameObject _lock;
    [SerializeField] private GameObject _unlock;
    [SerializeField] private SpriteAnimationUI _anim;
    [SerializeField] private bool _current = false;
    [SerializeField] private int _target = 0;
    [SerializeField] private bool _destroy = false;

    PlayerController _pc;
    CameraController _cc;
    Button _button;

    private void Start () {

        _pc = PlayerController.GetInstance ();
        _cc = CameraController.GetInstance ();
        _button = GetComponent<Button> ();
        _button.onClick.AddListener (() => { if (_pc.GetPlayer () != null && _pc.GetPlayer () != Structure) _pc.GetPlayer ().Selected = Structure; });

    }

    private void Update () {

        if (_anim == null) {

            _anim = Instantiate (_unlock, transform).GetComponent<SpriteAnimationUI> ();
            _anim.Elapsed = 0;
            _anim.UpdateSprite ();

        } else {

            Structure player = _pc.GetPlayer ();
            if (player == null || Structure == null || player == Structure) {
                _target = 0;
                _destroy = true;
            } else {
                if (player.Locks.ContainsKey (Structure)) {
                    if (player.Locks[Structure] == 100) _target = 2;
                    else _target = 1;
                    _destroy = false;
                } else {
                    _target = 0;
                    if (!player.Detected.Contains (Structure)) _destroy = true;
                    else _destroy = false;
                }
            }

            int num = _current ? 2 : 0;
            if (_target == num) {
                _anim.TimeScale = 1;
                if (!_current && _destroy && _anim.Completed ()) {
                    Destroy (this);
                    LeanTween.value (gameObject, 1, 0, 0.5f).setOnUpdateParam (gameObject).setOnUpdateObject ((float value, object obj) => {
                        if (_anim == null || _anim.gameObject == null) return;
                        Image img = _anim.GetComponent<Image> ();
                        if (img == null) return;
                        Color c = img.color;
                        c.a = value;
                        img.color = c;
                    });
                    Destroy (gameObject, 1);
                    return;
                }
            } else {
                _anim.TimeScale = -1;
                if (_anim.Completed () && (_current || _target == 2)) {
                    _current = !_current;
                    Destroy (_anim.gameObject);
                    _anim = Instantiate (_current ? _lock : _unlock, transform).GetComponent<SpriteAnimationUI> ();
                    _anim.Elapsed = 0;
                    _anim.UpdateSprite ();
                }
            }

        }

        if (Structure != null) {
            Camera cam = _cc.Camera;
            Vector3 screen = cam.WorldToScreenPoint (Structure.transform.position);
            Vector3 local = transform.parent.InverseTransformPoint (screen);
            transform.localPosition = local;
        }

    }

}

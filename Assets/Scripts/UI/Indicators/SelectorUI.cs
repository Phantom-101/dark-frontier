using System;
using System.Linq;
using DarkFrontier.Camera;
using DarkFrontier.Controllers;
using DarkFrontier.Foundation.Services;
using DarkFrontier.Structures;
using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI.Indicators {
    public class SelectorUI : MonoBehaviour {

        public Structure Structure;
        [SerializeField] private GameObject _lock;
        [SerializeField] private GameObject _unlock;
        [SerializeField] private SpriteAnimationUI _anim;
        [SerializeField] private bool _current = false;
        [SerializeField] private int _target = 0;
        [SerializeField] private bool _destroy = false;
        [SerializeField] private float _tweenAlpha = 1;
        [SerializeField] private float _alphaMult = 1;

        CameraController _cc;
        Button _button;
        Image _img;

        private readonly Lazy<PlayerController> iPlayerController = new Lazy<PlayerController>(() => Singletons.Get<PlayerController>(), false);
    
        private void Start () {
            _cc = CameraController.GetInstance ();
            _button = GetComponent<Button> ();
            _button.onClick.AddListener (() => { if (iPlayerController.Value.UPlayer != null && iPlayerController.Value.UPlayer != Structure) iPlayerController.Value.UPlayer.uSelected.UId.Value = Structure.uId; });
        }

        private void Update () {

            if (_anim == null) {

                _anim = Instantiate (_unlock, transform).GetComponent<SpriteAnimationUI> ();
                _anim.Elapsed = 0;
                _anim.UpdateSprite ();
                _img = _anim.GetComponent<Image> ();

            } else {

                Structure player = iPlayerController.Value.UPlayer;
                if (player == null || Structure == null || player == Structure) {
                    _target = 0;
                    _destroy = true;
                } else {
                    if (player.uLocks.Keys.Any (lGetter => lGetter.UId.Value == Structure.uId)) {
                        if (player.uLocks.Where (lPair => lPair.Key.UId.Value == Structure.uId).First ().Value == 1) _target = 2;
                        else _target = 1;
                        _destroy = false;
                    } else {
                        _target = 0;
                        if (!player.CanDetect (Structure)) _destroy = true;
                        else _destroy = false;
                    }
                }

                int num = _current ? 2 : 0;
                if (_target == num) {
                    _anim.TimeScale = 1;
                    if (!_current && _destroy && _anim.Completed ()) {
                        Destroy (this);
                        LeanTween.value (gameObject, 1, 0, 0.2f).setOnUpdateParam (gameObject).setOnUpdateObject ((float value, object obj) => {
                            _tweenAlpha = value;
                            if (_img == null) return;
                            Color c = _img.color;
                            c.a = _tweenAlpha * _alphaMult;
                            _img.color = c;
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
                        _img = _anim.GetComponent<Image> ();
                    }
                }

            }

            if (Structure != null) {
                UnityEngine.Camera cam = _cc.Camera;
                Vector3 screen = cam.WorldToScreenPoint (Structure.transform.position);
                Vector3 local = transform.parent.InverseTransformPoint (screen);
                transform.localPosition = new Vector3 (local.x, local.y);
                if (local.z > 0) {
                    _alphaMult = 1;
                    _button.enabled = true;
                } else {
                    _alphaMult = 0;
                    _button.enabled = false;
                }
            }

            SetAlpha ();

        }

        private void SetAlpha () {

            if (_img == null) return;
            Color c = _img.color;
            c.a = _tweenAlpha * _alphaMult;
            _img.color = c;

        }

    }
}

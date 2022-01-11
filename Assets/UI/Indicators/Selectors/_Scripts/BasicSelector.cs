using UnityEngine;
using UnityEngine.UI;

#nullable enable
namespace DarkFrontier.UI.Indicators.Selectors
{
    public class BasicSelector : Selector
    {
        public ISelectable? selectable;

        private float _baseSize;

        private RectTransform _transform = null!;

        private Image? _image;

        private UnityEngine.Camera? _camera;

        public override void Initialize()
        {
            _transform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
            _camera = UnityEngine.Camera.main;

            _baseSize = _transform.sizeDelta.x;
        }

        public override void Tick(float dt)
        {
            if(selectable == null || _image == null || _camera == null)
            {
                Destroy(gameObject);
                return;
            }

            var screen = _camera.WorldToScreenPoint(selectable.Position);
            var local = _transform.parent.InverseTransformPoint(screen);

            _transform.localPosition = new Vector3(local.x, local.y, _transform.localPosition.z);

            var color = _image.color;
            color.a = local.z >= 0 ? 1 : 0;
            _image.color = color;
        }
    }
}
#nullable restore
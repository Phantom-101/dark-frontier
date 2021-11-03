using UnityEngine;
using UnityEngine.UI;

namespace DarkFrontier.UI {
    public class SpriteAnimationUI : MonoBehaviour {

        public Sprite[] Sprites;
        public AnimationCurve Anim;
        public float Length;
        public float Elapsed;
        public float TimeScale;

        private Image _image;

        private void Awake () {

            _image = GetComponent<Image> ();

        }

        private void Update () {

            if (_image == null) {
                Debug.Log ($"Image component not found on {gameObject.name}");
                return;
            }

            Elapsed = Mathf.Clamp (Elapsed + UnityEngine.Time.deltaTime * TimeScale, 0, Length);
            UpdateSprite ();

        }

        public void UpdateSprite () {

            float progress = Elapsed / Length;
            float curFrame = Anim.Evaluate (progress);
            int frame = (int) (curFrame * (Sprites.Length - 1));
            _image.sprite = Sprites[frame];

        }

        public bool Completed () {
            return (TimeScale > 0 && Elapsed == Length) || (TimeScale < 0 && Elapsed == 0);
        }

    }
}

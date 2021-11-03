using UnityEngine;

namespace DarkFrontier.Other {
    public class TestMover : MonoBehaviour {

        [SerializeField]
        private Rigidbody _rb;

        private void FixedUpdate () {

            _rb.MovePosition (transform.position + new Vector3 (0, 0, 0.1f) * UnityEngine.Time.deltaTime);

        }

    }
}

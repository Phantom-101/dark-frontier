using UnityEngine;
using static UnityEngine.ParticleSystem;

public class MissilePS : MonoBehaviour {

    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private ParticleSystem _ps;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _triggerDistance;
    [SerializeField]
    private float _releaseForce;
    private Particle[] _particles;

    private void Awake () {

        MainModule main = _ps.main;
        main.startLifetime = _range / _speed;
        main.startSpeed = _releaseForce;

        _particles = new Particle[_ps.main.maxParticles];

    }

    private void Update () {

        int num = _ps.GetParticles (_particles);

        for (int i = 0; i < num; i++) {

            Vector3 offset = _target.transform.position - _particles[i].position;
            _particles[i].velocity += offset.normalized * _speed * Time.deltaTime;
            if (offset.sqrMagnitude <= _triggerDistance * _triggerDistance) {

                _particles[i].remainingLifetime = 0;
                _ps.TriggerSubEmitter (0, ref _particles[i]);

            }

        }

        _ps.SetParticles (_particles, num);

    }

}

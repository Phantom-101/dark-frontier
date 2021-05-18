using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Rotator : MonoBehaviour {
    public float Speed;
    [Range (0, 180)]
    public float HalfArc;
    public ParticleSystem System;
    public Color HitColor;
    public float TimeMultiplier;

    private Particle[] _particles;

    private void Update () {
        if (_particles == null || _particles.Length < System.main.maxParticles) {
            _particles = new Particle[System.main.maxParticles];
        }
        transform.Rotate (new Vector3 (0, Speed * Time.deltaTime, 0), Space.Self);
        if (transform.localEulerAngles.y < 180 && transform.localEulerAngles.y > HalfArc) {
            transform.localEulerAngles = new Vector3 (
                transform.localEulerAngles.x,
                -HalfArc,
                transform.localEulerAngles.z
            );
        }
        int numParticles = System.GetParticles (_particles);
        for (int i = 0; i < numParticles; i++) {
            Particle particle = _particles[i];
            if (particle.velocity.sqrMagnitude < 1) {
                if (particle.startColor != HitColor) {
                    particle.startLifetime = 2 * HalfArc / Speed * TimeMultiplier;
                    particle.remainingLifetime = 2 * HalfArc / Speed * TimeMultiplier;
                    particle.startColor = HitColor;
                    _particles[i] = particle;
                }
            }
        }
        System.SetParticles (_particles, numParticles);
    }
}

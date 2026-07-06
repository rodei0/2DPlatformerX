using UnityEngine;

public class ParticlesEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleEffect;
    [SerializeField] private ParticleSystem particleBurstEffect;

    public void StopParticles() => particleEffect.Stop();

    public void ResumeParticles() => particleEffect.Play();

    public void PlayBurst() => particleBurstEffect.Play();

}
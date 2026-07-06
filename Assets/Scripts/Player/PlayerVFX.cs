using System.Collections;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [Header("Particles:")]
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private ParticleSystem dustFuego;
    [SerializeField] private ParticleSystem dustAgua;
    [SerializeField] private ParticleSystem fuego;
    [SerializeField] private ParticleSystem agua;
    [SerializeField] private ParticleSystem aguaCircle;
    [SerializeField] private ParticleSystem fuegoCircle;

    [SerializeField] private SpriteRenderer srBody;
    [SerializeField] private SpriteRenderer srHead;

    [SerializeField] private Color colorFire;
    [SerializeField] private Color colorWater;

    [Header("Script References:")]
    private PlayerStateList pState;
    private PlayerManagement playerManagement;
    void Start()
    {
        playerManagement = GetComponent<PlayerManagement>();
        pState = GetComponent<PlayerStateList>();
        fuego.Stop();
        agua.Stop();

    }

    private IEnumerator SpawnFire() {
        dustFuego.Play();
        yield return new WaitForSeconds(0.5f);
        dustFuego.Stop();
    }
    private IEnumerator SpawnWater() {
        dustAgua.Play();
        yield return new WaitForSeconds(0.5f);
        dustAgua.Stop();
    }

    public void SpawnFireEffect() {
        StartCoroutine(SpawnFire());
    }

    public void SpawnWaterEffect() {
        StartCoroutine(SpawnWater());
    }

    public void CreateDust() {
        dust.Play();
    }


    public IEnumerator ChangeColor(Color color, float duration) {
        Color start = srBody.color;
        float elapsed = 0f;
        while (elapsed < duration) {
            srBody.color = Color.Lerp(start, color, elapsed / duration);
            srHead.color = Color.Lerp(start, color, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        srBody.color = color;
        srHead.color = color;
        playerManagement.SetActualColor(srBody.color);
    }



    public void ChangeParticles() //cambio de particulas y de color del personaje para los elementos
    {
        if (pState.isFire) {
            StartCoroutine(ChangeColor(colorWater, 0.2f));
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.swapElementSFX, transform.position);
            aguaCircle.Play();
            CameraShake.Instance.Shake(0.2f, 0.1f);
            fuego.Stop();
            agua.Play();
        }
        else {
            StartCoroutine(ChangeColor(colorFire, 0.2f));
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.swapElementSFX, transform.position);
            fuegoCircle.Play();
            CameraShake.Instance.Shake(0.2f, 0.1f);
            agua.Stop();
            fuego.Play();
        }

        pState.isFire = !pState.isFire;
    }

}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    bool isSquashing = false;
    bool isStretching = false;
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
    [SerializeField] private Transform fullSprite;

    [SerializeField] private Color colorFire;
    [SerializeField] private Color colorWater;

    [Header("Script References:")]
    private PlayerStateList pState;
    private PlayerManagement playerManagement;
    void Awake()
    {
        playerManagement = GetComponent<PlayerManagement>();
        pState = GetComponent<PlayerStateList>();
        fuego.Stop();
        agua.Stop();
        FirstColor();
    }

    //funcion para que el personaje empiece con el color de fuego y las particulas de fuego
    private void FirstColor() {
        StartCoroutine(ChangeColor(colorFire, 0.2f));
        var main = dust.main;
        main.startColor = colorFire;
        fuego.Play();
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

    //en las dos funciones se podria sustituir los valores por valores serializados si queremos que se puedan cambiar desde el editor
    //pero no necesario por ahora
    public IEnumerator SquashCharacter() {
        if (isSquashing || isStretching) {
            yield break;
        }
            isSquashing = true;
            Vector3 original = Vector3.one;
            Vector3 squash = new Vector3(1.2f, 0.8f, 1);
            float timer = 0;
            while (timer < 0.08f) {
                timer += Time.deltaTime;
                fullSprite.transform.localScale = Vector3.Lerp(original, squash, timer / 0.08f);
                yield return null;
            }
            timer = 0;

            while (timer < 0.10f) {
                timer += Time.deltaTime;
                fullSprite.transform.localScale = Vector3.Lerp(squash, original, timer / 0.10f);
                yield return null;
            }
            fullSprite.transform.localScale = original;
            isSquashing = false;
    }

    public IEnumerator StretchOnJump() {
        if (isSquashing || isStretching) {
            yield break;
        }
            isStretching = true;
            Vector3 original = Vector3.one;
            Vector3 Stretch = new Vector3(0.8f, 1.2f, 1);
            float timer = 0;
            while (timer < 0.08f) {
                timer += Time.deltaTime;
                fullSprite.transform.localScale = Vector3.Lerp(original, Stretch, timer / 0.08f);
                yield return null;
            }
            timer = 0;

            while (timer < 0.10f){
                timer += Time.deltaTime;
                fullSprite.transform.localScale = Vector3.Lerp(Stretch, original, timer / 0.10f);
                yield return null;
            }
            fullSprite.transform.localScale = original;
            isStretching = false;
    }
    public void ChangeParticles() //cambio de particulas y de color del personaje para los elementos
    {
        if (pState.isFire) {
            var main = dust.main;
            main.startColor = colorWater;
            StartCoroutine(ChangeColor(colorWater, 0.2f));
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.swapElementSFX, transform.position);
            aguaCircle.Play();
            CameraShake.Instance.Shake(0.2f, 0.1f);
            fuego.Stop();
            agua.Play();
        }
        else {
            var main = dust.main;
            main.startColor = colorFire;
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

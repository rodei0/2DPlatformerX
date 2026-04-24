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

    [SerializeField] private SpriteRenderer srBody;
    [SerializeField] private SpriteRenderer srHead;

    private bool isFire = true;

    private PlayerManagement playerManagement;
    void Start()
    {
        playerManagement = GetComponent<PlayerManagement>();
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
        playerManagement.actualColor = srBody.color;
    }



    public void ChangeParticles() //cambio de particulas y de color del personaje para los elementos
    {
        if (isFire == true) {
            StartCoroutine(ChangeColor(new Color(0.23f, 0.345f, 0.655f), 0.2f));
            fuego.Stop();
            agua.Play();
        }
        else {
            StartCoroutine(ChangeColor(new Color(0.619f, 0.227f, 0.10f), 0.2f));
            agua.Stop();
            fuego.Play();
        }

        isFire = !isFire;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        Debug.Log("Triggerea");

        if (collision.CompareTag("Fire")) {
            if (!isFire) playerManagement.KillPlayer();
            else StartCoroutine(SpawnFire());
        }

        if (collision.CompareTag("Agua")) {
            if (isFire) playerManagement.KillPlayer();
            else StartCoroutine(SpawnWater());
        }

    }
}

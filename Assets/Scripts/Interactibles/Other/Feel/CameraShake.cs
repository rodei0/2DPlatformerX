using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Coroutine shakeCoroutine;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Shake(float duration, float magnitude) {
        if (shakeCoroutine != null) {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude) {
        float elapsed = 0f;

        while (elapsed < duration) {
            if (Time.timeScale == 0f) {   // pausado: se queda quieto
                yield return null;
                continue;
            }
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = transform.position + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;

        }
    }
}

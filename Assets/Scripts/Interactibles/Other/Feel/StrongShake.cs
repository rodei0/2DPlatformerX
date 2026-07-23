using System.Collections;
using UnityEngine;

public class StrongShake : MonoBehaviour
{
    [SerializeField] private float initialAngle = 35f;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private int swings = 6;

    private Coroutine shakeCoroutine;

    public void ShakeOnHit() {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine() {
        float elapsed = 0f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            // La amplitud va disminuyendo hasta 0
            float amplitude = Mathf.Lerp(initialAngle, 0f, t);

            // Oscilación
            float angle = Mathf.Sin(t * swings * Mathf.PI) * amplitude;

            transform.localRotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        shakeCoroutine = null;
    }
}

using System.Collections;
using UnityEngine;

public class StretchOnJump : MonoBehaviour
{
   public void StretchCorroutine() {
        StartCoroutine(StretchAnimCoroutine());
    }


    private IEnumerator StretchAnimCoroutine() {
        Vector3 start = Vector3.one;
        Vector3 target = new Vector3(1.4f, 0.6f, 1f);

        float t = 0;
        while (t < 1f) {
            transform.localScale = Vector3.Lerp(start, target, t);
            t += Time.deltaTime * 7f;
            yield return null;
        }

        t = 0;
        while (t < 1f) {
            transform.localScale = Vector3.Lerp(target, start, t);
            t += Time.deltaTime * 7f;
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}

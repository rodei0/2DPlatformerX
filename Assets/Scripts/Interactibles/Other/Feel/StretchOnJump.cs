using System.Collections;
using UnityEngine;

public class StretchOnJump : MonoBehaviour
{
    private bool isStretching = false;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            PlayerStateList player = collision.GetComponent<PlayerStateList>();
            if (player.jumping && !isStretching) {
                StartCoroutine(StretchAnimCoroutine());
                Debug.Log("Stretching!");
            }
        }
    }

    private IEnumerator StretchAnimCoroutine() {
        isStretching = true;

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
        isStretching = false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwapScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private float durationTransition;
    [SerializeField] private Image blackTransition;

    private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player")) {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition() {
        float t = 0;
        while (t < durationTransition) {
            t += Time.deltaTime;
            blackTransition.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t / durationTransition));
            yield return null;
        }
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.changeScene, transform.position);
        SceneManager.LoadScene(sceneName);
    }
}

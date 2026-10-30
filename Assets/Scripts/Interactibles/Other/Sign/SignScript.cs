using UnityEngine;

public class SignScript : MonoBehaviour
{
    [SerializeField] private GameObject Panel;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Panel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Panel.SetActive(false);
        }
    }
}

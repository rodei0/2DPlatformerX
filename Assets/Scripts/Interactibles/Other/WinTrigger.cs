using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            SaveManager.Instance.HasCompletedGame();
            IngameMenu.Instance.OnWinMenu();
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.winSFX, transform.position);
        }
    }
}

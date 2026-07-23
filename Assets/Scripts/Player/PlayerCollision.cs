using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerManagement playerManagement;
    private PlayerVFX playerVFX;
    private PlayerStateList pState;
     void Awake()
     {
        playerManagement = GetComponent<PlayerManagement>();
        playerVFX = GetComponent<PlayerVFX>();
        pState = GetComponent<PlayerStateList>();
     }

    private void OnTriggerStay2D(Collider2D collision) {
        if (pState.isDead) return;

        if (collision.CompareTag("Fire")) {
            if (!pState.isFire) playerManagement.KillPlayer();
        }

        if (collision.CompareTag("Agua")) {
            if (pState.isFire) playerManagement.KillPlayer();
        }

    }
    

    private void OnTriggerEnter2D(Collider2D collision) {
        if (pState.isDead) return;

        if (collision.CompareTag("Fire")) {
            if (pState.isFire)
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.touchWaterSFX, transform.position);
            playerVFX.SpawnFireEffect();
            if (!pState.isFire) playerManagement.KillPlayer();
        }

        if (collision.CompareTag("Agua")) {
            if (!pState.isFire)
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.touchWaterSFX, transform.position);
            playerVFX.SpawnWaterEffect();
            if (pState.isFire) playerManagement.KillPlayer();
        }

        if (collision.CompareTag("Dead")) {
           playerManagement.KillPlayer();
        }
    }



}

using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckPoint : MonoBehaviour
{
    private StrongShake strongShake;
    [SerializeField] private ParticleSystem particles;

    [SerializeField] public string checkpointName;

    private void Awake() {
        strongShake = GetComponent<StrongShake>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) { 
            PlayerManagement playerManagement = collision.GetComponent<PlayerManagement>();
            if (playerManagement != null && transform.position != playerManagement.respawn.position) {
                playerManagement.SetRespawnPoint(transform.position);
                SaveManager.Instance.SetCheckpoint(checkpointName);
                CameraShake.Instance.Shake(0.2f, 0.1f);
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.checkPointTouch, transform.position);
                strongShake.ShakeOnHit();
                particles.Play();
            }
        }
    }


}

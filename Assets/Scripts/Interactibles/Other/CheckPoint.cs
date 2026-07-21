using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) { 
            PlayerManagement playerManagement = collision.GetComponent<PlayerManagement>();
            if (playerManagement != null && transform.position != playerManagement.respawn.position) {
                playerManagement.SetRespawnPoint(transform.position);
            }
        }
    }
}

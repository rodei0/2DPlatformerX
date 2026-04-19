using UnityEngine;

public class JumpResetObject : MonoBehaviour
{
    private bool canUse = true;
    [SerializeField] private float cd;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && canUse)
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ResetJump();
                canUse = false;
                Invoke(nameof(ResetUse), cd);
            }
        }

    }

    private void ResetUse() {
        canUse = true;
        Debug.Log("Jump reset object is ready to use again.");
    }
}

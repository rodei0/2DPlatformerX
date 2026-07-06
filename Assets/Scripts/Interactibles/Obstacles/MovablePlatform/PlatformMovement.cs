using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float speed = 2f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.left * Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("wei no mames");
            if (IsPlayerOnTop(collision)) {
                collision.gameObject.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(null);
        }
    }

    private bool IsPlayerOnTop(Collision2D collision) {
        foreach (ContactPoint2D contact in collision.contacts) {
            Debug.Log("Normal Y: " + contact.normal.y);
            if (contact.normal.y < -0.5f) // la normal apunta hacia arriba
                return true;
        }
        return false;
    }
}

using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{

    public Vector2 direction;
    public float speed;

    private float despawnTime = 7f;
    private float timer = 0;
    private void FixedUpdate() {
       transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer > despawnTime) {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Ground") || collision.CompareTag("Player")) {
            //codigo con animacion que se rompa (particulas mismo)
            Destroy(this.gameObject, 0.01f);
        }
    }

}

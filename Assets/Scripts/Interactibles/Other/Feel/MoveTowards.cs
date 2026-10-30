using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    [SerializeField] private bool right = false;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float destroyX = 15f;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        moveBird();
        DestroyObject();
    }

    private void moveBird() {
        if (!right) {
            transform.localPosition += Vector3.left * speed * Time.deltaTime;
        }
        else if (right) {
            transform.localPosition += Vector3.right * speed * Time.deltaTime;
        }
    }

    private void DestroyObject() {
        if (!right && transform.position.x <= -destroyX) {
            Destroy(gameObject);
        }

        if (right && transform.position.x >= destroyX) {
            Destroy(gameObject);
        }
    }

    public void Setdir(bool isright) {
        this.right = isright;
    }
}

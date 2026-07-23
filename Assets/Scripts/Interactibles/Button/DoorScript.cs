using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator animator;
    private Collider2D coll;

    public bool keepOpen = false;
    void Start() {
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }


    public void OpenDoor() {
        animator.SetBool("isOpened", true);
        coll.isTrigger = true;
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.doorOpened, transform.position);
    }

    public void CloseDoor() {
            animator.SetBool("isOpened", false);
            coll.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            keepOpen = true;
        }
    }
}

using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private Animator animator;
    private bool activated = false;
    [SerializeField] private DoorScript[] doors;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        PlayerManagement.OnPlayerDeath += Reset;
    }
    private void OnDisable() {
        PlayerManagement.OnPlayerDeath -= Reset;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (activated) return;
        activated = true;
        animator.SetBool("isPressed", true);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.buttonTouch, transform.position);
        foreach (DoorScript door in doors) {
            door.OpenDoor();
        }
    }

    private void Reset() {
        bool allDoorsRemainOpen = true;

        foreach (DoorScript door in doors) {
            if (!door.keepOpen) {
                allDoorsRemainOpen = false;
                door.CloseDoor();
            }
        }

        if (!allDoorsRemainOpen) {
            activated = false;
            if (animator != null) {
                animator.SetBool("isPressed", false);
            }
        }
    }
}

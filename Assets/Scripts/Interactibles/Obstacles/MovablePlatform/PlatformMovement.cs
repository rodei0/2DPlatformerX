using Unity.VisualScripting;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed = 2f;

    [SerializeField] private Transform originPoint;
    [SerializeField] private Transform endPoint;


    [SerializeField] private bool moveActivated = true;
    [SerializeField] private bool isLooped = true;
    private bool isMovingToEndPoint = true;
    private bool isMovingToOriginPoint = false;


    void Start()
    {
        transform.position = originPoint.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (moveActivated) {
            Movement();
        } 
    }

    private void OnEnable() {
        if (!isLooped) {
            PlayerManagement.OnPlayerDeath += ResetPlatform;
        }
    }

    private void OnDisable() {
        if (!isLooped)
            PlayerManagement.OnPlayerDeath -= ResetPlatform;
    }

    private void ResetPlatform() {
        transform.position = originPoint.position;
        moveActivated = false;
        isMovingToEndPoint = true;
        isMovingToOriginPoint = false;
    }

    private void Movement() {
        if (isMovingToEndPoint) {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, Time.deltaTime * speed);
            if (transform.position == endPoint.position) {
                isMovingToEndPoint = false;
                isMovingToOriginPoint = true;
            }
        }

        if (isMovingToOriginPoint && isLooped) {
            transform.position = Vector3.MoveTowards(transform.position, originPoint.position, Time.deltaTime * speed);
            if (transform.position == originPoint.position) {
                isMovingToOriginPoint = false;
                isMovingToEndPoint = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (IsPlayerOnTop(collision)) {
                collision.gameObject.transform.SetParent(transform);
                moveActivated = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(null);
        }
    }

    //detectamos si el jugador está encima de la plataforma con la normal
    private bool IsPlayerOnTop(Collision2D collision) {
        foreach (ContactPoint2D contact in collision.contacts) {
            if (contact.normal.y < -0.5f) // la normal apunta hacia arriba
                return true;
        }
        return false;
    }
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 5f;
    public static bool isManualMode = false;
    private Vector3 offset;

    [Tooltip("First element is left limit, second right limit, third up limit, fourth down limit")]
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Transform downLimit;
    [SerializeField] private Transform upLimit;
    private float halfWidth;
    private float halfHeight;

    private void Awake() {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }
    void Start()
    {
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isManualMode = true;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            isManualMode = false;
        }

        if (isManualMode)
        {
            transform.position += new Vector3(horizontal, vertical, 0) * moveSpeed * Time.deltaTime;
        }
        else 
        { 
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }

        LimitCamera();

    }

    private void LimitCamera() {
        if (transform.position.x < leftLimit.position.x + halfWidth) {
            transform.position = new Vector3(leftLimit.position.x + halfWidth, transform.position.y, transform.position.z);
        }
        if (transform.position.x > rightLimit.position.x - halfWidth) {
            transform.position = new Vector3(rightLimit.position.x - halfWidth, transform.position.y, transform.position.z);
        }

        if (transform.position.y > upLimit.position.y - halfHeight) {
            transform.position = new Vector3(transform.position.x, upLimit.position.y - halfHeight, transform.position.z);
        }

        if (transform.position.y < downLimit.position.y + halfHeight) {
            transform.position = new Vector3(transform.position.x, downLimit.position.y + halfHeight, transform.position.z);
        }
    }
}

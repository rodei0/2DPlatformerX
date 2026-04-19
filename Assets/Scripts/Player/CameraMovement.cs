using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 5f;
    public static bool isManualMode = false;
    private Vector3 offset;


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

        
    }
}

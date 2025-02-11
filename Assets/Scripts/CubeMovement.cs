using Unity.VisualScripting;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [SerializeField] private Transform puntoA;
    [SerializeField] private float velocidad = 2f;
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, puntoA.position, velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Si el jugador toca la plataforma
        {
            other.transform.SetParent(transform); // Hacerlo hijo de la plataforma
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Cuando el jugador se baja
        {
            other.transform.SetParent(null); // Eliminar relación con la plataforma
        }
    }
}

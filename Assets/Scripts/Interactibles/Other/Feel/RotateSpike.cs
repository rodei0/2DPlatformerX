using UnityEngine;

public class RotateSpike : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 1300f;

    private void FixedUpdate() {
        Rotation();
    }

    private void Rotation() {
        transform.Rotate(0,0, spinSpeed * Time.deltaTime);
    }
}

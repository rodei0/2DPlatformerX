using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MovementOscile : MonoBehaviour
{
    [SerializeField] private float oscileSpeed;
    [SerializeField] private float amplitude;

    private float randomOffset;

    private Vector2 startPos;

    private void Awake() {
        startPos = transform.position;
        randomOffset = Random.Range(0f, 100f);
    }
    private void Update() {
        MoveUpAndDown();
    }
    private void MoveUpAndDown() {
        float offsetY = Mathf.Sin((Time.time + randomOffset) * oscileSpeed) * amplitude;
        transform.position = new Vector2(startPos.x,startPos.y + offsetY);
    }

}

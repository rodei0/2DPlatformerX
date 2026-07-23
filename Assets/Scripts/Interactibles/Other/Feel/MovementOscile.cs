using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MovementOscile : MonoBehaviour
{
    [Header("Oscile options")]
    [SerializeField] bool oscileUpDown = false;
    [SerializeField] bool oscileRotation = false;

    [Header("Oscile Variables")]
    [SerializeField] private float oscileSpeed;
    [SerializeField] private float amplitude;

    [SerializeField] private float oscileSpeedRot;
    [SerializeField] private float amplitudeRot;


    private float randomOffset;

    private Vector2 startPos;

    private void Awake() {
        startPos = transform.position;
        randomOffset = Random.Range(0f, 100f);
    }
    private void Update() {
        if (oscileUpDown) { MoveUpAndDown(); }
        if (oscileRotation) { RotateOnZ(); }
    }
    private void MoveUpAndDown() {
        float offsetY = Mathf.Sin((Time.time + randomOffset) * oscileSpeed) * amplitude;
        transform.position = new Vector2(startPos.x,startPos.y + offsetY);
    }

    private void RotateOnZ() {
        float rotationZ = Mathf.Sin((Time.time + randomOffset) * oscileSpeedRot) * amplitudeRot;
        transform.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

}

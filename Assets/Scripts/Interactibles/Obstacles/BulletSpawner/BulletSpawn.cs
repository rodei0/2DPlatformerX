using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletSpawn : MonoBehaviour
{
    #region direction variables
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private Direction direction;
    private Quaternion directionRotation;
    private Vector2 directionVector;
    #endregion

    #region firewater things
    [SerializeField] private GameObject bulletFirePrefab;
    [SerializeField] private GameObject bulletWaterPrefab;
    [SerializeField] private bool spawnsWater;
    [SerializeField] private bool spawnsFire;
    #endregion

    [SerializeField] private float spawnRate;
    [SerializeField] private float bulletSpeed;

    private GameObject gameObjectChoosed;

    private Vector3 startScale;
    private float timer;
    void Start()
    {
        startScale = transform.localScale;
        directionVector = chooseDirection(direction);
        directionRotation = chooseRotation(direction);
        if (spawnsWater) {
            gameObjectChoosed = bulletWaterPrefab;
        }
        else if (spawnsFire) {
            gameObjectChoosed = bulletFirePrefab;
        }
        else {
            // Default to water if neither is specified
            gameObjectChoosed = bulletWaterPrefab;
        }
}

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        SpawnBullets();
    }

    private void FixedUpdate() {

    }


    private void SpawnBullets() {
        if (timer >= spawnRate) {
                GameObject bullet = Instantiate(gameObjectChoosed, transform.position, directionRotation);
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.shootSFX, transform.position);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.direction = directionVector;
                bulletScript.speed = bulletSpeed;
            timer = 0f;   
            StartCoroutine(ShootAnimation());
        }
    }

    private IEnumerator ShootAnimation() {
        float transitionTime = 0.1f; // Time taken for the bullet to reach its full speed
        float t = 0;
        while (t < transitionTime) {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, new Vector3(2f, 1, 1), t / transitionTime);
            transform.localScale = Vector3.Lerp(new Vector3(2f, 1, 1), startScale, t / transitionTime);
            yield return null;
        }
    }

    private Vector2 chooseDirection(Direction direction) {
        switch(direction) {
            case Direction.Up:
                return Vector2.up;
            case Direction.Down:
                return Vector2.down;
            case Direction.Left:
                return Vector2.left;
            case Direction.Right:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    private Quaternion chooseRotation(Direction direction) {
        switch (direction) {
            case Direction.Up:
                return Quaternion.Euler(0, 0, 0);
            case Direction.Down:
                return Quaternion.Euler(0, 0, 180);
            case Direction.Left:
                return Quaternion.Euler(0, 0, 90);
            case Direction.Right:
                return Quaternion.Euler(0, 0, -90);
            default:
                return Quaternion.identity;
        }
    }
}

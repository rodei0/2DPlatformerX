using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class SpawnerBG : MonoBehaviour
{
    [SerializeField] private GameObject thingtoSpawn;
    [SerializeField] private bool moveRight;

    [Tooltip("The object will spawn between randomRangeYDown and randomRangeYUp.")]
    [SerializeField] private float randomRangeYUp;
    [Tooltip("The object will spawn between randomRangeYDown and randomRangeYUp.")]
    [SerializeField] private float randomRangeYDown;

    [Tooltip("The object will spawn randomly in any second between tbs1 and tbs2 (so if u put 2 and 5, it will spawn every 2 to 5 seconds.")]
    [SerializeField] private float timebetweenSpawns1;
    [Tooltip("The object will spawn randomly in any second between tbs1 and tbs2 (so if u put 2 and 5, it will spawn every 2 to 5 seconds.")]
    [SerializeField] private float timebetweenSpawns2;
    void Start()
    {
        StartCoroutine(SpawnThingCorroutine());
    }


    private void SpawnThing() {
        Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(randomRangeYDown, randomRangeYUp), 0f);
        GameObject thingSpawned = Instantiate(thingtoSpawn, spawnPosition, Quaternion.identity);
        MoveTowards moveScript = thingSpawned.GetComponent<MoveTowards>();
        moveScript.Setdir(moveRight);
    }

    private IEnumerator SpawnThingCorroutine() {
        while (true) {
            SpawnThing();
            yield return new WaitForSeconds(Random.Range(timebetweenSpawns1, timebetweenSpawns2));
        }
    }
}

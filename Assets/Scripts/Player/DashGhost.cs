using System.Collections;
using UnityEngine;

public class DashGhost : MonoBehaviour
{

    [SerializeField] SpriteRenderer sr;
    [SerializeField] SpriteRenderer bodySr;
    private bool isSpawning = false;
    [SerializeField] float fadeDuration;
    [SerializeField] float ghostSpawnInterval;

    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnGhosts());
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
    private void GhostSpawn()
    {
        GameObject ghost = new GameObject("DashGhost");
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
        ghost.transform.localScale = transform.localScale;

        SpriteRenderer ghostSR = ghost.AddComponent<SpriteRenderer>();
        ghostSR.sortingLayerName = sr.sortingLayerName;
        ghostSR.sprite = sr.sprite;
        ghostSR.color = bodySr.color;
        ghostSR.enabled = true;
        ghostSR.sortingOrder = sr.sortingOrder - 1;
        ghostSR.flipX = sr.flipX;

        StartCoroutine(FadeOutAndDestroy(ghost));
    }

    private IEnumerator FadeOutAndDestroy(GameObject ghost)
    {
        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.6f, 0f, elapsedTime / fadeDuration);
            ghostSR.color = new Color(ghostSR.color.r, ghostSR.color.g, ghostSR.color.b, alpha);
            yield return null;
        }
        Destroy(ghost);
    }

    private IEnumerator SpawnGhosts()
    {
        while (isSpawning)
        {
            GhostSpawn();
            yield return new WaitForSeconds(ghostSpawnInterval);
        }
    }

}

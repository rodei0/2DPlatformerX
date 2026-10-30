using System.Collections;
using UnityEngine;

public class DestroyAfterXSecs : MonoBehaviour
{
    [SerializeField] private float destroyAfter = 0.5f;

    void Start()
    {
        StartCoroutine(DestroyObject());
    }
    private IEnumerator DestroyObject() {
        yield return new WaitForSeconds(destroyAfter);
        Destroy(this.gameObject);
        yield return null;
    }
}

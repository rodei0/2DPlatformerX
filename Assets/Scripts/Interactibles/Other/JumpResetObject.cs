using System.Collections;
using UnityEngine;

public class JumpResetObject : MonoBehaviour
{
    private bool canUse = true;
    [SerializeField] private float cd;
    private LightOscile lightOscile;
     
    void Awake()
     {
     lightOscile = GetComponent<LightOscile>();
     }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && canUse)
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ResetJump();
                lightOscile.FadelightFunc();
                canUse = false;
                StartCoroutine(ResetUse());
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !canUse)
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null) {
                player.unResetJump();
            }
        }

    }

    private IEnumerator ResetUse() {
        yield return new WaitForSeconds(cd);

        yield return StartCoroutine(lightOscile.FadeInLightFunc());

        canUse = true;
        Debug.Log("Jump reset object is ready to use again.");
    }
}

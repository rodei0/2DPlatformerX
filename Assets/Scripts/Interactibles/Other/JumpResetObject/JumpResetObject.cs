using System.Collections;
using UnityEngine;

public class JumpResetObject : MonoBehaviour
{
    private bool canUse = true;
    [SerializeField] private float cd;
    private LightOscile lightOscile;
     
    private ParticlesEffect particlesEffect;
    StretchOnJump stretchOnJump;

    private void Awake()
    {
        lightOscile = GetComponent<LightOscile>();
        particlesEffect = GetComponent<ParticlesEffect>();
        stretchOnJump = GetComponent<StretchOnJump>();
    }


    private void Jumped() {
        stretchOnJump.StretchCorroutine();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.jumpResetSFX, transform.position);
        lightOscile.FadelightFunc();
        particlesEffect.StopParticles();
        canUse = false;
        StartCoroutine(ResetUse());
        PlayerMovement.OnJumpResetConsumed -= Jumped;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && canUse)
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                PlayerMovement.OnJumpResetConsumed += Jumped;
                player.ResetJump();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null) {
                player.unResetJump();
                PlayerMovement.OnJumpResetConsumed -= Jumped;
            }
        }

    }

    private IEnumerator ResetUse() {
        yield return new WaitForSeconds(cd);

        yield return StartCoroutine(lightOscile.FadeInLightFunc());
        particlesEffect.ResumeParticles();
        canUse = true;
        Debug.Log("Jump reset object is ready to use again.");
    }
}

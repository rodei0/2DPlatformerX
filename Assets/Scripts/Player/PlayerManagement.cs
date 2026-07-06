using System.Collections;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private Transform respawn;
    [SerializeField] private SpriteRenderer srBody;
    [SerializeField] private SpriteRenderer srHead;
    private float defaultGravity;
    public Color actualColor;

    [Header("Player Scripts")]
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private DashGhost dashGhost;
    private PlayerStateList pState;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        dashGhost = GetComponent<DashGhost>();
        pState = GetComponent<PlayerStateList>();
        playerDash = GetComponent<PlayerDash>();
        actualColor = srBody.color;
        defaultGravity = rb.gravityScale;
    }

    private void RespawnPlayer() {
        rb.transform.position = respawn.transform.position;
        rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator PlayerDead() {
        AudioManager.Instance.StopAllSFX();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hittedSFX, transform.position);
        pState.isDead = true;
        playerDash.ResetDashState();
        dashGhost.StopSpawning();

        CameraShake.Instance.Shake(0.10f, 0.5f);

        playerMovement.DisableControls(); // Desactiva temporalmente los controles
        rb.gravityScale = 0f; // Detiene la física
        rb.linearVelocity = Vector2.zero; // Detiene el movimiento
        SetPlayerVisible(false);
        yield return new WaitForSeconds(0.20f);
        SetPlayerVisible(true);
        yield return new WaitForSeconds(0.2f);
        SetPlayerVisible(false);
        yield return new WaitForSeconds(0.2f);
        SetPlayerVisible(true);
        yield return new WaitForSeconds(0.2f);
        SetPlayerVisible(false);
        yield return new WaitForSeconds(0.15f);
        SetPlayerVisible(true);
        rb.gravityScale = defaultGravity;
        playerMovement.EnableControls();
        pState.isDead = false;
        RespawnPlayer();

    }

    private void SetPlayerVisible(bool visible) {
        Color color = visible ? actualColor : Color.clear;
        srBody.color = color;
        srHead.color = color;
    }

    public void SetActualColor(Color color) {
        actualColor = color;
    }
    public void KillPlayer() {
        StartCoroutine(PlayerDead());
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] public Transform respawn;
    [SerializeField] private SpriteRenderer srBody;
    [SerializeField] private SpriteRenderer srHead;
    private Collider2D col;
    private float defaultGravity;
    public Color actualColor;

    public static event System.Action OnPlayerDeath;

    [Header("Player Scripts")]
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private DashGhost dashGhost;
    private PlayerStateList pState;

    [Header("BlackTransition")]
    [SerializeField] private float durationTransition = 0.5f;
    [SerializeField] private Image blackTransition;

    private CheckPoint[] checkpoints;
    void Awake()
    {
        checkpoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);
        FindActualCheckpoint();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        dashGhost = GetComponent<DashGhost>();
        pState = GetComponent<PlayerStateList>();
        playerDash = GetComponent<PlayerDash>();
        actualColor = srBody.color;
        defaultGravity = rb.gravityScale;
        col = GetComponent<Collider2D>();
    }

    private void OnEnable() {
        blackTransition.color = new Color(0, 0, 0, 1);
        StartCoroutine(TransitionBlack());
    }

    public void SetRespawnPoint(Vector3 newRespawnPoint) {
        respawn.position = newRespawnPoint;
    }

    public void FindActualCheckpoint() {
        foreach (CheckPoint checkpoint in checkpoints) {
            if (checkpoint.checkpointName == SaveManager.Instance.Data.lastCheckpoint) {
                respawn.position = checkpoint.transform.position;
                transform.position = checkpoint.transform.position;
                return;
            }
        }
    }

    private IEnumerator PlayerDead() {
        SaveManager.Instance.AddDeathCount();
        AudioManager.Instance.StopAllSFX();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.hittedSFX, transform.position);
        pState.isDead = true;
        playerDash.ResetDashState();
        dashGhost.StopSpawning();

        CameraShake.Instance.Shake(0.10f, 0.5f);

        col.enabled = false;
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

        float t = 0;
        while (t < durationTransition) {
            t += Time.deltaTime;
            blackTransition.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t / durationTransition));
            yield return null;
        }
        OnPlayerDeath?.Invoke();
        RecoverPlayer();
        StartCoroutine(TransitionBlack());

    }

    private IEnumerator TransitionBlack() {
        float t = 0;
        while (t < durationTransition) {
            t += Time.deltaTime;
            blackTransition.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t / durationTransition));
            yield return null;
        }
    }

    private void RecoverPlayer() {
        rb.gravityScale = defaultGravity;
        playerMovement.EnableControls();
        pState.isDead = false;
        rb.transform.position = respawn.transform.position;
        rb.linearVelocity = Vector2.zero;
        col.enabled = true;
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

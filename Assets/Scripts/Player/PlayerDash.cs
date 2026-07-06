using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash variables:")]
    private bool cdOn = true;
    private float originalGravity;
    private float defaultGravity;
    private bool gravityStored = false;
    [SerializeField] private float dashingHorizontalMultiplier = 1f;
    [SerializeField] private float dashingVerticalMultiplier = 0.4f;
    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCd = 0.2f;

    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Script references")]
    private DashGhost dashGhost;
    private PlayerVFX playerVFX;
    private PlayerMovement playerMovement;
    private PlayerStateList pState;

    public Coroutine dashCoroutine;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashGhost = GetComponent<DashGhost>();
        playerVFX = GetComponent<PlayerVFX>();
        playerMovement = GetComponent<PlayerMovement>();
        pState = GetComponent<PlayerStateList>();
        defaultGravity = rb.gravityScale;
    }

    void Update() {
        if (playerMovement.playerControls.Player.Dash.WasPressedThisFrame() && pState.canDash && cdOn) {
            dashCoroutine = StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash() //bro lo de hacer el dash y el reset de dash y que cuando dasheo con el reset de dash se buggea la gravedad q cancer es <- fixed todo bien
{
        if (!gravityStored) {
            originalGravity = rb.gravityScale;
            gravityStored = true;
        }

        float capturedX = DashLeftOrRight();
        float capturedY = DashUpOrDown();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.dashSFX, transform.position);
        pState.isDashing = true;
        dashGhost.StartSpawning();
        playerVFX.CreateDust();
        pState.canDash = false;
        cdOn = false;


        Debug.Log($"Dash START - originalGravity guardada: {originalGravity}, gravityScale actual: {rb.gravityScale}");
        rb.gravityScale = 0f;

        Vector2 rawDir = new Vector2(capturedX * dashingHorizontalMultiplier,
                              capturedY * dashingVerticalMultiplier);

        // Solo normalizar en diagonal para evitar que se sume potencia, pero respetar los multiplicadores en ejes puros
        Vector2 dashDir = (capturedX != 0 && capturedY != 0) ? rawDir.normalized * dashingPower
                                                              : rawDir * dashingPower;

        rb.linearVelocity = dashDir;

        yield return new WaitForSeconds(dashingTime);
        Debug.Log($"Dash END - restaurando gravedad a: {originalGravity}");

        rb.gravityScale = originalGravity;
        gravityStored = false;
        pState.isDashing = false;
        dashGhost.StopSpawning();

        yield return new WaitForSeconds(dashingCd);
        cdOn = true;

        if (pState.isGrounded) {
            pState.canDash = true;
        }
    }

    private void FullDashReset() {
        pState.canDash = true;
        //cdOn = true; //no queremos que el dashreset reinicie el cd ya que ocurren bugs raros de dashear la nada, asi que hacemos que cuando vuelva el cd si se pueda dashear, god
        StartCoroutine(HitStop());
    }

    public void ResetDashState() //funcion que se llamará desde playerManagement
    {
        if (dashCoroutine != null) {
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
        }

        gravityStored = false;
        pState.isDashing = false;
        pState.canDash = true;
        cdOn = true;
    }

    private IEnumerator HitStop() //efecto que relentiza el tiempo al tocar el resetdash para notar mas el impacto y poder dashear mas comfy
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(0.06f);
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(0.04f);
        Time.timeScale = 1f;
    }

    private float DashUpOrDown() {
        Vector2 moveInput = playerMovement.playerControls.Player.Move.ReadValue<Vector2>();
        return moveInput.y;
    }

    private float DashLeftOrRight() {
        Vector2 moveInput = playerMovement.playerControls.Player.Move.ReadValue<Vector2>();
        if (moveInput.x != 0) return moveInput.x;
        if (moveInput.y == 0) return pState.isFacingRight ? 1 : -1;
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("DashReset") && !pState.canDash) {
            DashResetObject dashRobject = collision.GetComponent<DashResetObject>();
            if (dashRobject.canUse) {
                dashRobject.OnEnter();
                FullDashReset();
            }
        }
    }
}


using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Basic variables:")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int maxJump;
    private int jumpCount;
    private bool canDoubleJump;
    private int jumpBufferCounter = 0;
    [SerializeField] private int jumpBufferFrames;
    private float horizontal;
    private bool jumpReleased = false;
    public bool isFacingRight = true;
    private bool isFire = true;


    [Header("Components:")]
    [SerializeField] private SpriteRenderer srBody;
    [SerializeField] private SpriteRenderer srHead;
    private Rigidbody2D rb;
    //Animator animator;
    [SerializeField] private Animator headAnim;
    [SerializeField] private Animator bodyAnim;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    

    [Header("Script References:")]
    PlayerStateList pState;
    public PlayerControls playerControls;
    private DashGhost dashGhost;
    public Coroutine dashCoroutine;
    private PlayerManagement playerManagement;

    [Header("Dash variables:")]
    private bool canDash = true;
    private bool cdOn = true;
    private bool isDashing;
    private float originalGravity;
    private float defaultGravity;
    private bool gravityStored = false;
    [SerializeField] private float dashingHorizontalMultiplier = 1f;
    [SerializeField] private float dashingVerticalMultiplier = 0.4f;
    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCd = 0.2f;
    [SerializeField] private TrailRenderer tr;

    [Header("Particles:")]
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private ParticleSystem dustFuego;
    [SerializeField] private ParticleSystem dustAgua;
    [SerializeField] private ParticleSystem fuego;
    [SerializeField] private ParticleSystem agua;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();
    }

    private void OnEnable() {
        if (playerControls != null)
            playerControls.Enable();
    }

    private void OnDisable() {
        if (playerControls != null)
            playerControls.Disable();
    }

    void Start()
    {
        //sr = GetComponent<SpriteRenderer>(); reemplazado por 2 sprites
        playerManagement = GetComponent<PlayerManagement>();
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        dashGhost = rb.GetComponent<DashGhost>();
        //animator = GetComponent<Animator>(); reemplazado por 2 anims para cabeza y cuerpo, por lo que se asignan manualmente en el inspector
        defaultGravity = rb.gravityScale;
        fuego.Stop();
        agua.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerControls.Player.Jump.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferFrames;
        } 

        // Detect release in Update (frame-based) and store it for FixedUpdate physics handling.
        if (playerControls.Player.Jump.WasReleasedThisFrame())
        {
            jumpReleased = true;
        }

        if (playerControls.Player.Jump.WasPressedThisFrame() && canDoubleJump && !IsGrounded()) {
            JumpDouble();
            canDoubleJump = false;
        }


            if (CameraMovement.isManualMode) return;

        horizontal = Mathf.Round(playerControls.Player.Move.ReadValue<Vector2>().x);

        if (playerControls.Player.Dash.WasPressedThisFrame() && canDash && cdOn)
        {

            dashCoroutine = StartCoroutine(Dash());
        }

        if (playerControls.Player.ChangeElement.WasPressedThisFrame())
        {
            ChangeParticles();
        }


        Flip();


    }

    private void FixedUpdate()
    {
        UpdateJumpVariables();
        Jump();

        if (isDashing)
        {
            return;
        }
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        headAnim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        bodyAnim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        headAnim.SetFloat("yVelocity", rb.linearVelocity.y);
        bodyAnim.SetFloat("yVelocity", rb.linearVelocity.y);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Triggerea");  

        if (collision.CompareTag("Fire"))
        {
            if (!isFire) playerManagement.KillPlayer();
            else StartCoroutine(SpawnFire());
        }

        if (collision.CompareTag("Agua"))
        {
            if (isFire) playerManagement.KillPlayer();
            else StartCoroutine(SpawnWater());
        }

        if (collision.CompareTag("DashReset") && !canDash)
        {
            DashResetObject dashRobject = collision.GetComponent<DashResetObject>();
            if (dashRobject.canUse) {
                dashRobject.OnEnter();
                FullDashReset(); }
        }
    }

    #endregion

    #region Movement Methods
    private void Jump()
    {

        // salto variable, cuando el salto se suelta el personaje va hacia abajo mas rapido
        if (jumpReleased)
        {
            if (rb.linearVelocity.y > 0f && !isDashing)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
            // consume the release event
            jumpReleased = false;
        }
        
        if (!pState.jumping && !isDashing)
        {
            if (jumpBufferCounter > 0 && IsGrounded()) 
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                pState.jumping = true;

                bodyAnim.SetBool("isJumping", true);
                headAnim.SetBool("isJumping", true);
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.jumpSFX, transform.position);
                Debug.Log("Jumping");
                CreateDust();

                jumpBufferCounter = 0;
            }
        }
        /*if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded() && jumpCount < maxJump) //esto es para doble salto (era)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            audioSource.PlayOneShot(salto);
            jumpCount++;
            animator.SetBool("isJumping", !IsGrounded());

        }
        */

        bodyAnim.SetBool("isJumping", !IsGrounded());
        headAnim.SetBool("isJumping", !IsGrounded());
    }

    private void JumpDouble() {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        pState.jumping = true;

        bodyAnim.SetBool("isJumping", true);
        headAnim.SetBool("isJumping", true);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.jumpSFX, transform.position);
        Debug.Log("Jumping");
        CreateDust();

        jumpBufferCounter = 0;
    }

    private void UpdateJumpVariables()
    {
        bool grounded = IsGrounded();
        
        // Detect landing
        if (grounded)
        {
            pState.jumping = false;
            if (!isDashing)
            {
                canDash = true;
            }
        }
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter--;
        }
    }

    public void ResetJump() 
    {
        canDoubleJump = true;
    }

    public void unResetJump()
    {
        canDoubleJump = false;
    }

    public void DisableControls() 
    {
        playerControls.Disable();
    }

    public void EnableControls() 
    {
        playerControls.Enable();
    }
    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || !isFacingRight && horizontal > 0f) 
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    #endregion

    #region Dash Methods

    private IEnumerator Dash() //bro lo de hacer el dash y el reset de dash y que cuando dasheo con el reset de dash se buggea la gravedad q cancer es
    {
        if (!gravityStored)
        {
            originalGravity = rb.gravityScale;
            gravityStored = true;
        }

        float capturedX = DashLeftOrRight();
        float capturedY = DashUpOrDown();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.dashSFX, transform.position);
        isDashing = true;
        dashGhost.StartSpawning();
        CreateDust();
        canDash = false;
        cdOn = false;


        Debug.Log($"Dash START - originalGravity guardada: {originalGravity}, gravityScale actual: {rb.gravityScale}");
        rb.gravityScale = 0f;

        Vector2 rawDir = new Vector2(capturedX * dashingHorizontalMultiplier,
                              capturedY * dashingVerticalMultiplier);

        // Solo normalizar en diagonal para evitar que se sume potencia, 
        // pero respetar los multiplicadores en ejes puros
        Vector2 dashDir = (capturedX != 0 && capturedY != 0) ? rawDir.normalized * dashingPower
                                                              : rawDir * dashingPower;

        rb.linearVelocity = dashDir;
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);
        Debug.Log($"Dash END - restaurando gravedad a: {originalGravity}");

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        gravityStored = false;
        isDashing = false;
        dashGhost.StopSpawning();

        yield return new WaitForSeconds(dashingCd);
        cdOn = true;

        if (IsGrounded())
        {
            canDash = true;
        }
    }

    private void FullDashReset()
    {
        canDash = true;
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
        isDashing = false;
        canDash = true;
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

    private float DashUpOrDown()
    {
        Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();
        return moveInput.y;
    }

    private float DashLeftOrRight()
    {
        Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();
        if (moveInput.x != 0) return moveInput.x;
        if (moveInput.y == 0) return isFacingRight ? 1 : -1;
        return 0;
    }

    #endregion

    #region Particles and Effects


    private IEnumerator SpawnFire()
    {
        dustFuego.Play();
        yield return new WaitForSeconds(0.5f);
        dustFuego.Stop();
    }
    private IEnumerator SpawnWater()
    {
        dustAgua.Play();
        yield return new WaitForSeconds(0.5f);
        dustAgua.Stop();
    }

    void CreateDust()
    {
        dust.Play();
    }


    public IEnumerator ChangeColor(Color color, float duration)
    {
        Color start = srBody.color;
        tr.startColor = color;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            tr.startColor = Color.Lerp(start, color, elapsed / duration);
            srBody.color = Color.Lerp(start, color, elapsed / duration);
            srHead.color = Color.Lerp(start, color, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        srBody.color = color;
        srHead.color = color;
        playerManagement.actualColor = srBody.color;
    }

        

    private void ChangeParticles() //cambio de particulas y de color del personaje para los elementos
    {
        if (isFire == true)
        {
            StartCoroutine(ChangeColor(new Color(0.23f, 0.345f, 0.655f), 0.2f));
            fuego.Stop();
            agua.Play();
        } else
        {
            StartCoroutine(ChangeColor(new Color(0.619f, 0.227f, 0.10f), 0.2f));
            agua.Stop();
            fuego.Play();
        }

        isFire = !isFire;
    }

    #endregion

    //#region Other
    /*private void RespawnPlayer()
    {
        rb.transform.position = respawn.transform.position;
        rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator PlayerDead()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
            dashCoroutine = null;
            tr.emitting = false;
            dashGhost.StopSpawning();
            gravityStored = false;
            isDashing = false;
            canDash = true;
            cdOn = true;
        }
        playerControls.Disable(); // Desactiva temporalmente los controles
        rb.gravityScale = 0f; // Detiene la física
        rb.linearVelocity = Vector2.zero; // Detiene el movimiento
        isDashing = false;
        srBody.color = Color.clear;
        srHead.color = Color.clear;
        yield return new WaitForSeconds(0.20f);
        srBody.color = actualColor;
        srHead.color = actualColor;
        yield return new WaitForSeconds(0.2f);
        srBody.color = Color.clear;
        srHead.color = Color.clear;
        yield return new WaitForSeconds(0.2f);
        srBody.color = actualColor;
        srHead.color = actualColor;
        yield return new WaitForSeconds(0.2f);
        srBody.color = Color.clear;
        srHead.color = Color.clear;
        yield return new WaitForSeconds(0.15f);
        srBody.color = actualColor;
        srHead.color = actualColor;
        rb.gravityScale = defaultGravity;
        playerControls.Enable();
        RespawnPlayer();

    }

    #endregion  */


}

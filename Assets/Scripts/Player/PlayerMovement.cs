using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Basic variables:")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxFallSpeed = 14f;
    //[SerializeField] private int maxJump;
    //private int jumpCount;
    private bool hasExtraJump;

    //buffer jump
    private int jumpBufferCounter = 0;
    [SerializeField] private int jumpBufferFrames;

    [SerializeField] private float coyoteTime = 0.15f;
    private float horizontal;
    private bool jumpReleased = false;
    public bool jumpResetting = false;


    [Header("Components:")]
    private Rigidbody2D rb;
    [SerializeField] private Animator headAnim;
    [SerializeField] private Animator bodyAnim;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;



    [Header("Script References:")]
    private PlayerStateList pState;
    private PlayerVFX playerVFX;
    public PlayerControls playerControls;

    [Header("Footsteps sound")]
    private float footstepTimer = 0f;
    [SerializeField] private float footstepInterval = 0.3f; // Intervalo entre pasos en segundos

    // Events
    public static event System.Action OnJumpResetConsumed;


    #endregion

    #region Unity Methods

    private void Awake() {

        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>(); reemplazado por 2 anims para cabeza y cuerpo, por lo que se asignan manualmente en el inspector
        playerVFX = GetComponent<PlayerVFX>();
    }

    void Start() {
        playerControls = InputManager.Instance.PlayerControls;
    }

    // Update is called once per frame
    void Update() {

        if (playerControls.Player.Jump.WasPressedThisFrame()) {
            if (hasExtraJump && !pState.isGrounded) {
                Debug.Log("Double Jumping");
                JumpDouble();
                hasExtraJump = false;
            }
            else {
                Debug.Log("Jump Button Pressed");
                jumpBufferCounter = jumpBufferFrames;
            }
        }

        // se detecta la liberación del botón de salto para el salto variable
        if (playerControls.Player.Jump.WasReleasedThisFrame()) {
            jumpReleased = true;
        }

       //if (playerControls.Player.Jump.WasPressedThisFrame() && hasExtraJump && !pState.isGrounded) {
       //    JumpDouble();
       //    hasExtraJump = false;
       //}


        if (CameraMovement.isManualMode) return;

        horizontal = Mathf.Round(playerControls.Player.Move.ReadValue<Vector2>().x);

        if (playerControls.Player.ChangeElement.WasPressedThisFrame()) {
            playerVFX.ChangeParticles();
        }

        //sonido de pasos
        FootstepSounds();

        Flip();
        UpdateGroundVariables();
    }

    private void FixedUpdate() {
        //capar la velocidad de caida
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));

        Jump();

        if (pState.isDashing) {
            return;
        }
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        headAnim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        bodyAnim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        headAnim.SetFloat("yVelocity", rb.linearVelocity.y);
        bodyAnim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

   


    #endregion

    #region Movement Methods
    private void Jump() {

        // salto variable, cuando el salto se suelta el personaje va hacia abajo mas rapido
        if (jumpReleased) {
            if (rb.linearVelocity.y > 0f && !pState.isDashing) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
            // consume the release event
            jumpReleased = false;
        }

        if (!pState.isJumping && !pState.isDashing) {
            if (jumpBufferCounter > 0 && pState.coyoteCounter > 0) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                pState.coyoteCounter = 0;
                pState.isJumping = true;

                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.jumpSFX, transform.position);
                Debug.Log("Jumping");
                StartCoroutine(playerVFX.StretchOnJump());
                playerVFX.CreateDust();

                hasExtraJump = false;
                jumpBufferCounter = 0;
            }
        }
        bodyAnim.SetBool("isJumping", !pState.isGrounded);
        headAnim.SetBool("isJumping", !pState.isGrounded);

        /*
         if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded() && jumpCount < maxJump) //esto es para doble salto (era)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            audioSource.PlayOneShot(salto);
            jumpCount++;
            animator.SetBool("isJumping", !IsGrounded());

        }
        */

    }

    private void JumpDouble() {
        OnJumpResetConsumed?.Invoke();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        pState.isJumping = true;
        

        bodyAnim.SetBool("isJumping", true);
        headAnim.SetBool("isJumping", true);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.jumpSFX, transform.position);
        Debug.Log("Jumping");
        playerVFX.CreateDust();

        jumpBufferCounter = 0;
    }

    private void UpdateGroundVariables() {
        pState.isGrounded = IsGrounded();

        // Detect landing
        if (pState.isGrounded && pState.wasinAir) {
            playerVFX.CreateDust();
            StartCoroutine(playerVFX.SquashCharacter());
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.fallTouchFloor, transform.position);
        }
        if (pState.isGrounded) {
            pState.isJumping = false;
            pState.coyoteCounter = coyoteTime;
            pState.wasinAir = false;
            if (!pState.isDashing) {
                pState.canDash = true;
            }
        } else { 
            pState.coyoteCounter -= Time.fixedDeltaTime;
            pState.wasinAir = true;
        }
        if (jumpBufferCounter > 0) { jumpBufferCounter--;}
        
        
    }

    public void ResetJump() {
        hasExtraJump = true;
    }

    public void unResetJump() {
        hasExtraJump = false;
    }

    public void DisableControls() {
        playerControls.Disable();
    }

    public void EnableControls() {
        playerControls.Enable();
    }
    private void Flip() {
        if ((pState.isFacingRight && horizontal < 0f) || !pState.isFacingRight && horizontal > 0f) {
            pState.isFacingRight = !pState.isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private bool IsGrounded() {
        if (rb.linearVelocity.y > 0.1f)
            return false;

        return Physics2D.OverlapCircle(groundCheck.position, 0.10f, groundLayer);
    }

    private void FootstepSounds() {
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f && IsGrounded() && !pState.isDashing) {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval) {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.footstepsSFX, transform.position);
                footstepTimer = 0f;
            }
        }
        else {
            footstepTimer = footstepInterval;
        }
    }
}

    #endregion

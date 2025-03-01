using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // basic var
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpCount;
    [SerializeField] private int maxJump;
    private bool isFacingRight = true;
    private Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private Transform respawn;
    private float horizontal;
    //bool isGrounded;

    private bool canDash = true;
    private bool cdOn = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCd = 1f;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private ParticleSystem dustFuego;
    [SerializeField] private ParticleSystem dustAgua;
    [SerializeField] private ParticleSystem fuego;
    [SerializeField] private ParticleSystem agua;
    private bool isFire = true;
    //audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip salto;
    //animaciones
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        fuego.Stop();
        agua.Stop();
    }

    // Update is called once per frame
    void Update()
    {

        if (isDashing)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeParticles();
            }
            return;

        }

        if (IsGrounded())
        {
            jumpCount = 0;
            canDash = true;
            if (rb.linearVelocity.y <= 0f) // Solo cambiar a false si está cayendo o en el suelo
            {
                animator.SetBool("isJumping", false);
            }
        }

        if (CameraMovement.isManualMode) return;
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && jumpCount < maxJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
            audioSource.PlayOneShot(salto);
            CreateDust();
           
        }

        if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded() && jumpCount < maxJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            audioSource.PlayOneShot(salto);
            jumpCount++;
            animator.SetBool("isJumping", !IsGrounded());

        }
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f )  //Codigo para q salte mas si lo mantienes
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); 
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && cdOn)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeParticles();
        }

        flip();
        /*
        if (isGrounded)
        {
            Debug.Log("Estoy en el suelo");
        }
        else 
            {
                Debug.Log("no estoy en el suelo");
            }
        */



    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void flip()
    {
        if ((isFacingRight && horizontal < 0f) || !isFacingRight && horizontal > 0f) 
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Triggerea");  

        if (collision.CompareTag("Fire"))
        {
            if (!isFire) RespawnPlayer();
            else StartCoroutine(SpawnFire());
        }

        if (collision.CompareTag("Agua"))
        {
            if (isFire) RespawnPlayer();
            else StartCoroutine(SpawnWater());
        }
    }

    private void RespawnPlayer()
    {
        rb.transform.position = respawn.transform.position;
        rb.linearVelocity = Vector2.zero;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private IEnumerator Dash()
    {
        CreateDust();
        canDash = false;
        cdOn = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCd);
        cdOn = true;
    }

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

    private void ChangeParticles()
    {
        if (isFire == true)
        {
            fuego.Stop();
            agua.Play();
        } else
        {
            agua.Stop();
            fuego.Play();
        }

        isFire = !isFire;
    }

    


}

using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
  public bool isJumping = false;
  public bool isFire = true;
  public bool isDead = false;
  public bool isGrounded = false;
  public bool isFacingRight = true; 
  public bool isDashing = false;
  public bool canDash = true;
  public bool wasinAir = false;

  public float coyoteCounter = 0f;
}

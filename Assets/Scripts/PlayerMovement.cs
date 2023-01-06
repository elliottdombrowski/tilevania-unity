using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  Vector2 moveInput;
  Rigidbody2D rb;
  Animator anim;
  CapsuleCollider2D cc;

  public float runSpeed = 5f;
  public float jumpSpeed = 16f;
  public float climbSpeed = 5f;
  float gravityScaleAtStart;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    cc = GetComponent<CapsuleCollider2D>();
    gravityScaleAtStart = rb.gravityScale;
  }

  void Update()
  {
    Run();
    FlipSprite();
    ClimbLadder();
  }

  void OnMove(InputValue value)
  {
    moveInput = value.Get<Vector2>();
  }

  void OnJump(InputValue value)
  {
    if (!cc.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

    if (value.isPressed)
    {
      rb.velocity += new Vector2(0f, jumpSpeed);
    }
  }

  void Run()
  {
    Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y); // KEEP CURRENT VELOCITY ON Y
    rb.velocity = playerVelocity;

    // HANDLE ANIMATIONS
    bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
    anim.SetBool("isRunning", playerHasHorizontalSpeed);
  }

  void FlipSprite()
  {
    // GET ABSOLUTE VALUE OF X - TRUE IF GREATER THAN EPSILON - AKA GREATER THAN 0 TO A FINE DECIMAL LEVEL.
    bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

    if (playerHasHorizontalSpeed)
    {
      transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }
  }

  void ClimbLadder()
  {
    if (!cc.IsTouchingLayers(LayerMask.GetMask("Climb")))
    {
      rb.gravityScale = gravityScaleAtStart;
      anim.SetBool("isClimbing", false); // SET BACK TO FALSE TO AVOID FREEZING ON CLIMB ANIM
      return;
    }

    Vector2 climbVelocity = new Vector2(rb.velocity.x, moveInput.y * climbSpeed);
    rb.velocity = climbVelocity;
    rb.gravityScale = 0f;

    bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
    anim.SetBool("isClimbing", playerHasVerticalSpeed);
    // anim.enabled = playerHasVerticalSpeed; // ENABLE / DISABLE ANIMATION BASED ON IF PLAYER IS MOVING
  }
}

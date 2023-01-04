using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  Vector2 moveInput;
  Rigidbody2D rb;

  public float runSpeed = 5f;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    Run();
    FlipSprite();
  }

  void OnMove(InputValue value)
  {
    moveInput = value.Get<Vector2>();
    Debug.Log(moveInput);
  }

  void Run()
  {
    Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, rb.velocity.y); // KEEP CURRENT VELOCITY ON Y
    rb.velocity = playerVelocity;
  }

  void FlipSprite()
  {
    // GET ABSOLUTE VALUE OF X - TRUE IF GREATER THAN EPSILON - AKA GREATER THAN 0 TO A FINE DECIMAL LEVEL.
    bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

    if (playerHasHorizontalSpeed)
    {
      transform.localScale = new Vector2 (Mathf.Sign(rb.velocity.x), 1f);
    }
  }
}

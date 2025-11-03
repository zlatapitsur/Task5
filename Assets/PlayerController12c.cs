using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController12c : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 16f;

    private float horizontal;
    private bool isFacingRight = true;
    private bool doubleJump = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    void Update()
    {
        // Movement input
        horizontal = Input.GetAxisRaw("Horizontal");

        // Sprint
        bool isSprint = Input.GetKey(KeyCode.LeftShift);

        // Reset double jump on ground
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || !doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                
                // Trigger animations
                if (IsGrounded()) 
                    animator.SetTrigger("IsJump");
                else 
                    animator.SetTrigger("DoubleJ");

                doubleJump = !doubleJump;
            }
        }

        Flip(horizontal);

        // Animator Params
        animator.SetBool("isRun", isSprint && horizontal != 0);
        animator.SetFloat("IsMove", Mathf.Abs(horizontal));
        animator.SetBool("IsGrounded", IsGrounded());
    }

    private void FixedUpdate()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip(float horizontal)
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }
}

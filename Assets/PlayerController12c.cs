using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Definiuje jaki inny komponenent jest obowi?zkowy dla poprawnego dzia?ania tego skryptu
//1. Nie b?dzie mo?liwo?ci usun?? ten komponent z gameObject'u
//2. Je?li nie by? dodany r?cznie, to b?dzie dodany automatycznie
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController12c : MonoBehaviour
{
    public float moveSpeed = 5;
    public float runSpeed = 7;
    public float jumpForce = 100;
    public int maxJumps = 2;

    private Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private bool isSprint = false;
    private float moveVector = 0;

    private int jumpCount;
    private bool jumpQueued = false;

    public GroundChecker groundChecker;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        moveVector = Input.GetAxis("Horizontal");

        // Sprint
        isSprint = Input.GetKey(KeyCode.LeftShift);

        // Double Jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (groundChecker.isGrounded || jumpCount < maxJumps)
            {
                jumpQueued = true;

                if (jumpCount == 0) anim.SetTrigger("IsJump");       // перший стрибок
                else anim.SetTrigger("DoubleJump");                    // другий стрибок
            }
        }

        // Flip sprite
        if (moveVector < 0) spriteRenderer.flipX = true;
        else if (moveVector > 0) spriteRenderer.flipX = false;

        // Animator params
        anim.SetFloat("IsMove", Mathf.Abs(moveVector));
        anim.SetBool("isRun", isSprint && moveVector != 0);
        anim.SetBool("IsGrounded", groundChecker.isGrounded);
    }

    private void FixedUpdate()
    {
        // Movement
        if (isSprint)
            rb.velocity = new Vector2(moveVector * runSpeed * Time.fixedDeltaTime, rb.velocity.y);
        else
            rb.velocity = new Vector2(moveVector * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);

        // Reset jumps when grounded
        if (groundChecker.isGrounded)
        {
            jumpCount = 0;
        }

        // Perform jump
        if (jumpQueued)
        {
            rb.AddForce(Vector2.up * jumpForce);
            jumpQueued = false;
            jumpCount++;
        }
    }
}

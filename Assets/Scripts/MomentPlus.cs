using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Definiuje jaki inny komponenent jest obowi?zkowy dla poprawnego dzia?ania tego skryptu
//1. Nie b?dzie mo?liwo?ci usun?? ten komponent z gameObject'u
//2. Je?li nie by? dodany r?cznie, to b?dzie dodany automatycznie

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float runSpeed = 7;
    public float jumpForce = 100;

    private Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    //Pomocnicze w?asciwo?ci do przekazania warto?ci input z metody Update do metody FixedUpdate
    private bool isSprint = false;
    private float moveVector = 0;
    private bool isJumping = false;

    public GroundChecker groundChecker;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }


    void Update()
    {      
        moveVector = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift)) { isSprint = true; }
        if (Input.GetKeyDown(KeyCode.Space) && groundChecker.isGrounded)
        {
            isJumping = true;
        }

        if (moveVector != 0)
        {
            anim.SetFloat("IsMove", 1);
        }
        else
        {
            anim.SetFloat("IsMove", -1);
        }

        if (moveVector < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        if (isJumping)
        {
            anim.SetBool("IsJump", true);
        }
        else
        {
            anim.SetBool("IsJump", false);
        }
        if (!groundChecker.isGrounded) {
            anim.SetBool("IsGrounded", false);
        }
        else
        {
            anim.SetBool("IsGrounded", true);
        }

    }
    private void FixedUpdate()
    {
        if (isSprint)
        {
            rb.velocity = new Vector2(moveVector * runSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveVector * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        if (isJumping)
        {
            rb.AddForce(Vector2.up * jumpForce);
            isJumping = false;
        }    
    }
}
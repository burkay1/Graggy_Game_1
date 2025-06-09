using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private float xInput;
    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;

    [Header("GroundCeck")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Dash")]
    private bool ableToDash = true;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;
    
    

    private int facingDirection = 1;
    private bool facingLeft = true;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    
    }


    void Update()
    {
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        xInput = Input.GetAxisRaw("Horizontal");   

        CheckImput();
        FlipController();

    }

    private void Dash()
    {


        if (Input.GetKeyDown(KeyCode.LeftShift) && ableToDash && dashCooldownTimer < 0)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

        if (dashTime > 0)
        {
            rb.velocity = new Vector2(xInput * dashForce, 0);
        }
    }

    private void CheckImput()
    {

        Movement();
        Dash();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingLeft = !facingLeft;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (rb.velocity.x < 0 && !facingLeft)
        {
            Flip();
        }

        else if (rb.velocity.x > 0 && facingLeft)
        {
            Flip();
        }
    }
}

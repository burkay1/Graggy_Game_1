    using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerMovement : MonoBehaviour
{
    private float xInput;

    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    private bool canMove = true;
    private Vector2 velocity;
    
    //to increase gravity when falling
    [Header("CustomGravity")]
    [SerializeField] private float gravity = -50f;
    [SerializeField] private float fallGravityMultiplier = 2.5f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 20f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    public bool isJumping = false;

    [Header("GroundCeck")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Dash")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;
    private bool canDash = true;
    
    

    private int facingDirection = 1;
    private bool facingLeft = true;



    void Start()
    {
        
    
    }


    void Update()
    {
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        xInput = Input.GetAxisRaw("Horizontal");   

        CheckImput();
        FlipController();

        //cut jump if space is relesed early
        if(Input.GetKeyUp(KeyCode.Space)){
            if(velocity.y < 0){
                velocity.y *= jumpCutMultiplier;
            }
        }

        //stop falling after reaching the ground
        if(isGrounded && velocity.y < 0){
            velocity.y = 0;
            isJumping = false;
        }

    }

    private void Dash()
    {


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashCooldownTimer < 0)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        if (!canMove) return;
       
        velocity.x = xInput * moveSpeed;

        //calculate & apply gravity if we are mid air
        if(!isGrounded){
            float appliedGravity = gravity;
            //more gravity while falling
            if (velocity.y < 0) appliedGravity *= fallGravityMultiplier;
            
            velocity.y += appliedGravity * Time.deltaTime;
        }

        //update position
        transform.position += (Vector3)(velocity * Time.deltaTime);
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
            velocity.y = jumpHeight;
            isJumping = true;
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
        if (xInput < 0 && !facingLeft)
        {
            Flip();
        }

        else if (xInput > 0 && facingLeft)
        {
            Flip();
        }
    }
}

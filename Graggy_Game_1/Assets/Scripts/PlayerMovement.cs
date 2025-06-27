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
        private BoxCollider2D collider;
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
        [SerializeField] private float jumpCutMultiplier;
        private bool isJumping = false;
        private bool canJump = true;

        [Header("GroundCeck")]
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private LayerMask whatIsGround;
        private bool isGrounded;

        [Header("Wall & Ceiling Check")]
        [SerializeField] private float ceilingCheckDistance = 0.1f;
        [SerializeField] private float wallCheckDistance = 0.1f;
        private bool isTouchingCeiling;
        private bool isTouchingLeftWall;
        private bool isTouchingRightWall;

        [Header("Dash")]
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashTime;
        [SerializeField] private float dashForce;
        [SerializeField] private float dashCooldown;
        private float dashCooldownTimer;
        private bool canDash = true;
        
        [Header("Coyote Time")]
        [SerializeField] private float coyoteTime = 0.1f;
        private float coyoteTimeCounter;
        
        [Header("Jump Buffer")]
        [SerializeField] float jumpBufferTime = 0.1f;
        private float jumpBufferCounter;


        private int facingDirection = 1;
        private bool facingLeft = true;



        void Start()
        {
        collider = GetComponent<BoxCollider2D>();
        }


        void Update()
        {
            dashTime -= Time.deltaTime;
            dashCooldownTimer -= Time.deltaTime;

            //check if grounded
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);

            xInput = Input.GetAxisRaw("Horizontal");

            CheckInput();
            FlipController();

            //check if touching ceiling
            isTouchingCeiling = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);
            if (isTouchingCeiling && velocity.y > 0f)
            {
                velocity.y = 0f; //stop going up if you bump your head upwards
            }
        //check if touching wall on the right or left (same as ground check)
            Vector2 originRight = (Vector2)transform.position + Vector2.right * (collider.size.x / 2f + 0.01f); 
            Vector2 originLeft = (Vector2)transform.position + Vector2.left * (collider.size.x / 2f + 0.01f);
            isTouchingRightWall = Physics2D.Raycast(originRight, Vector2.right, wallCheckDistance, whatIsGround);
            if (isTouchingRightWall && velocity.x > 0f)
            {
                velocity.x = 0f;    
            }

            isTouchingLeftWall = Physics2D.Raycast(originLeft, Vector2.left, wallCheckDistance, whatIsGround);
            if (isTouchingLeftWall && velocity.x < 0f)
            {
                velocity.x = 0f;    
            }

            if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
            


            //cut jump if space is relesed early
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (velocity.y > 0)
            {
                velocity.y *= jumpCutMultiplier;
            }
        }

            //stop falling after reaching the ground
            if(isGrounded && velocity.y < 0){
                velocity.y = 0;
                isJumping = false;
                canJump = true; //enable jump for afterwards
            }

            //coyote time for jumping again and again flawlessly
            if(isGrounded){
                coyoteTimeCounter = coyoteTime;
            }
            else{
                coyoteTimeCounter -= Time.deltaTime;
            }
            Jump();
        }

        private void Dash()
        {
           //dash function will be filled later
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

        private void CheckInput()
        {
            Movement();
            Dash();
        }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.color = Color.red;
        if (collider != null)
{
            Vector2 originRight = (Vector2)transform.position + Vector2.right * (collider.size.x / 2f + 0.01f);
            Vector2 originLeft = (Vector2)transform.position + Vector2.left * (collider.size.x / 2f + 0.01f);

            Gizmos.DrawLine(originRight, originRight + Vector2.right * wallCheckDistance);
            Gizmos.DrawLine(originLeft, originLeft + Vector2.left * wallCheckDistance);
}
    }

        private void Jump()
        {
            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && canJump)
            {
                velocity.y = jumpHeight;
                isJumping = true;

                //reset jumping counters
                canJump = false;
                coyoteTimeCounter = 0f; 
                jumpBufferCounter = 0f;
            }
        }

        private void Flip()
        {
            facingDirection *= -1;
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

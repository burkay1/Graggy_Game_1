<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
=======
        using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Security.Cryptography;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.UIElements.Experimental;
>>>>>>> parent of fdde6bd (karakteri tilemaple çarpıştıramıyorum amk)

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
        [SerializeField] private float jumpCutMultiplier;
        private bool isJumping = false;
        private bool canJump = true;

        [Header("GroundCeck")]
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private LayerMask whatIsGround;
        private bool isGrounded;

<<<<<<< HEAD
        [Header("Wall & Ceiling Check")]
        [SerializeField] private float ceilingCheckDistance = 0.1f;
        [SerializeField] private float wallCheckDistance = 0.1f;

=======
>>>>>>> parent of fdde6bd (karakteri tilemaple çarpıştıramıyorum amk)
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
            
        
        }


        void Update()
        {
            dashTime -= Time.deltaTime;
            dashCooldownTimer -= Time.deltaTime;

<<<<<<< HEAD
            //check if grounded
            Vector2 leftOrigin = (Vector2)collider.bounds.center + Vector2.down * collider.bounds.extents.y;
            leftOrigin.x -= collider.bounds.extents.x / 2f;

            Vector2 rightOrigin = (Vector2)collider.bounds.center + Vector2.down * collider.bounds.extents.y;
            rightOrigin.x += collider.bounds.extents.x / 2f;

            bool leftGrounded = Physics2D.Raycast(leftOrigin, Vector2.down, groundCheckDistance, whatIsGround);
            bool rightGrounded = Physics2D.Raycast(rightOrigin, Vector2.down, groundCheckDistance, whatIsGround);

            isGrounded = leftGrounded || rightGrounded;
=======
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
            xInput = Input.GetAxisRaw("Horizontal");   
>>>>>>> parent of fdde6bd (karakteri tilemaple çarpıştıramıyorum amk)

            if(Input.GetKeyDown(KeyCode.Space)){
                jumpBufferCounter = jumpBufferTime;
            }
            else{
                jumpBufferCounter -= Time.deltaTime;
            }

            CheckInput();
            FlipController();

<<<<<<< HEAD

            if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

=======
>>>>>>> parent of fdde6bd (karakteri tilemaple çarpıştıramıyorum amk)
            //cut jump if space is relesed early
            if(Input.GetKeyUp(KeyCode.Space)){
                if(velocity.y > 0){
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
        if (!isGrounded)
        {
            float appliedGravity = gravity;
            //more gravity while falling
            if (velocity.y < 0) appliedGravity *= fallGravityMultiplier;

            velocity.y += appliedGravity * Time.deltaTime;
        }

        MoveAndCollide(); //hareketten sonra çarpışmalar kontrol edildiği için hep bir hareket oluyordu,
                        //bunun için çarpışma kontrolünü hareketle tek fonksiyonda yapmam gerekti
        }

    private void MoveAndCollide()
    {
        Vector2 newPosition = transform.position;

        //x ekseninde hareket
        if (velocity.x != 0)
        {
            //yönü hız değerinin pozitif/negatifliğinden anla ve başlangıç koordinatını al
            Vector2 direction = Vector2.right * Mathf.Sign(velocity.x);
            Vector2 origin = (Vector2)collider.bounds.center + direction * (collider.bounds.extents.x + 0.01f);

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Abs(velocity.x * Time.deltaTime), whatIsGround);
            if (hit.collider == null)
            {
                newPosition.x += velocity.x * Time.deltaTime;
            }
            else
            {
                float offset = collider.bounds.extents.x;
                if (velocity.x > 0) newPosition.x = hit.point.x - offset;
                else if (velocity.x < 0) newPosition.x = hit.point.x + offset;

                velocity.x = 0;
            }

        }

        //y eksenide hareket
        if (velocity.y != 0)
        {
            Vector2 direction = Vector2.up * Mathf.Sign(velocity.y);
            Vector2 origin = (Vector2)transform.position + direction * (collider.size.y / 2f + collider.offset.y + 0.01f);

            float rayLength = Mathf.Max(Mathf.Abs(velocity.y * Time.deltaTime), 0.1f);
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength, whatIsGround);

            if (hit.collider == null)
            {
                 newPosition.y += velocity.y * Time.deltaTime;
            }
            else
            {
                float offset = collider.bounds.extents.y;
                if (velocity.y > 0)
                    newPosition.y = hit.point.y - offset;
                else
                    newPosition.y = hit.point.y + offset;
                    velocity.y = 0;
            }
        }
        transform.position = newPosition;       
    }
        

        private void CheckInput()
    {
        Movement();
        Dash();
    }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private Agent agent;
    private SpriteRenderer spriteRenderer;
    private new BoxCollider2D collider;
    [SerializeField] private LayerMask jumpableGround;
    private Vector2 movementInput;
    

    [Header("Wall Jump Sysytem")]
    public Transform wallCheck;
    bool isWallToutch;
    bool isSliding;
    public float wallSlidingSpeed;
    public float wallJumpDuration;
    public Vector2 wallJumpForce;
    bool wallJumping;


    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;  
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jumping, falling}
    [SerializeField]
    public InputActionReference movement, jump;

    private void OnEnable()
    {
        jump.action.performed += OnJump;
        jump.action.Enable();
    }
    private void OnDisable()
    {
        jump.action.performed -= OnJump;
        jump.action.Disable();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        // Call your jump function here.
        Jump();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        agent = GetComponent<Agent>();



    }

    // Update is called once per frame
    private void Update()
    {
        
        
        agent.wallCheck = wallCheck;

 

        agent.MovementInput = movement.action.ReadValue<Vector2>();
        movementInput = movement.action.ReadValue<Vector2>();

        
        isWallToutch = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.92f, 1.39f), 0, jumpableGround);
        
        
        if (isWallToutch && !IsGrounded() && movementInput.x != 0)
        {
            agent.wallSlidingSpeed = wallSlidingSpeed;
            isSliding = true;
        }
        else
        {
            agent.wallSlidingSpeed = 0;
            isSliding = false;
        }

        

    }



    private void Jump()
    {
        
        if (IsGrounded())
        {
            agent.ApplyForce(new Vector2(0f,jumpForce));
        }
        else if (isSliding)
        {
            if (movementInput.x > 0)
            {
                agent.wallJumoForce = wallJumpForce;
            }
            else if (movementInput.x < 0)
            {
                agent.wallJumoForce = new Vector2(-wallJumpForce.x, wallJumpForce.y);
            }

            isSliding = false;
            
            Invoke("StopWallJumping", wallJumpDuration);
        }
        
    }

    private void StopWallJumping()
    {
        agent.wallJumoForce = Vector2.zero;
        
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }


}

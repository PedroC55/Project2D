using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        
        if (jump.action.IsPressed())
        {
            Jump();
        }
        else
        {
            agent.jumpForce = 0;
        }

        if (isWallToutch && !IsGrounded() && movementInput.x != 0)
        {
            //Debug.Log("Estamos em slide");
            agent.wallSlidingSpeed = wallSlidingSpeed;
            isSliding = true;
        }
        else
        {
            agent.wallSlidingSpeed = 0;
            isSliding = false;
        }
        if (wallJumping && isSliding)
        {
            Debug.Log("Devia dar para saltar nas paredes");
            agent.wallJumoForce = wallJumpForce;
        }

    }



    private void Jump()
    {
        if (IsGrounded())
        {
            agent.jumpForce = jumpForce;
        }
        else if (isSliding)
        {
            wallJumping = true;
            Invoke("StopWallJumping", wallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        wallJumping = false;
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Agent agent;
    private ParrySystem playerParrySystem;
    private SpriteRenderer spriteRenderer;
    private new BoxCollider2D collider;
    [SerializeField] private LayerMask jumpableGround;
    public Vector2 movementInput;
    

    [Header("Wall Jump Sysytem")]
    public Transform wallCheck;
    bool isWallToutch;
    bool isSliding;
    public float wallSlidingSpeed;
    public float wallJumpDuration;
    public Vector2 wallJumpForce;

    [Header("Dash Sysytem")]
    private bool canDash = true;
    private bool isDashing;
    private readonly float dashingPowerX = 24f;
    private readonly float dashingPowerY = 17f;
    private readonly float dashingTime = 0.2f;
    private readonly float dashingCooldown = 1f;
    [SerializeField] TrailRenderer trailRenderer;

    [SerializeField] private float moveSpeed = 7f;  
    [SerializeField] private float jumpForce = 14f;

    private bool canMove = true;

    private enum MovementState { idle, running, jumping, falling}
    public InputActionReference movement, jump, dash;

    private void OnEnable()
    {
        playerParrySystem = GetComponent<ParrySystem>();
        jump.action.Enable();
        jump.action.performed += OnJump;

        HitEvent.OnHit += OnPlayerHit;
    }
    private void OnDisable()
    {
        jump.action.Disable();
        jump.action.performed -= OnJump;

        HitEvent.OnHit -= OnPlayerHit;
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        // Call your jump function here.
        Jump();
    }

    private void OnPlayerHit(int damage, GameObject sender, GameObject receiver)
    {
        if (receiver.CompareTag("Player"))
        {
            if (sender.CompareTag("Trap"))
            {
				agent.GetHit(damage, sender);
			}
			else if (sender.CompareTag("Enemy"))
            {
				if (playerParrySystem.CheckParryTiming())
				{
                    ParryEvent.Parry(1, sender.transform.parent.gameObject);
					Debug.Log("Parry");
				}
				else
				{
					agent.GetHit(damage, sender);
				}
			}
            
        }
        
    }

    public IEnumerator DisableMovementDuringParry()
    {
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        agent = GetComponent<Agent>();


        //GetHitEvenet.onGetHit += GetHitEvent;
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (!canMove) {
            agent.MovementInput = Vector2.zero;
            rigidbody.drag = 10f;
            return;
        }

        agent.wallCheck = wallCheck;
        movementInput = movement.action.ReadValue<Vector2>();
            
        if (isDashing)
        {
            return;
        }

        Movement();

        isWallToutch = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.92f, 1.39f), 0, jumpableGround);

        if (dash.action.triggered && canDash)
        {

            StartCoroutine(Dash());
        }
        Slide();
    }

    private void Slide()
    {
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
    private void Movement()
    {
        if (IsGrounded())
        {
            if (movementInput.x != 0f)
            {
                agent.MovementInput = movementInput;
                rigidbody.drag = 0f;
            }
            else
            {
                agent.MovementInput = Vector2.zero;
                rigidbody.drag = 10f;
            }
        }
        else
        {
            agent.MovementInput = movementInput;
            rigidbody.drag = 0f;
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
                agent.wallJumpForce = wallJumpForce;
            }
            else if (movementInput.x < 0)
            {
                agent.wallJumpForce = new Vector2(-wallJumpForce.x, wallJumpForce.y);
            }

            isSliding = false;
            
            Invoke("StopWallJumping", wallJumpDuration);
        }
        
    }

    private IEnumerator Dash()
    {
        if (movementInput.y != 0f && movementInput.x == 0)
        {
            yield break;
        }
        canDash = false;
        isDashing = true;
        float originalGravity = rigidbody.gravityScale;
        rigidbody.gravityScale = 0f;
        agent.Dash(dashingPowerX,dashingPowerY, movementInput);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        rigidbody.gravityScale = originalGravity;
        agent.ResetDash();
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    
    private void StopWallJumping()
    {
        agent.wallJumpForce = Vector2.zero;
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Slow Goo"))
		{
			int slow = collision.GetComponent<SlowGoo>().slowPercentage;
			agent.SlowMovement(slow);
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Slow Goo"))
		{
			// Restaura a velocidade
			agent.SlowMovement(0);
		}
	}


}

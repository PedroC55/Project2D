using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    public Rigidbody2D playerRb2d;
    private Agent agent;
    private Dash dashComp;
    private PlayerAttack attackComp;
    private WallJump wallJumpComp;
    private ParrySystem playerParrySystem;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D playerCollider;
    [SerializeField] private LayerMask jumpableGround;

    public Vector2 movementInput;

    public Transform wallCheck;
    bool isWallToutch;
    bool isSliding;
    public float wallSlidingSpeed;

    // Attack
    public bool attacking = false;

    public bool canDash = true;

    [SerializeField] private float jumpForce = 14f;

    private float lastJumpTime;
    private float lastGroundedTime;
    public float jumpBufferTime;
    public float jumpCoyoteTime;
    

    private bool canMove = true;
    private float jumpCut = -0.2f;

    private enum MovementState { idle, running, jumping, falling}
    public InputActionReference movement, jump, dash, attack;

    private void OnEnable()
    {
        playerParrySystem = GetComponent<ParrySystem>();

        HitEvent.OnHit += OnPlayerHit;
    }
    private void OnDisable()
    {
        HitEvent.OnHit -= OnPlayerHit;
    }

    void Start()
    {
		playerRb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		playerCollider = GetComponent<BoxCollider2D>();
        agent = GetComponent<Agent>();
        dashComp = GetComponent<Dash>();
        wallJumpComp = GetComponent<WallJump>();
        attackComp = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    private void Update()
    {
        lastJumpTime -= Time.deltaTime;
        lastGroundedTime = IsGrounded() ? jumpCoyoteTime : lastGroundedTime -Time.deltaTime;

        if (!canMove)
        {
            agent.MovementInput = Vector2.zero;
			playerRb2d.drag = 10f;
            return;
        }


        agent.wallCheck = wallCheck;
        movementInput = movement.action.ReadValue<Vector2>();

        Movement();


        if (jump.action.triggered)
        {
            lastJumpTime = jumpBufferTime;
            Jump();
            jump.action.canceled += context => OnJumpUp();
        }

        if (attack.action.triggered)
        {
            attackComp.Attack();
        }
        if (attacking)
        {
            attackComp.ResetAttack();
        }
        //Da pra mudar isso para um metodo igual a IsGrounded
        //Al�m disso, da pra tirar o objecto filho "wallCheck" e fazer igual fazemos no metodo IsGrounded, onde faz um box cast na dire��o que o player estiver olhando
        isWallToutch = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.92f, 1.39f), 0, jumpableGround);

        if (dashComp && dash.action.triggered && canDash)
        {
            dashComp.StartDash();

        }
        if(wallJumpComp)
            Slide();
    }


    private void OnPlayerHit(int damage, GameObject sender, GameObject receiver)
    {
        if (receiver.CompareTag("Player"))
        {
            if (sender.CompareTag("Trap"))
            {
				agent.GetHit(damage, sender);
				CanvasEvent.UpdateHealth(0);
			}
			else if (sender.CompareTag("Enemy"))
            {
                
				if (playerParrySystem.CheckParryTiming() && playerParrySystem.CheckDirection(sender))
				{
                    ParryEvent.Parry(1, sender.transform.parent.gameObject);
					Debug.Log("Parry");
				}
				else
				{
					int health = agent.GetHit(damage, sender);
                    CanvasEvent.UpdateHealth(health);
				}
			}
        }
    }

    public IEnumerator DisableMovementDuringParry()
    {
        canMove = false;
        agent.ParryAnimation(); 
		yield return new WaitForSeconds(0.5f);
        canMove = true;
    }

    private void Slide()
    {
        //Se transformar isWallToutch em um metodo � s� chamar aqui igual ta fazendo com IsGrounded
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
                playerRb2d.drag = 0f;
            }
            else
            {
				//N�o precisa fazer isso de colocar 'agent.MovementInput' igual a 0, pois o 'movementInput' ja vai receber 0 caso o player n�o esteja preciosando o bot�o
				agent.MovementInput = Vector2.zero;
                playerRb2d.drag = 10f;
            }
        }
        else
        {
            agent.MovementInput = movementInput;
            playerRb2d.drag = 0f;
        }
    }

    private void OnJumpUp()
    {
        if(playerRb2d.velocity.y > 0)
        {
            agent.ApplyForce(Vector2.down * playerRb2d.velocity.y * ( 1 - jumpCut));

        }
    }
    private void Jump()
    {
        if ((lastGroundedTime > 0 && lastJumpTime > 0 && playerRb2d.velocity.y <= 0) || IsGrounded() )
        {
            playerRb2d.velocity = new Vector2(movementInput.x, jumpForce);
        }
        else if (wallJumpComp && isSliding)
        {
            wallJumpComp.PerformWallJump();
            isSliding = false;
            
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
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

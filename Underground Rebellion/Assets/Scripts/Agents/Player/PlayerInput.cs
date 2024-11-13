using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    private Agent agent;
    private Dash dashComp;
    private PlayerAttack attackComp;
    private WallJump wallJumpComp;
    private ParrySystem playerParrySystem;
    private SpriteRenderer spriteRenderer;
    private new BoxCollider2D collider;
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

    private bool canMove = true;

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
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        agent = GetComponent<Agent>();
        dashComp = GetComponent<Dash>();
        wallJumpComp = GetComponent<WallJump>();
        attackComp = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (!canMove)
        {
            agent.MovementInput = Vector2.zero;
            rigidbody.drag = 10f;
            return;
        }


        agent.wallCheck = wallCheck;
        movementInput = movement.action.ReadValue<Vector2>();

        Movement();

        if (jump.action.triggered)
        {
            Jump();
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
        //Além disso, da pra tirar o objecto filho "wallCheck" e fazer igual fazemos no metodo IsGrounded, onde faz um box cast na direção que o player estiver olhando
        isWallToutch = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.92f, 1.39f), 0, jumpableGround);

        if (dashComp && dash.action.triggered && canDash)
        {
            dashComp.StartDash();

        }
        if(wallJumpComp)
            Slide();
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
        //Se transformar isWallToutch em um metodo é só chamar aqui igual ta fazendo com IsGrounded
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
				//Não precisa fazer isso de colocar 'agent.MovementInput' igual a 0, pois o 'movementInput' ja vai receber 0 caso o player não esteja preciosando o botão
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
            rigidbody.velocity = new Vector2(movementInput.x, jumpForce);
        }
        else if (wallJumpComp && isSliding)
        {
            wallJumpComp.PerformWallJump();
            isSliding = false;
            
        }
    }

    public bool IsGrounded()
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

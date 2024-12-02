using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    private Rigidbody2D playerRb2d;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D playerCollider;
    private Agent agent;
    private Dash dashComp;
    private PlayerAttack attackComp;
    private WallJump wallJumpComp;
    private ParrySystem playerParrySystem;
    [SerializeField] private LayerMask jumpableGround;
    
    public Vector2 movementInput;
    private bool canMove = true;
    
    //Hit Feedback
    private KnockBackFeedback knockBack;
    public ParticleSystem hitVFX;
    public float slowMotionTime;
    public float immunityTime = 2f;

	//Jump
	[SerializeField] private float jumpForce;
    private float lastJumpTime;
    private float lastGroundedTime;
    public float jumpBufferTime;
    public float jumpCoyoteTime;
    private float jumpCut = -0.2f;

    //Wall Jumping
    public Transform wallCheck;
    bool isWallToutch;
    bool isSliding;
    public float wallSlidingSpeed;

    // Attack
    public bool attacking = false;

    //Dash
    public bool canDash = true;

    private enum MovementState { idle, running, jumping, falling}
    public InputActionReference movement, jump, dash, attack;

    private void OnEnable()
    {
        HitEvent.OnHit += OnPlayerHit;
        LevelEvent.OnResetPlayer += ResetPlayer;
    }

    private void OnDisable()
    {
        HitEvent.OnHit -= OnPlayerHit;
		LevelEvent.OnResetPlayer -= ResetPlayer;
	}

	private void Awake()
	{
		playerRb2d = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		playerCollider = GetComponent<BoxCollider2D>();
		agent = GetComponent<Agent>();
		dashComp = GetComponent<Dash>();
		wallJumpComp = GetComponent<WallJump>();
		attackComp = GetComponent<PlayerAttack>();
		playerParrySystem = GetComponent<ParrySystem>();
		knockBack = GetComponent<KnockBackFeedback>();

		
        jump.action.canceled += context => OnJumpUp();
}

	void Start()
    {
		//Antes tava no Update, coloquei no Start, pq não acho que precisar estar lá
		agent.wallCheck = wallCheck;
	}

    // Update is called once per frame
    private void Update()
    {
        if (!canMove)
        {
			agent.MovementInput = Vector2.zero;
            return;
        }

        lastJumpTime -= Time.deltaTime;

        lastGroundedTime = IsGrounded() ? jumpCoyoteTime : lastGroundedTime - Time.deltaTime;

        Movement();

        if (jump.action.triggered)
        {
            lastJumpTime = jumpBufferTime;
            SoundManager.PlaySound(SoundType.JUMP_1, 0.25f);
        }

        Jump();

        if (attack.action.triggered && !attacking)
        {
            attackComp.Attack();
            agent.AttackAnimation();
            
        }
        if (attacking)
        {
            attackComp.ResetAttack();
        }

        if (dashComp && dash.action.triggered && canDash)
        {
            dashComp.StartDash();
        }

        if (wallJumpComp)
        {
            //Da pra mudar isso para um metodo igual a IsGrounded
            //Al�m disso, da pra tirar o objecto filho "wallCheck" e fazer igual fazemos no metodo IsGrounded, onde faz um box cast na dire��o que o player estiver olhando
            isWallToutch = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.92f, 1.39f), 0, jumpableGround);
            Slide();
        }
    }

    private void Movement()
    {
		movementInput = movement.action.ReadValue<Vector2>();
		agent.MovementInput = movementInput;
    }

    private void OnJumpUp()
    {
        lastJumpTime = 0f;
        if(playerRb2d.velocity.y > 0)
        {
			agent.ApplyForce((1 - jumpCut) * playerRb2d.velocity.y * Vector2.down);
        }
    }

    private void Jump()
    {
        if (lastGroundedTime > 0f && lastJumpTime > 0f)
        {
            agent.ApplyForce(new Vector2(0, jumpForce));
			lastJumpTime = 0f;
        }
        else if (wallJumpComp && isSliding && lastJumpTime > 0f)
        {
            wallJumpComp.PerformWallJump();
            isSliding = false;
			lastJumpTime = 0f;
		}
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

	//COLOCAR EM OUTRO SCRIPT
	private void OnPlayerHit(int damage, GameObject sender, GameObject receiver)
	{
		if (receiver.CompareTag("Player"))
		{
			if (sender.CompareTag("Trap"))
			{
				int health = agent.GetHit(damage, sender);
				PlayHitVFX(sender);
				CanvasEvent.UpdateHealth(health);
			}
			else if (sender.CompareTag("Enemy"))
			{

				if (playerParrySystem.CheckParry(sender))
				{
					SoundManager.PlaySound(SoundType.PARRY, 0.5f);
					ParryEvent.Parry(1, sender);
				}
				else
				{
					int health = agent.GetHit(damage, sender);
					PlayHitVFX(sender);
					CanvasEvent.UpdateHealth(health);
				}
			}
		}
	}

	//COLOCAR EM OUTRO SCRIPT (Criar o script 'Player')
	private void PlayHitVFX(GameObject sender)
    {
        StartCoroutine(ImmunityCooldown());

        TimeManager.Instance.StopTime(slowMotionTime);
        
        Vector2 direction = Vector2.up;
        direction.x = Mathf.Sign(gameObject.transform.position.x - sender.transform.position.x);

        CameraManager.Instance.ShakeCamera(0.5f);
		EffectsManager.Instance.PlayOneShot(hitVFX, transform.position, direction * 5);
        knockBack.PlayFeedback(sender);

		SoundManager.PlaySound(SoundType.DAMAGE, 0.25f);
	}

	//COLOCAR EM OUTRO SCRIPT (Criar o script 'Player')
	private void ResetPlayer(Transform lastSavedPosition)
	{
		transform.position = lastSavedPosition.position;
		agent.ResetAgent(true);
	}

    private IEnumerator ImmunityCooldown()
    {
		spriteRenderer.color = Color.gray; //So vai funcionar quando tirar a mudança de cor do parry na animção do player
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"));
		yield return new WaitForSecondsRealtime(immunityTime);
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
		spriteRenderer.color = Color.white; //So vai funcionar quando tirar a mudança de cor do parry na animção do player
	}

	public IEnumerator DisableMovementDuringParry()
	{
		canMove = false;
		agent.ParryAnimation();
		yield return new WaitForSeconds(0.5f);
		canMove = true;
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

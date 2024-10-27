using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Agent agent;
    private new BoxCollider2D collider;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;  // Usamos "SerializeField" para podermos editar os valores no Unity. Tbm daria para editar no unity mudando a variavel para "public" mas dessa forma outros scripts iriam ter acesso às variáveis.
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jumping, falling}

    [SerializeField]
    private InputActionReference movement;

	private void OnEnable()
	{
		HitEvent.OnHit += GetHit;
	}

	private void OnDisable()
	{
		HitEvent.OnHit -= GetHit;
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
    void Update()
    {
        //agent.MovementInput = new Vector2 (Input.GetAxisRaw("Horizontal"), 0);
        
        dirX = Input.GetAxisRaw("Horizontal");   // Caso queira que o Player continue durante um pouco após o largar da tecla devo usar o GetAxis. Ao usar o GetAxisRaw ele para imediatamente após o user largar a tecla!
        agent.MovementInput = movement.action.ReadValue<Vector2>();
        //rigidbody.velocity = new Vector2(dirX * moveSpeed, rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
			//agent.jump = 10f;
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
            
        }

        // Atenção!! Fazer verificação do salto depois da corrida uma vez que o salto tem prioridade!!

        if (rigidbody.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rigidbody.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        animator.SetInteger("state", (int)state);
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

	public void GetHit(int damage, GameObject sender, GameObject receiver)
	{
        if (receiver.CompareTag("Player"))
		{
			agent.GetHit(damage, sender);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Agent agent;
    private PlayerInput player;
    private readonly float dashingPowerX = 24f;
    private readonly float dashingPowerY = 17f;
    private readonly float dashingTime = 0.2f;
    private readonly float dashingCooldown = 1f;
    private TrailRenderer trailRenderer;
    private float originalGravity;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        agent = GetComponent<Agent>();
        player = GetComponent<PlayerInput>();
		trailRenderer = GetComponent<TrailRenderer>();
	}

	private void Start()
	{
        originalGravity = rb2d.gravityScale;
		player.canDash = true;
	}

	public void StartDash()
    {
        StartCoroutine(ExecuteDash());
    }

    private IEnumerator ExecuteDash()
    {
        if (player.movementInput.y != 0f && player.movementInput.x == 0)
        {
            yield break;
        }
        player.canDash = false;
        agent.IsExecutingDash();
		rb2d.gravityScale = 0f;
        agent.Dash(dashingPowerX, dashingPowerY, player.movementInput);
		trailRenderer.enabled = true;
		trailRenderer.emitting = true;
		yield return new WaitForSeconds(dashingTime);
		trailRenderer.emitting = false;
		trailRenderer.Clear();
		trailRenderer.enabled = false;
		rb2d.gravityScale = originalGravity;
        agent.ResetDash();
        yield return new WaitForSeconds(dashingCooldown);
        player.canDash = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Agent agent;
    private PlayerInput player;
    private readonly float dashingPowerX = 24f;
    private readonly float dashingPowerY = 17f;
    private readonly float dashingTime = 0.2f;
    private readonly float dashingCooldown = 1f;
    [SerializeField] TrailRenderer trailRenderer;

    void Start()
    {
        agent = GetComponent<Agent>();
        player = GetComponent<PlayerInput>();
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
        float originalGravity = player.playerRb2d.gravityScale;
        player.playerRb2d.gravityScale = 0f;
        agent.Dash(dashingPowerX, dashingPowerY, player.movementInput);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        player.playerRb2d.gravityScale = originalGravity;
        agent.ResetDash();
        yield return new WaitForSeconds(dashingCooldown);
        player.canDash = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    private PlayerInput player;
    private Agent agent;

    public float wallJumpDuration;
    public Vector2 wallJumpForce;
    public float stop_movement_durantion;
    public float walljumpPower = 10f;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerInput>();
        agent = GetComponent<Agent>();
    }

    public void PerformWallJump()
    {
        if (player.movementInput.x > 0)
        {
            agent.WallJump(wallJumpForce);
        }
        else if (player.movementInput.x < 0)
        {
            agent.WallJump(new Vector2(-wallJumpForce.x, wallJumpForce.y));

        }

        StartCoroutine(StopMovement());
    }

    private IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(stop_movement_durantion);
        agent.ResetWallJump();
    }
}

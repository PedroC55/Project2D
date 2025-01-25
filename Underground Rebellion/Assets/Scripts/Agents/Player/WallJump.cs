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


		wallJumpDuration = 0.5f;
	    wallJumpForce = new Vector2(-10f, 14f);
	    stop_movement_durantion = 0.35f;
	    walljumpPower = 10f;
}

    public void PerformWallJump(Vector2 wallDirection)
    {
        if (wallDirection == Vector2.right)
        {
            agent.WallJump(wallJumpForce);
        }
        else if (wallDirection == Vector2.left)
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

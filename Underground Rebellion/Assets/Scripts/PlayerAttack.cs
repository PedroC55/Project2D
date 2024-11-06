using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;
    private PlayerMovement player;

    private float timetoAttack = 0.25f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        player = GetComponent<PlayerMovement>();
    }


    public void Attack()
    {
        player.attacking = true;
        attackArea.SetActive(player.attacking);
    }

    public void ResetAttack()
    {
        timer += Time.deltaTime;
        if (timer >= timetoAttack)
        {
            timer = 0;
            player.attacking = false;
            attackArea.SetActive(player.attacking);
        }
    }
}

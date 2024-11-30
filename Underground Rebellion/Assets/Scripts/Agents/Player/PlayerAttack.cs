using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]private GameObject attackArea;
    private PlayerInput player;

    private readonly float timetoAttack = 0.25f;
    private float timer = 0f;

    void Start()
    {
        player = GetComponent<PlayerInput>();
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

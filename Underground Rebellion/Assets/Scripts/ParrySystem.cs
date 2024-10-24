using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class ParrySystem : MonoBehaviour
{
    public event Action OnParry;
    public event Action OnGetHit;

    public float parryWindow = 0.5f;
    private float enemyAttackTime;
    private bool parryAttempted = false;

    private PlayerMovement player;

    private int facingDirection = 1;



    // InputAction references for the new input system
    [SerializeField]
    public InputActionReference parry;

    private void OnEnable()
    {
        parry.action.Enable();

        // Subscribe to the parry input action
        parry.action.performed += OnParryPerformed;
    }

    private void OnDisable()
    {
        parry.action.Disable();

        // Unsubscribe from the parry action to avoid memory leaks
        parry.action.performed -= OnParryPerformed;
    }

    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {

        if (player.movementInput.x < 0)
            facingDirection = -1;  // Facing left
        else if (player.movementInput.x > 0)
            facingDirection = 1;   // Facing right
    }

    // This method is triggered when the parry button is pressed
    private void OnParryPerformed(InputAction.CallbackContext context)
    {
        parryAttempted = true;
        CheckParryTiming();
    }

    // Called by the enemy when it attacks
    public void EnemyAttacks(float attackTime, int enemyDirection)
    {
        enemyAttackTime = attackTime;

        // Check if the player is facing the enemy
        if (facingDirection == enemyDirection)
        {
            CheckParryTiming();
        }
        else
        {
            // Player is not facing the enemy, trigger hit
            OnGetHit?.Invoke();
        }
    }

    private void CheckParryTiming()
    {
        float currentTime = Time.time;
        OnParry?.Invoke();

        // Check if player's parry attempt was within the allowed time window
        if (parryAttempted && Mathf.Abs(currentTime - enemyAttackTime) <= parryWindow)
        {
            OnParry?.Invoke();  // Parry is successful
        }
        else
        {
            OnGetHit?.Invoke(); // Parry failed, player got hit
        }

        // Reset parry attempt flag
        parryAttempted = false;
    }
}
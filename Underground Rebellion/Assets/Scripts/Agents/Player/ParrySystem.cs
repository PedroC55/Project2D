using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class ParrySystem : MonoBehaviour
{
    public float parryWindow = 0.5f;
	public float parryCoolDown = 0.75f;
	private bool parryAttempted = false;

    private PlayerInput player;

    private float parryTime;

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
        player = GetComponent<PlayerInput>();
    }

    // This method is triggered when the parry button is pressed
    private void OnParryPerformed(InputAction.CallbackContext context)
    {
        if (!parryAttempted)
        {
			StartCoroutine(player.DisableMovementDuringParry());
			parryAttempted = true;
			parryTime = Time.deltaTime;
			StartCoroutine(ParryCoolDown());
		}
	}

    public bool CheckParryTiming()
    {
        float currentTime = Time.deltaTime;
        bool sucess = false;
        // Check if player's parry attempt was within the allowed time window
        if (!parryAttempted)
        {
            return false;
        }
        if (parryAttempted && Mathf.Abs(parryTime - currentTime) <= parryWindow )
        {
            sucess = true;
            SoundManager.PlaySound(SoundType.PARRY);
        }

        parryAttempted = false;

        return sucess;
        
    }

    public bool CheckDirection(GameObject enemy)
    {
        bool sucess = false;

        Vector2 direction = (enemy.transform.position - player.transform.position).normalized;
        if((direction.x > 0 && player.transform.localScale.x == 1) || (direction.x < 0 && player.transform.localScale.x == -1))
        {
            sucess = true;
        }
        
        return sucess;
    }

    private IEnumerator ParryCoolDown()
    {
		yield return new WaitForSeconds(parryCoolDown);
        parryAttempted = false;
	}
}
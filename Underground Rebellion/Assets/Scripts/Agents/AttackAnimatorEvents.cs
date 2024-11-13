using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackAnimatorEvents : ActionAnimatorEvent
{
    /// <summary>
    /// EXCLUIR DEPOIS
    /// </summary>
	private SpriteRenderer sprite;
	private Collider2D attackCollider;
	//public ParticleSystem impactEffect;
	//public Transform impactTransform;
	//public float cameraShakeIntensity = 0.2f;

	private void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		attackCollider = GetComponent<Collider2D>();
	}

	public override void OnActionStart()
    {
		sprite.enabled = true;
		attackCollider.enabled = true;
    }

    public override void OnActionEnd()
    {
		sprite.enabled = false;
		attackCollider.enabled = false;
	}
}

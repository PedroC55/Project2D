using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackAnimatorEvents : ActionAnimatorEvent
{
    /// <summary>
    /// EXCLUIR DEPOIS
    /// </summary>
	public SpriteRenderer Sprite;
	public Collider2D AttackCollider;
    //public ParticleSystem impactEffect;
    //public Transform impactTransform;
    //public float cameraShakeIntensity = 0.2f;

    public override void OnActionStart()
    {
		Sprite.enabled = true;
		AttackCollider.enabled = true;
    }

    public override void OnActionEnd()
    {
		Sprite.enabled = false;
		AttackCollider.enabled = false;
	}
}

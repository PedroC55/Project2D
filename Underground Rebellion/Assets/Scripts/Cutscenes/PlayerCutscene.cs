using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCutscene : MonoBehaviour
{
	[SerializeField] private ParticleSystem falldropVFX;
	private Rigidbody2D rb2d;
	private Animator animator;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void OnEnable()
	{
		CutsceneEvent.OnStopScroller += StartFall;
	}

	void OnDisable()
	{
		CutsceneEvent.OnStopScroller -= StartFall;
	}

	private void StartFall()
	{
		rb2d.bodyType = RigidbodyType2D.Dynamic;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Finish"))
		{
			animator.SetTrigger("down");
			PlayFallVFX();
		}
	}

	private void PlayFallVFX()
	{
		Vector2 direction = Vector2.up;

		CameraManager.Instance.ShakeCamera(1f);
		EffectsManager.Instance.PlayOneShot(falldropVFX, transform.position, direction);

		SoundManager.Instance.PlaySound(SoundType.DAMAGE);
	}
}

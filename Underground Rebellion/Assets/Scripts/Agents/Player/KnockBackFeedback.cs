using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockBackFeedback : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField]
    private float strength = 16, delay = 0.15f;

    public UnityEvent OnBegin, OnDone;

	private void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	public void PlayFeedback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector2 direction = (gameObject.transform.position - sender.transform.position).normalized;
		Debug.Log(direction);
		direction.x = Mathf.Sign(direction.x);
        direction.y = (direction.y > -0.5f) ? 1 : -1 ;
        rb2d.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
	}

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb2d.velocity = Vector2.zero;
        OnDone?.Invoke();
    }
}

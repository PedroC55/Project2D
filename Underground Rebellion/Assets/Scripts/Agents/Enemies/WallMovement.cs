using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.U2D;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    public float speed = 2f;
    public LayerMask groundLayer;
    private Vector2 direction;
    private Vector2 direction45Degree;
    public float rayDistance = 2f;
	public float gravityForce = 10f;
	private Rigidbody2D rb;
	private Vector2 gravityDirection;

	public Vector2 directionOffset;
    private Vector2 currentOffset;

    private Agent agent;

    private bool isRotation = false;
    public float rotationLerpTime = 2f;
    private float currenteRotationTime = 0f;
    private Quaternion startRotation;

    private float goalAngle = 0f;

	private void Start()
    {
        // Iniciar com movimento horizontal
		rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<Agent>();
        rb.gravityScale = 0; // Desativa gravidade padrão
	}

    private void FixedUpdate()
    {
        Quaternion enemyRotation = transform.rotation;
		float faceDirection = transform.localScale.x;
        currentOffset = directionOffset;

        gravityDirection = new(0, -1);
		direction = new(faceDirection, 0);
		direction45Degree = new(faceDirection, -1);

		direction = enemyRotation * direction;
		direction45Degree = enemyRotation * direction45Degree;
		gravityDirection = enemyRotation * gravityDirection;
		currentOffset = enemyRotation * currentOffset;

		Debug.DrawRay((Vector2)transform.position - currentOffset, gravityDirection * rayDistance, Color.red);
        Debug.DrawRay((Vector2)transform.position - currentOffset, direction * rayDistance, Color.yellow);
        Debug.DrawRay((Vector2)transform.position - currentOffset, direction45Degree * rayDistance, Color.yellow);

		rb.AddForce(gravityDirection * gravityForce, ForceMode2D.Force);
		agent.MovementInput = direction * speed;
        
        DetectWallOrEdge();

	}

    private void DetectWallOrEdge()
    {
        if (isRotation)
        {
            ChangeDirection();
            return;
        }


        // Raycast para detectar se existe superfície na direção do movimento
        RaycastHit2D hitWall = Physics2D.Raycast((Vector2)transform.position - currentOffset, direction, rayDistance, groundLayer);
        if(hitWall.collider != null)
        {
            //Rotate Up

            return;
        }
		RaycastHit2D hitEdge = Physics2D.Raycast((Vector2)transform.position - currentOffset, direction45Degree, rayDistance, groundLayer);
        if(hitEdge.collider == null)
        {
            Debug.Log("Não achou collider do chão");
            //Rotate Down
            isRotation = true;
			if (gravityDirection.y == -1 || gravityDirection.y == 1)
				goalAngle += (direction45Degree.x * direction45Degree.y) > 0 ? 90 : -90;
            else
				goalAngle += (direction45Degree.x * direction45Degree.y) > 0 ? -90 : 90;

            startRotation = transform.rotation;

			//float zRotation = transform.eulerAngles.z;
			//         Debug.Log(zRotation);
			//transform.rotation = Quaternion.Euler(0, 0, zRotation - speed * 1.9f);
			return;
        }
    }

    private void ChangeDirection()
    {
        Quaternion goalQuaternion = Quaternion.Euler(0, 0, goalAngle);

		transform.rotation = Quaternion.Lerp(startRotation, goalQuaternion, currenteRotationTime / rotationLerpTime);
        currenteRotationTime += Time.deltaTime;

        Quaternion angleDiff = transform.rotation * Quaternion.Inverse(goalQuaternion);

		if (angleDiff.eulerAngles.z < 5f)
        {
            Debug.Log("Chegou no destino");
            isRotation = false;
			transform.rotation = goalQuaternion;
            currenteRotationTime = 0f;
		}
		//Se x > 0 e y < 0 = -90
		//Se x < 0 e y < 0 = 90
		//Se x > 0 e y > 0 = 90
		//Se x < 0 e y > 0 = -90


        //Se x < 0 e y < 0 = -90
        //Se x < 0 e y > 0 = 90
        //Se x > 0 e y < 0 = 90
        //Se x > 0 e y > 0 = -90
	}
}

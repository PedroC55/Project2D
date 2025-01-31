using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    private ConstantForce2D constForce2D;
    private BoxCollider2D bCollider2D;

    private LayerMask groundLayer;
    private Vector2 direction;
    private Vector2 gravityDirection;

    [SerializeField]
    private float rayDistance = 1f;
    [SerializeField]
    private float gravityForce = 10f;

    public Vector2 directionOffset;
    private Vector2 currentOffset;

    private Agent agent;

    private bool isRotating = false;
    public float rotationLerpTime = 2f;
    private float currenteRotationTime = 0f;
    private Quaternion startRotation;
    private float goalAngle = 0f;

    public TerrainPath tPath;
    private TerrainPathNode currentNode;
    private TerrainPathNode destinyNode;

    private GravityDirection gDirection;
    private readonly List<int> gravityAnglesDirection = new() { 0, 90, 180, 270, 360 };
    private readonly Dictionary<int, GravityDirection> gravityDict = new()
    {
        {0, GravityDirection.Down},
        {90, GravityDirection.Right},
        {180, GravityDirection.Up},
        {270, GravityDirection.Left},
        {360, GravityDirection.Down},
    };

    private void Start()
    {
        constForce2D = GetComponent<ConstantForce2D>();
        bCollider2D = GetComponent<BoxCollider2D>();
        agent = GetComponent<Agent>();

        groundLayer = LayerMask.GetMask("Ground");

        currentNode = tPath.GetNodeCloseToTransform(gameObject.transform.position);

        SetGravityDirection();
    }

    private void FixedUpdate()
    {
        UpdateDirections();

        constForce2D.force = gravityDirection.normalized * gravityForce;

        DetectWallOrEdge();
    }
    public Vector2 GetDirection()
    {
        return direction;
    }

    public GravityDirection GetGravityDirection()
    {
        return gDirection;
    }

    public bool IsRotating()
    {
        return isRotating;
    }

    public void SetDestiny(Transform objectTransform)
    {
        TerrainPathNode node = tPath.GetNodeCloseToTransform(objectTransform.position);

        if (!destinyNode || destinyNode.id != node.id)
        {
            destinyNode = node;
            //Tem que fazer isso para que não bugue quando for fazer o calculo da nova direção
            currentNode = tPath.GetNodeCloseToTransform((Vector2)gameObject.transform.position - currentOffset);
        }
    }

    //Mudar isso daqui para o Patrol chamar a função para dizer qual o node ele deve ir
    public void UpdateNode(TerrainPathNode node)
    {
        currentNode = node;
    }

    private void UpdateDirections()
    {
        Quaternion agentRotation = transform.rotation;
        currentOffset = directionOffset;

        gravityDirection = Vector2.down;
        gravityDirection = agentRotation * gravityDirection;
        currentOffset = agentRotation * currentOffset;

        if (isRotating)
        {
            direction = agentRotation * new Vector2(transform.localScale.x, 0);
            return;
        }

        //Check if tem destinyNode, se tiver fazer com que vá nessa direção
        if (!destinyNode || Vector2.Distance(transform.position, destinyNode.transform.position) <= 0.5f)
        {
            direction = Vector2.zero;
            return;
        }

        if(currentNode.id == destinyNode.id)
        {
			direction = agentRotation * CalculateDirectionCurrentNode();
            return;
		}

        if (tPath.IsLoopPath())
        {
            direction = agentRotation * CalculateLoopPathDirection();
			return;
        }

        if (destinyNode.id > currentNode.id)
        {
            //nextPosition = currentNode.next.transform.position;
            direction = agentRotation * new Vector2(1, 0);
            return;
        }
        if (destinyNode.id < currentNode.id)
        {
            direction = agentRotation * new Vector2(-1, 0);
            return;
        }
    }

    private void DetectWallOrEdge()
    {
        if (isRotating)
        {
            RotateAgent();
            return;
        }

        if (!IsGrounded())
            return;


        // Raycast para detectar se existe superf�cie na dire��o do movimento
        RaycastHit2D hitWall = Physics2D.Raycast((Vector2)transform.position - currentOffset, direction, rayDistance, groundLayer);
        if (hitWall.collider != null)
        {
            GravityAngleTransition(false);
            return;
        }
        RaycastHit2D hitEdge = Physics2D.Raycast((Vector2)transform.position - currentOffset, gravityDirection, rayDistance, groundLayer);
        if (hitEdge.collider == null)
        {
            GravityAngleTransition(true);
            return;
        }
    }

    private void RotateAgent()
    {
        Quaternion goalQuaternion = Quaternion.Euler(0, 0, startRotation.eulerAngles.z + goalAngle);
        float currentSpeed = agent.GetCurrentSpeed();

        transform.rotation = Quaternion.Lerp(startRotation, goalQuaternion, (currenteRotationTime * currentSpeed) / rotationLerpTime);
        currenteRotationTime += Time.deltaTime;

        Quaternion angleDiff = transform.rotation * Quaternion.Inverse(goalQuaternion);

        if (angleDiff.eulerAngles.z < 5f)
        {
            isRotating = false;
            transform.rotation = goalQuaternion;
            currenteRotationTime = 0f;
            SetGravityDirection();
        }
    }

    private void GravityAngleTransition(bool isEdge)
    {
        isRotating = true;

        //Se tiver olhando para direita vai rotacionar positivamente
        //Se tiver olhando para esquerda vai rotacionar negativamente
        //Se for rotação pq acabou o chão, então é o contrario
        float _ = (isEdge ? -90 : 90);
        goalAngle = _ * transform.localScale.x;

        startRotation = transform.rotation;
    }

    private void SetGravityDirection()
    {
        float zAngle = transform.rotation.eulerAngles.z;

        int closest = gravityAnglesDirection.OrderBy(item => Mathf.Abs(zAngle - item)).First();

        gDirection = gravityDict[closest];
    }

    private Vector2 CalculateDirectionCurrentNode()
    {
		Vector2 nodeDirection = (currentNode.transform.position - transform.position).normalized;

		int xValue = 1;

		switch (gDirection)
		{
			case GravityDirection.Right:
				if (nodeDirection.y < 0)
					xValue = -1;

				break;

			case GravityDirection.Down:
				if (nodeDirection.x < 0)
					xValue = -1;

				break;

			case GravityDirection.Left:
				if (nodeDirection.y > 0)
					xValue = -1;

				break;

			case GravityDirection.Up:
				if (nodeDirection.x > 0)
					xValue = -1;

				break;
		}

		return new Vector2(xValue, 0);
	}

    private Vector2 CalculateLoopPathDirection()
    {
        bool isPositiveX = true;
        // 9 > 2
        if(destinyNode.id > currentNode.id)
        {
            // 9 - 2 = 7
            int idDistance = destinyNode.id - currentNode.id;
            // 2 - (9 - 10) = 3
            int idDistanceLoop = currentNode.id - (destinyNode.id - tPath.nodes.Length);

            if(idDistanceLoop < idDistance)
                isPositiveX = false;
        }
		//2 < 9
		else
		{
            //previous path
			// 9 - 2 = 7
			int idDistance = currentNode.id - destinyNode.id;
			// (2 + 10) - 9 = 3
			int idDistanceLoop = (destinyNode.id + tPath.nodes.Length) - currentNode.id;

			if (idDistanceLoop > idDistance)
				isPositiveX = false;
		}

        return new Vector2(isPositiveX ? 1 : -1, 0);
	}

	private bool IsGrounded()
	{
		return Physics2D.BoxCast(bCollider2D.bounds.center, bCollider2D.bounds.size, 0f, gravityDirection.normalized, .1f, groundLayer);
	}

}

public enum GravityDirection { Down, Left, Up, Right };

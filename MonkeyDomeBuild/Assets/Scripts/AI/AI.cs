using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : Actor
{
	enum State
	{
		Idle,
		Catch,
		Throw,
		Move
	}

	private BoxCollider2D myCollider;

	public GameObject tempTarg;
	public float approxJumpDist;

	[SerializeField]
	private Vector3 MoveTarget;

	//private Vector3 ToTarget;

	[SerializeField]
	private Vector3 currEndTarg;

	private Vector3 currEndTargBound;

	private float currClosestDist;
	private float maxJump;
	private float jumpVelocity;
	private float xInput;
	private int count;
	private bool canJump;
	private bool isAtTargetX;

	State currentState;

	// Use this for initialization
	void Start()
	{
		currentState = State.Idle;

		myCollider = GetComponent<BoxCollider2D>();
		cache_tf = GetComponent<Transform>();
		cache_rb = GetComponent<Rigidbody2D>();
		tempTarg = GameObject.FindGameObjectWithTag("Ball");
		canJump = true;

		CalculateMaxJump();
		UpdateTarget();
	}

	void Update()
	{
		if (GameManager.Instance.gmInputs[playerIndex].mJump)
		{
			Jumping();
		}
		Aim();
		characterType.CHUpdate();
		CheckLeader();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		ExecuteState();
		MovementVelocity();
		AnimationControl();
		characterType.CHFixedUpdate();
	}

	void ExecuteState()
	{
		switch (currentState)
		{
			case State.Idle:
				currentState = ExecuteIdle();
				break;
			case State.Catch:
				currentState = ExecuteCatch();
				break;
			case State.Throw:
				currentState = ExecuteThrow();
				break;
			case State.Move:
				currentState = ExecuteMove();
				break;
			default:
				Debug.LogError("YOU FUCKING BROKE IT!");
				break;
		}

	}

	State ExecuteIdle()
	{
		if (GameManager.Instance.gmUIManager.matchTime < GameManager.Instance.gmUIManager.startMatchTime)
		{
			return State.Move;
		}
		return State.Move;
	}

	State ExecuteCatch()
	{
		//TODO Catch Logic
		return currentState;
	}

	State ExecuteThrow()
	{
		//TODO Target Selection and Throw Logic
		return currentState;
	}

	State ExecuteMove()
	{
		isAtTargetX = CheckClose();
		if (isAtTargetX && cache_tf.position.y >= currEndTarg.y)
		{
			UpdateTarget();
			isAtTargetX = CheckClose();
			canJump = true;
		}

		xInput = (currEndTarg - cache_tf.position).normalized.x;
		
		if (currEndTarg.y > cache_tf.position.y - (myCollider.size.y - myCollider.offset.y) - 0.5f)
		{
			if (isAtTargetX)
			{
				float dirmult = (currEndTargBound - currEndTarg).normalized.x;
				dirmult = currEndTargBound.x + (dirmult * (approxJumpDist * 0.5f));
				currEndTarg.x = dirmult;
				currEndTarg.y = cache_tf.position.y;
			}
			else
			{
				if (currEndTargBound.x >= cache_tf.transform.position.x - approxJumpDist)
				{
					if (canJump && !IsInAir)
					{
						xInput = CalculateJump(currEndTargBound, false) / characterType.movespeed;
						if (xInput <= 1.0f && xInput >= -1.0f)
						{
							GameManager.Instance.gmInputs[playerIndex].mJump = true;
							canJump = false;
							StartCoroutine(RealisticInput());
						}
					}
				}
				else if (currEndTargBound.x <= cache_tf.transform.position.x + approxJumpDist)
				{
					if (canJump && !IsInAir)
					{
						xInput = CalculateJump(currEndTargBound, true) / characterType.movespeed;
						if (xInput <= 1.0f && xInput >= -1.0f)
						{
							GameManager.Instance.gmInputs[playerIndex].mJump = true;
							canJump = false;
							StartCoroutine(RealisticInput());
						}
					}
				}
			}
		}
		else if(currEndTarg.y < cache_tf.position.y - (myCollider.size.y - myCollider.offset.y))
		{
			if(isAtTargetX)
			{
				float dirmult = (currEndTargBound - currEndTarg).normalized.x;
				currEndTarg.x = currEndTargBound.x + 0.5f * dirmult;
				currEndTarg.y = currEndTargBound.y;
			}
		}
		GameManager.Instance.gmInputs[playerIndex].mXY.x = xInput;
		
		return currentState;
	}

	private bool CheckClose()
	{
		return (cache_tf.position.x < currEndTarg.x + 0.5f && cache_tf.position.x > currEndTarg.x - 0.5f);
	}

	private void UpdateTarget()
	{
		MoveTarget = tempTarg.transform.position;
		if (!IsInAir)
		{
			FindNearestPlatform(MoveTarget);
			if (MoveTarget.y > currEndTarg.y)
			{
				count = GameManager.Instance.gmLevelObjectScript.numberOfLevels;
				while (currEndTarg.y > (cache_tf.position.y + maxJump - (myCollider.size.y - myCollider.offset.y)) || count <= 0)
				{
					FindNearestPlatform(currEndTarg);
					count--;
				}
			}
			currEndTargBound = FindEdgeOfPlatform(currEndTarg);
		}
	}

	private void FindNearestPlatform(Vector3 FinalPos)
	{
		Vector3 position;
		currClosestDist = 0.0f;
		float dist;
		for (float i = 0.0f; i <= 1.0f; i += 0.05f)
		{
			position = Vector3.Lerp(cache_tf.position, FinalPos, i);
			foreach (var T in GameManager.Instance.gmLevelObjectScript.loPlatforms)
			{
				if (T.transform.position != FinalPos)
				{
					dist = (T.transform.position - position).magnitude;
					if (currClosestDist == 0.0f)
					{
						currClosestDist = dist;
						currEndTarg = T.transform.position;
					}
					else if (dist < currClosestDist)
					{
						currClosestDist = dist;
						currEndTarg = T.transform.position;
					}
				}
			}
		}
	}

	private Vector3 FindEdgeOfPlatform(Vector3 platPos)
	{
		Vector3 result = Vector3.zero;
		foreach (var T in GameManager.Instance.gmLevelObjectScript.loPlatforms)
		{
			if (platPos == T.transform.position)
			{
				if (cache_tf.position.x < T.transform.position.x)
				{
					result = T.transform.position;
					result.x -= (T.transform.lossyScale.x * 0.5f);
				}
				else
				{
					result = T.transform.position;
					result.x += (T.transform.lossyScale.x * 0.5f);
				}
			}
		}
		return result;
	}

	private float CalculateJump(Vector3 jumpLanding, bool dir) // right is true
	{
		float dx;
		float t;
		if (dir)
		{
			dx = (jumpLanding.x + 1.0f) - cache_tf.position.x;
		}
		else
		{
			dx = (jumpLanding.x - 1.0f) - cache_tf.position.x;
		}
		t = jumpVelocity / (cache_rb.gravityScale * Physics2D.gravity.magnitude);
		return dx / t;
	}

	private void CalculateMaxJump()
	{
		//float t;
		float g = cache_rb.gravityScale * Physics2D.gravity.magnitude;
		jumpVelocity = characterType.jumpforce / cache_rb.mass;
		maxJump = (jumpVelocity * jumpVelocity) / (2 * g);
		approxJumpDist = (jumpVelocity / g) * characterType.movespeed;
	}

	IEnumerator RealisticInput()
	{
		yield return new WaitForEndOfFrame();
		GameManager.Instance.gmInputs[playerIndex].mJump = false;
	}
}
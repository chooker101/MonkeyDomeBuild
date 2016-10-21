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

	public BoxCollider2D myCollider;

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
	private int levelCounter;
	private bool canJump;
	private bool isAtTargetX;
	private float centerToFeet;
	private bool isStuck = false;
	private float reverseX;
	private bool isEndTargVine = false;
	private Vector3 calcVar;

	State currentState;

	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		currentState = State.Idle;

		//myCollider = GetComponent<BoxCollider2D>();
		cache_tf = GetComponent<Transform>();
		cache_rb = GetComponent<Rigidbody2D>();
		tempTarg = GameObject.FindGameObjectWithTag("Ball");
		canJump = true;
		centerToFeet = myCollider.size.y * 0.5f;
		//(myCollider.size.x * 0.5f - myCollider.offset.x);

		CalculateMaxJump();
		UpdateTarget();
	}

	void Update()
	{
		cType = characterType.ToString();
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
		if (IsHoldingBall && ballHolding == null)
		{
			ReleaseBall();
		}
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

		if (!IsInAir)
		{
			if ((isAtTargetX && (currEndTarg.y > cache_tf.position.y - (centerToFeet + 0.5f) && currEndTarg.y < cache_tf.position.y + (centerToFeet + 0.5f))))
			{
				UpdateTarget();
				isAtTargetX = CheckClose();
				canJump = true;
			}
		}

		calcVar = cache_tf.position;
		calcVar.y += (maxJump - centerToFeet);
		Debug.DrawLine(cache_tf.position, currEndTarg, Color.red);
		Debug.DrawLine(cache_tf.position, MoveTarget, Color.blue);
		Debug.DrawLine(cache_tf.position, calcVar, Color.yellow);

		if((isEndTargVine && cache_tf.position.y >= currEndTargBound.y) && IsInAir)
		{
			GameManager.Instance.gmInputs[playerIndex].mJump = true;
			StartCoroutine(RealisticInputJump());
		}

		if (!IsInAir)
		{
			xInput = (currEndTarg - cache_tf.position).normalized.x;
		}

		if (!IsAtMainTarget())
		{
			if (MoveTarget.y > cache_tf.position.y - centerToFeet)
			{
				if ((isAtTargetX && currEndTarg.y > cache_tf.position.y) && (!IsInAir && currEndTarg.y < (cache_tf.position.y + maxJump - centerToFeet - 1.5f)))
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
							if (!isEndTargVine)
							{
								xInput = CalculateJump(currEndTargBound, false) / characterType.movespeed;
								if (xInput <= 1.0f && xInput >= -1.0f)
								{
									GameManager.Instance.gmInputs[playerIndex].mJump = true;
									canJump = false;
									StartCoroutine(RealisticInputJump());
								}
							}
							else
							{
								if (canJump && !IsInAir)
								{
									xInput = CalculateJump(currEndTargBound, false) / characterType.movespeed;
									if (xInput <= 1.0f && xInput >= -1.0f)
									{
										GameManager.Instance.gmInputs[playerIndex].mJump = true;
										canJump = false;
										StartCoroutine(RealisticInputJump());
									}
								}
							}
						}
						else if (currEndTargBound.x <= cache_tf.transform.position.x + approxJumpDist)
						{
							if (!isEndTargVine)
							{
								if (canJump && !IsInAir)
								{
									xInput = CalculateJump(currEndTargBound, true) / characterType.movespeed;
									if (xInput <= 1.0f && xInput >= -1.0f)
									{
										GameManager.Instance.gmInputs[playerIndex].mJump = true;
										canJump = false;
										StartCoroutine(RealisticInputJump());
									}
								}
							}
							else
							{
								if (canJump && !IsInAir)
								{
									xInput = CalculateJump(currEndTargBound, true) / characterType.movespeed;
									if (xInput <= 1.0f && xInput >= -1.0f)
									{
										GameManager.Instance.gmInputs[playerIndex].mJump = true;
										canJump = false;
										StartCoroutine(RealisticInputJump());
									}
								}
							}
						}
					}
				}
			}
			else if (MoveTarget.y < cache_tf.position.y - centerToFeet)
			{
				if (isAtTargetX && !IsInAir)
				{
					float dirmult = (currEndTargBound - currEndTarg).normalized.x;
					currEndTarg.x = currEndTargBound.x + (2f + (myCollider.size.x * 0.5f)) * dirmult;
					currEndTarg.y = currEndTargBound.y;
				}
			}
		}

		if(isStuck)
		{
			xInput = reverseX;
		}
		else if(RayCastSide(xInput / Mathf.Abs(xInput)))
		{
			StartCoroutine(RealisticInputX());
			reverseX = -xInput;
			isStuck = true;
		}

		if(xInput < 0.05f && xInput > -0.05f)
		{
			xInput = 0.0f;
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
		if (!IsAtMainTarget())
		{
			if (MoveTarget.y > cache_tf.position.y - centerToFeet)
			{
				FindNearestLevelObject(MoveTarget);
				if (currEndTarg.y > (cache_tf.position.y + (maxJump - centerToFeet - 1.5f)))
				{
					levelCounter = GameManager.Instance.gmLevelObjectScript.numberOfLevels;
					while (currEndTarg.y > (cache_tf.position.y + (maxJump - centerToFeet - 1.5f)) && levelCounter > 0)
					{
						FindNearestLevelObject(currEndTarg);
						levelCounter--;
					}
				}
				if (!isEndTargVine)
				{
					currEndTargBound = FindEdgeOfPlatform(currEndTarg);
				}
				else
				{
					currEndTargBound = FindEdgeOfVine(currEndTarg);
				}
			}
			else if (MoveTarget.y <= cache_tf.position.y - centerToFeet)
			{
				FindNearestLevelObject(MoveTarget);
				currEndTargBound = FindEdgeOfPlatform(currEndTarg);
			}
		}
	}

	private bool IsAtMainTarget()
	{
		return ((cache_tf.position.x < MoveTarget.x + 0.5f && cache_tf.position.x > MoveTarget.x - 0.5f) && (MoveTarget.y > cache_tf.position.y - (centerToFeet + 0.5f) && MoveTarget.y < cache_tf.position.y + (centerToFeet + 0.5f)));
	}

	private void FindNearestLevelObject(Vector3 FinalPos)
	{
		Vector3 position;
		currClosestDist = 0.0f;
		float dist;
		for (float i = 0.0f; i <= 1.0f; i += 0.05f)
		{
			position = Vector3.Lerp(cache_tf.position, FinalPos, i);
			foreach (var T in GameManager.Instance.gmLevelObjectScript.loPlatforms)
			{
				if (T.transform.position != FinalPos && !((cache_tf.position.x < T.transform.position.x + 0.5f && cache_tf.position.x > T.transform.position.x - 0.5f) && (T.transform.position.y > cache_tf.position.y - (centerToFeet + 0.5f) && T.transform.position.y < cache_tf.position.y + (centerToFeet + 0.5f))))
				{
					if (T.transform.position.y < FinalPos.y)
					{
						dist = (T.transform.position - position).magnitude;
						if (currClosestDist == 0.0f)
						{
							currClosestDist = dist;
							currEndTarg = T.transform.position;
							isEndTargVine = false;
						}
						else if (dist < currClosestDist)
						{
							currClosestDist = dist;
							currEndTarg = T.transform.position;
							isEndTargVine = false;
						}
					}
				}
			}
			foreach (var Y in GameManager.Instance.gmLevelObjectScript.loVines)
			{
				if (Y.transform.position != FinalPos && !((cache_tf.position.x < Y.transform.position.x + 0.5f && cache_tf.position.x > Y.transform.position.x - 0.5f) && (Y.transform.position.y > cache_tf.position.y - (centerToFeet + 0.5f) && Y.transform.position.y < cache_tf.position.y + (centerToFeet + 0.5f))))
				{
					if (Y.transform.position.y < FinalPos.y)
					{
						dist = (Y.transform.position - position).magnitude;
						if (currClosestDist == 0.0f)
						{
							currClosestDist = dist;
							currEndTarg = Y.transform.position;
							isEndTargVine = true;
						}
						else if (dist < currClosestDist)
						{
							currClosestDist = dist;
							currEndTarg = Y.transform.position;
							isEndTargVine = true;
						}
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

	private Vector3 FindEdgeOfVine(Vector3 vinePos)
	{
		Vector3 result = vinePos;
		foreach (var Y in GameManager.Instance.gmLevelObjectScript.loVines)
		{
			if (vinePos == Y.transform.position)
			{
				if(cache_tf.position.y < Y.transform.position.y)
				{
					result.y -= (Y.GetComponent<BoxCollider2D>().size.y - 1.0f);
				}
				else
				{
					result.y += (Y.GetComponent<BoxCollider2D>().size.y - 1.0f);
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

	IEnumerator RealisticInputJump()
	{
		yield return new WaitForEndOfFrame();
		GameManager.Instance.gmInputs[playerIndex].mJump = false;
	}

	IEnumerator RealisticInputX()
	{
		yield return new WaitForSeconds(1.0f);
		isStuck = false;
	}
}
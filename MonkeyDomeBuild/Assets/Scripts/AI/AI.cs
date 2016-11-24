using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

	[SerializeField]
	private float catchWaitTime = 0.3f;

	[SerializeField]
	private Vector3 currEndTargBound;

	private float currClosestDist;
	private float maxJump;
	private float jumpVelocity;
	private float throwVelocity;
	private float xInput;
	private int levelCounter;
	private bool canJump;
	private bool isAtTargetX;
	private float centerToFeet;
	private bool isStuck = false;
	private float reverseX;
	private bool isEndTargVine = false;
	private Vector3 calcVar;
	private Vector3 aimDir;
	private int lastPlayerTarget = 0;
	private bool letCatch = false;
	private bool waitForBallOnce = true;
	private bool wantsToThrow = false;
	private int upOrDownThrow = 0;
	private LayerMask ballLayer;
	private bool onCatchCoolDown = false;
	private Vector3 prevAiPos;
	private float timeInSamePos;
	private bool hadBalLLastFrame;
	private bool isTargetCoroutineRunning = false;
	private bool updateEndTargBeforeCoroutine = false;
	private bool oldSceneCheck = false;
	private string currSceneOld;
	private string lastSceneOld;

	//throw away shit
	private float g;
	private float range;
	private float height;
	private float negTheta;
	private float posTheta;
	private float t1;
	private float t2;
	private float xVel1;
	private float yVel1;
	private float xVel2;
	private float yVel2;
	private float directionOfParabola;
	private RaycastHit2D lineHit;
	private Vector2 posOnParabola;
	private Vector2 prevPosOnParabola;
	private Vector3 prevCurrEndTarg;
	private Vector3 vineTarget;
	private Target nearestSignTarget;
	private int gorillaIndex;
	private List<Transform> viableTargets = new List<Transform>();

	[SerializeField]
	State currentState;

	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		//currentState = State.Move;
#if UNITY_5_3
#if UNITY_EDITOR
		lastSceneOld = "";
		oldSceneCheck = true;
#endif
#elif UNITY_5_4
		SceneManager.activeSceneChanged += sceneChangedDelegate;
#endif
		//myCollider = GetComponent<BoxCollider2D>();
		cache_tf = GetComponent<Transform>();
		cache_rb = GetComponent<Rigidbody2D>();
		tempTarg = null;
		canJump = true;
		centerToFeet = myCollider.size.y * 0.5f;
		//(myCollider.size.x * 0.5f - myCollider.offset.x);
		ballLayer = (1 << 8);
		prevAiPos = cache_tf.position;

		CalculateMaxJump();
		UpdateTarget();
		if (currentState == State.Move)
		{
			UpdateEndTarget();
		}
	}

	void Update()
	{
		if (oldSceneCheck)
		{
#if UNITY_5_3
#if UNITY_EDITOR
			currSceneOld = EditorApplication.currentScene.ToString();
#endif
#endif
			if (currSceneOld != lastSceneOld)
			{
				sceneChangedOld();
			}
		}
		if (!haveBall && hadBalLLastFrame)
		{
			if (!onCatchCoolDown)
			{
				onCatchCoolDown = true;
				StartCoroutine(WaitForCatchCoolDown());
			}
		}
		if (!onCatchCoolDown && currentState != State.Idle)
		{
			IsBallNear();
		}
		//cType = characterType.ToString();
		if (GameManager.Instance.gmInputs[inputIndex].mJump)
		{
			Jumping();
		}
		Aim();
		characterType.CHUpdate();
		CheckLeader();

		if (isinair && cache_rb.velocity.y < 0f)
		{
			if (isClimbing)
			{
				//animator.SetBool("IsIdle", true);
				//animator.SetBool("IsInAirDown", false); 
				animator.SetBool("IsClimbing", true);
			}
			else
			{
				animator.SetBool("IsClimbing", false);
				animator.SetBool("IsInAirDown", true);
				animator.SetBool("IsInAir", false);
				animator.SetBool("IsJumping", false);
			}
		}
		else if (isinair && cache_rb.velocity.y >= 0f)
		{
			animator.SetBool("IsInAir", true);
			//animator.SetBool("IsStartJump", false);
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{

		if (wantsToThrow && haveBall)
		{
			if (CalculateThrow(GameManager.Instance.gmPlayers[lastPlayerTarget]))
			{
				currentState = State.Throw;
				wantsToThrow = false;
			}
		}
		ExecuteState();
		MovementVelocity();
		AnimationControl();
		characterType.CHFixedUpdate();
		if (IsHoldingBall && ballHolding == null)
		{
			ReleaseBall();
		}
		UpdateTarget();

		prevAiPos = cache_tf.position;

		hadBalLLastFrame = haveBall;
	}

	private void sceneChangedOld()
	{
		if (currSceneOld == "PregameRoom" || currSceneOld == "VictoryRoom")
		{
			currentState = State.Idle;
		}
		else
		{
			currentState = State.Move;
		}
		lastSceneOld = currSceneOld;
	}

	private void sceneChangedDelegate(Scene prevScene, Scene nextScene)
	{
		if (nextScene.name == "PregameRoom" || nextScene.name == "VictoryRoom")
		{
			currentState = State.Idle;
		}
		else
		{
			currentState = State.Move;
		}
	}

	void UpdateTarget()
	{
		if (characterType is Gorilla)
		{
			MoveTarget = GameManager.Instance.gmBalls[0].transform.position;
		}
		else
		{
			if ((!IsTargetViable(MoveTarget) || (cache_tf.position - MoveTarget).magnitude < 0.5f) || MoveTarget == Vector3.zero || (!hadBalLLastFrame && haveBall))
			{
				viableTargets.Clear();
				float dirX = 0.0f;
				float dirY = 0.0f;

				if (haveBall)
				{
					dirX = (cache_tf.position - GameManager.Instance.gmPlayers[gorillaIndex].transform.position).x;
					dirY = (cache_tf.position - GameManager.Instance.gmPlayers[gorillaIndex].transform.position).y;
					dirX = dirX / Mathf.Abs(dirX);
					dirY = dirY / Mathf.Abs(dirY);
					foreach (GameObject T in GameManager.Instance.gmLevelObjectsScript.loPlatforms)
					{
						if (T.transform.position.y * dirY > cache_tf.position.y && T.transform.position.x * dirX > cache_tf.position.x)
						{
							viableTargets.Add(T.transform);
						}
					}
				}
				else
				{
					foreach (GameObject T in GameManager.Instance.gmLevelObjectsScript.loPlatforms)
					{
						foreach (Actor P in GameManager.Instance.gmPlayerScripts)
						{
							dirX = (cache_tf.position - P.transform.position).x;
							dirY = (cache_tf.position - P.transform.position).y;
							dirX = dirX / Mathf.Abs(dirX);
							dirY = dirY / Mathf.Abs(dirY);
							if (T.transform.position.y * dirY > cache_tf.position.y && T.transform.position.x * dirX > cache_tf.position.x)
							{
								viableTargets.Add(T.transform);
								break;
							}
						}
					}
				}
				if (viableTargets.Count != 0)
				{
					MoveTarget = viableTargets[Random.Range(0, viableTargets.Count - 1)].position;
					UpdateEndTarget();
				}
			}
		}
	}

	bool IsTargetViable(Vector3 target)
	{
		if (haveBall)
		{
			foreach (Actor P in GameManager.Instance.gmPlayerScripts)
			{
				if (P.characterType is Gorilla)
				{
					if ((P.transform.position - target).magnitude <= (cache_tf.position - target).magnitude)
					{
						gorillaIndex = P.playerIndex;
						return false;
					}
				}
			}
		}
		else
		{
			foreach (Actor P in GameManager.Instance.gmPlayerScripts)
			{
				if (P.haveBall)
				{
					if (target == GameManager.Instance.gmBalls[0].transform.position)
					{
						return false;
					}
				}
				if ((P.transform.position - target).magnitude <= 2.5f)
				{
					return false;
				}
			}
		}
		return true;
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
		return currentState;
	}

	State ExecuteCatch()
	{
		GameManager.Instance.gmInputs[inputIndex].mXY.x = 0.0f;
		GameManager.Instance.gmInputs[inputIndex].mCatch = true;
		StartCoroutine(RealisticInputCatch());
		int lowestPoints = 0;
		int secondLowest = 0;
		for (int i = 0; i < GameManager.Instance.TotalNumberofPlayers; ++i)
		{
			if (i != playerIndex)
			{
				if (lowestPoints == -1)
				{
					lowestPoints = i;
				}
				else if (lowestPoints >= GameManager.Instance.gmScoringManager.GetScore(i))
				{
					secondLowest = lowestPoints;
					lowestPoints = i;
				}
			}
		}

		if (lowestPoints != lastPlayerTarget && GameManager.Instance.gmPlayerScripts[lowestPoints].characterType is Monkey)
		{
			lastPlayerTarget = lowestPoints;
			if (CalculateThrow(GameManager.Instance.gmPlayers[lowestPoints]))
			{
				currentState = State.Throw;
			}
			else
			{
				wantsToThrow = true;
				//calc move target
				currentState = State.Move;
			}
		}
		else if (secondLowest != lastPlayerTarget && GameManager.Instance.gmPlayerScripts[secondLowest].characterType is Monkey)
		{
			lastPlayerTarget = secondLowest;
			if (CalculateThrow(GameManager.Instance.gmPlayers[secondLowest]))
			{
				currentState = State.Throw;
			}
			else
			{
				wantsToThrow = true;
				//calc move target
				currentState = State.Move;
			}
		}
		else
		{
			nearestSignTarget = GameManager.Instance.gmTargetManager.GetTargetAtIndex(0);
			foreach (Target T in GameManager.Instance.gmTargetManager.TargetGetter())
			{
				if ((T.transform.position - cache_tf.position).magnitude < (nearestSignTarget.transform.position - cache_tf.position).magnitude)
				{
					nearestSignTarget = T;
				}
			}
			if (CalculateThrow(nearestSignTarget.gameObject))
			{
				currentState = State.Throw;
			}
			else
			{
				wantsToThrow = true;
				//calc move target
				currentState = State.Move;
			}
		}
		if (!onCatchCoolDown)
		{
			onCatchCoolDown = true;
			StartCoroutine(WaitForCatchCoolDown());
		}
		return currentState;
	}

	State ExecuteThrow()
	{
		//Debug.Log("Throw");

		GameManager.Instance.gmInputs[inputIndex].mXY.x = aimDir.x;
		GameManager.Instance.gmInputs[inputIndex].mXY.y = aimDir.y;
		GameManager.Instance.gmInputs[inputIndex].mCatch = true;
		GameManager.Instance.gmInputs[inputIndex].mChargeThrow = true;
		GameManager.Instance.gmInputs[inputIndex].mCatchRelease = true;
		StartCoroutine(RealisticInputThrow());
		wantsToThrow = false;
		if (!onCatchCoolDown)
		{
			onCatchCoolDown = true;
			StartCoroutine(WaitForCatchCoolDown());
		}
		currentState = State.Idle;

		return currentState;
	}

	State ExecuteMove()
	{
		SamePosUpdate();
		isAtTargetX = CheckClose();

		if (!IsInAir || isClimbing)
		{
			if (isAtTargetX)//&& (currEndTarg.y > cache_tf.position.y - (centerToFeet + 0.5f) && currEndTarg.y < cache_tf.position.y + (centerToFeet + 0.5f))))
			{
				UpdateEndTarget();
				isAtTargetX = CheckClose();
				canJump = true;
			}
		}

		calcVar = cache_tf.position;
		calcVar.y += (maxJump - centerToFeet);
		Debug.DrawLine(cache_tf.position, currEndTarg, Color.red);
		Debug.DrawLine(currEndTarg, MoveTarget, Color.blue);
		Debug.DrawLine(cache_tf.position, calcVar, Color.yellow);
		//Debug.DrawLine(cache_tf.position, currEndTargBound, Color.green);

		if (!IsInAir)
		{
			xInput = (currEndTarg - cache_tf.position).normalized.x;
		}

		if (!IsAtMainTarget())
		{
			if (MoveTarget.y > cache_tf.position.y - centerToFeet)
			{

				if ((isAtTargetX && currEndTarg.y > cache_tf.position.y) && (!IsInAir && currEndTarg.y < (cache_tf.position.y + (maxJump - centerToFeet))))
				{
					//float dirmult = (currEndTargBound - currEndTarg).normalized.x;
					//dirmult = currEndTargBound.x + (dirmult * (approxJumpDist * 0.5f));
					//updateEndTargBeforeCoroutine = true;
					//currEndTarg.x = dirmult;
					//currEndTarg.y = cache_tf.position.y;
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
									GameManager.Instance.gmInputs[inputIndex].mJump = true;
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
										GameManager.Instance.gmInputs[inputIndex].mJump = true;
										canJump = false;
										StartCoroutine(RealisticInputJump());
									}
								}
								else if ((IsInAir && canClimb) && !isClimbing)
								{
									if (cache_tf.position.y >= currEndTargBound.y)
									{
										GameManager.Instance.gmInputs[inputIndex].mJump = true;
										canJump = false;
										StartCoroutine(RealisticInputJump());
										if (currEndTarg == currEndTargBound)
										{
											if (cache_tf.position.y < vineTarget.y)
											{
												currEndTargBound.y += ((vineTarget.y * 0.5f) - 1.0f);
											}
											else
											{
												currEndTargBound.y -= ((vineTarget.y * 0.5f) - 1.0f);
											}
										}
										currEndTarg = currEndTargBound;
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
										GameManager.Instance.gmInputs[inputIndex].mJump = true;
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
										GameManager.Instance.gmInputs[inputIndex].mJump = true;
										canJump = false;
										StartCoroutine(RealisticInputJump());
									}
								}
								else if ((IsInAir && canClimb) && !isClimbing)
								{
									if (isAtTargetX)
									{
										GameManager.Instance.gmInputs[inputIndex].mJump = true;
										canJump = false;
										StartCoroutine(RealisticInputJump());
										if(currEndTarg == currEndTargBound)
										{
											
											if (cache_tf.position.y < vineTarget.y)
											{
												currEndTargBound.y += ((vineTarget.y * 0.5f) - 1.0f);
											}
											else
											{
												currEndTargBound.y -= ((vineTarget.y * 0.5f) - 1.0f);
											}
												
											
										}
										currEndTarg = currEndTargBound;
									}
								}
							}
						}
					}
				}

			}
			else if (MoveTarget.y < cache_tf.position.y - centerToFeet)
			{
				//if (isAtTargetX && !IsInAir)
				//{
				//	float dirmult = (currEndTargBound - currEndTarg).normalized.x;
				//	updateEndTargBeforeCoroutine = true;
				//	currEndTarg.x = currEndTargBound.x + (2f + (myCollider.size.x * 0.5f)) * dirmult;
				//	currEndTarg.y = currEndTargBound.y;
				//}
			}
			else
			{
				xInput = (MoveTarget - cache_tf.position).normalized.x;
			}
		}

		if (isStuck)
		{
			xInput = reverseX;
		}

		if (xInput < 0.05f && xInput > -0.05f)
		{
			xInput = 0.0f;
		}

		if (isEndTargVine && isClimbing)
		{
			GameManager.Instance.gmInputs[inputIndex].mXY.y = (currEndTarg - cache_tf.position).normalized.y;
		}
		GameManager.Instance.gmInputs[inputIndex].mXY.x = xInput;

		return currentState;
	}

	private void SamePosUpdate()
	{
		if (cache_tf.position == prevAiPos)
		{
			timeInSamePos += Time.deltaTime;
		}
		if (timeInSamePos >= 1.0f)
		{
			timeInSamePos = 0.0f;
			if (!isAtTargetX)
			{
				isStuck = true;
				StartCoroutine(RealisticInputX());
				reverseX = -xInput;
			}
			else
			{
				Debug.Log("Cannot make it to end target");
			}
		}
	}

	private bool CheckClose()
	{
		return (cache_tf.position.x < currEndTarg.x + 0.5f && cache_tf.position.x > currEndTarg.x - 0.5f);
	}

	private void UpdateEndTarget()
	{
		if (!isTargetCoroutineRunning)
		{
			StartCoroutine(WaitToCheckEndTarg());
		}
		else
		{
			updateEndTargBeforeCoroutine = true;
		}

		if (!IsAtMainTarget())
		{
			
			if (MoveTarget.y > cache_tf.position.y + centerToFeet)
			{
				FindNearestLevelObject(MoveTarget, currEndTarg);
				prevCurrEndTarg = currEndTarg;
				if (isEndTargVine)
				{
					vineTarget = currEndTarg;
					currEndTarg = FindEdgeOfVine(currEndTarg);
				}
				if (currEndTarg.y > (cache_tf.position.y + (maxJump - centerToFeet)))
				{
					levelCounter = GameManager.Instance.gmLevelObjectsScript.numberOfLevels;
					while (currEndTarg.y > (cache_tf.position.y + (maxJump - centerToFeet)) && levelCounter > 0)
					{
						FindNearestLevelObject(currEndTarg, currEndTarg);
						if (isEndTargVine)
						{
							vineTarget = currEndTarg;
							currEndTarg = FindEdgeOfVine(currEndTarg);
						}
						if (currEndTarg == prevCurrEndTarg)
						{
							FindNearestJumpableObject();
							break;
						}
						levelCounter--;
						prevCurrEndTarg = currEndTarg;
					}
				}
				if (!isEndTargVine)
				{
					currEndTargBound = FindEdgeOfPlatform(currEndTarg);
				}
				else
				{
					currEndTarg.z = 0.0f;
				}
			}
			else if (MoveTarget.y <= cache_tf.position.y - centerToFeet)
			{
				FindNearestLevelObject(MoveTarget,currEndTarg);
				if (!isEndTargVine)
				{
					currEndTargBound = FindEdgeOfPlatform(currEndTarg);
				}
				else
				{
					currEndTarg.z = 0.0f;
				}
			}
		}
		if (currEndTarg == Vector3.zero)
		{
			currEndTarg = MoveTarget;
			currEndTargBound = MoveTarget;
		}

	}

	private bool IsAtMainTarget()
	{
		return ((cache_tf.position.x < MoveTarget.x + 0.5f && cache_tf.position.x > MoveTarget.x - 0.5f) && (MoveTarget.y > cache_tf.position.y - (centerToFeet + 0.5f) && MoveTarget.y < cache_tf.position.y + (centerToFeet + 0.5f)));
	}

	private void FindNearestLevelObject(Vector3 FinalPos, Vector3 oldEndTarg)
	{
		Vector3 position;
		currClosestDist = 0.0f;
		float dist;
		for (float i = 0.0f; i <= 1.0f; i += 0.05f)
		{
			position = Vector3.Lerp(cache_tf.position, FinalPos, i);
			foreach (var T in GameManager.Instance.gmLevelObjectsScript.loPlatforms)
			{
				if (T.transform.position != FinalPos && !((cache_tf.position.x < T.transform.position.x + 1.0f && cache_tf.position.x > T.transform.position.x - 1.0f) && (T.transform.position.y > cache_tf.position.y - (centerToFeet + 1.0f) && T.transform.position.y < cache_tf.position.y + (centerToFeet + 1.0f))))
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
			foreach (var Y in GameManager.Instance.gmLevelObjectsScript.loVines)
			{
				if (Y.transform.position != FinalPos && !((cache_tf.position.x < Y.transform.position.x + 1.0f && cache_tf.position.x > Y.transform.position.x - 1.0f) && (Y.transform.position.y > cache_tf.position.y - (centerToFeet + 1.0f) && Y.transform.position.y < cache_tf.position.y + (centerToFeet + 1.0f))))
				{
					if (Y.transform.position.y < FinalPos.y)
					{
						dist = (Y.transform.position - position).magnitude;
						if (dist < currClosestDist)
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

	private void FindNearestJumpableObject()
	{
		
		List<Vector3> viableEndTargets = new List<Vector3>();
		List<bool> isVines = new List<bool>();
		Vector3 currClosest = Vector3.zero;
		int i = 0;
		foreach (var T in GameManager.Instance.gmLevelObjectsScript.loPlatforms)
		{
			if (T.transform.position.y < cache_tf.position.y + (maxJump - centerToFeet) && T.transform.position != currEndTarg)
			{
				viableEndTargets.Add(T.transform.position);
				isVines.Add(false);
			}
		}
		foreach (var Y in GameManager.Instance.gmLevelObjectsScript.loVines)
		{
			if (Y.transform.position.y < cache_tf.position.y + (maxJump - centerToFeet) && Y.transform.position != currEndTarg)
			{
				viableEndTargets.Add(FindEdgeOfVine(Y.transform.position));
				isVines.Add(true);
			}
		}
		foreach (var V in viableEndTargets)
		{
			if (currClosest == Vector3.zero)
			{
				currClosest = V;
				isEndTargVine = isVines[i];
			}
			else if ((MoveTarget - currClosest).magnitude > (MoveTarget - V).magnitude)
			{
				currClosest = V;
				isEndTargVine = isVines[i];
			}
			++i;
		}
		currEndTarg = currClosest;
		if (isEndTargVine)
		{
			vineTarget = currEndTarg;
			currEndTarg = FindEdgeOfVine(currEndTarg);
		}
	}

	private Vector3 FindEdgeOfPlatform(Vector3 platPos)
	{
		Vector3 result = Vector3.zero;
		foreach (var T in GameManager.Instance.gmLevelObjectsScript.loPlatforms)
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
		currEndTargBound = vinePos;
		foreach (var Y in GameManager.Instance.gmLevelObjectsScript.loVines)
		{
			if (vinePos == Y.transform.position)
			{
				if (cache_tf.position.y < Y.transform.position.y)
				{
					result.y -= ((Y.transform.lossyScale.y * 0.5f) - 1.0f);
					currEndTargBound.y += ((Y.transform.lossyScale.y * 0.5f) - 1.0f);
				}
				else
				{
					result.y += ((Y.transform.lossyScale.y * 0.5f) - 1.0f);
					currEndTargBound.y -= ((Y.transform.lossyScale.y * 0.5f) - 1.0f);
				}
			}
		}
		return result;
	}

	private void IsBallNear()
	{
		if (ballInRange)
		{
			if (letCatch)
			{
				currentState = State.Catch;
				letCatch = false;
			}
			else if (waitForBallOnce)
			{
				StartCoroutine(WaitToCatch());
				waitForBallOnce = false;
			}
		}
		else if (GameManager.Instance.gmBalls[0] != null)
		{
			if ((GameManager.Instance.gmBalls[0].transform.position - cache_tf.position).magnitude < (currEndTarg - cache_tf.position).magnitude)
			{
				MoveTarget = GameManager.Instance.gmBalls[0].transform.position;
			}
		}
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

	private bool CalculateThrow(GameObject playerTarg)
	{
		aimDir = Vector3.zero;
		posOnParabola = Vector2.zero;
		prevPosOnParabola = cache_tf.position;
		g = cache_rb.gravityScale * Physics2D.gravity.magnitude;
		throwVelocity = characterType.throwForce;
		range = (playerTarg.transform.position.x - cache_tf.position.x);
		if (range != 0.0f && (cache_tf.position.x <= playerTarg.transform.position.x - 0.3f || cache_tf.position.x >= playerTarg.transform.position.x + 0.3f))
		{
			directionOfParabola = range / Mathf.Abs(range);
		}
		else
		{
			return false;
		}
		height = (playerTarg.transform.position.y - cache_tf.position.y);
		posTheta = 0.0f;
		negTheta = 0.0f;
		if (((throwVelocity * throwVelocity) * (throwVelocity * throwVelocity)) - g * (g * (range * range) + 2 * height * (throwVelocity * throwVelocity)) > 0.0f)
		{
			posTheta = Mathf.Atan(((throwVelocity * throwVelocity) + Mathf.Sqrt(((throwVelocity * throwVelocity) * (throwVelocity * throwVelocity)) - g * (g * (range * range) + 2 * height * (throwVelocity * throwVelocity)))) / (g * range));
			negTheta = Mathf.Atan(((throwVelocity * throwVelocity) - Mathf.Sqrt(((throwVelocity * throwVelocity) * (throwVelocity * throwVelocity)) - g * (g * (range * range) + 2 * height * (throwVelocity * throwVelocity)))) / (g * range));
		}
		else
		{
			return false;
		}

		xVel1 = throwVelocity * Mathf.Cos(posTheta);
		yVel1 = throwVelocity * Mathf.Sin(posTheta);
		t1 = 0.0f;
		if (directionOfParabola > 0)
		{
			if ((yVel1 * yVel1) - (4 * (0.5f * g) * height) > 0)
			{
				t1 = ((directionOfParabola * yVel1 + Mathf.Sqrt((yVel1 * yVel1) - (4 * (0.5f * g) * height))) / (2 * (0.5f * g)));
			}
			else
			{
				return false;
			}
		}
		else
		{
			if ((yVel1 * yVel1) - (4 * (0.5f * g) * height) > 0)
			{
				t1 = ((directionOfParabola * yVel1 - Mathf.Sqrt((yVel1 * yVel1) - (4 * (0.5f * g) * height))) / (2 * (0.5f * g)));
			}
			else
			{
				return false;
			}
		}

		xVel2 = throwVelocity * Mathf.Cos(negTheta);
		yVel2 = throwVelocity * Mathf.Sin(negTheta);
		t2 = 0.0f;
		if (directionOfParabola > 0)
		{
			if ((yVel2 * yVel2) - (4 * (0.5f * g) * height) > 0)
			{
				t2 = ((directionOfParabola * yVel2 + Mathf.Sqrt((yVel2 * yVel2) - (4 * (0.5f * g) * height))) / (2 * (0.5f * g)));
			}
			else
			{
				return false;
			}
		}
		else
		{
			if ((yVel2 * yVel2) - (4 * (0.5f * g) * height) > 0)
			{
				t2 = ((directionOfParabola * yVel2 - Mathf.Sqrt((yVel2 * yVel2) - (4 * (0.5f * g) * height))) / (2 * (0.5f * g)));
			}
			else
			{
				return false;
			}
		}

		upOrDownThrow = 1;
		for (float i = 0.0f; i < Mathf.Abs(t1) || i < 10f; i += 0.1f)
		{
			posOnParabola.x = cache_tf.position.x + (directionOfParabola * xVel1 * i);
			posOnParabola.y = cache_tf.position.y + (directionOfParabola * yVel1 * i + 0.5f * -g * (i * i));
			lineHit = Physics2D.Linecast(prevPosOnParabola, posOnParabola, ballLayer);
			Debug.DrawLine(prevPosOnParabola, posOnParabola, Color.black, 0.5f);
			if (lineHit.collider != null)
			{
				if (lineHit.transform.tag == "Floor" || lineHit.transform.tag == "Wall")
				{
					aimDir = Vector3.zero;
					upOrDownThrow = 0;
					break;
				}
			}
			if (posOnParabola.x >= range - 0.3f && posOnParabola.x <= range + 0.3f)
			{
				break;
			}
			prevPosOnParabola = posOnParabola;
		}


		if (upOrDownThrow == 0)
		{
			prevPosOnParabola = cache_tf.position;
			upOrDownThrow = -1;
			for (float i = 0.0f; i < Mathf.Abs(t2) || i < 10f; i += 0.1f)
			{
				posOnParabola.x = cache_tf.position.x + (directionOfParabola * xVel2 * i);
				posOnParabola.y = cache_tf.position.y + (directionOfParabola * yVel2 * i + 0.5f * -g * (i * i));
				lineHit = Physics2D.Linecast(prevPosOnParabola, posOnParabola, ballLayer);
				Debug.DrawLine(prevPosOnParabola, posOnParabola, Color.gray, 0.5f);
				if (lineHit.collider != null)
				{
					if (lineHit.transform.tag == "Floor" || lineHit.transform.tag == "Wall")
					{
						aimDir = Vector3.zero;
						upOrDownThrow = 0;
						break;
					}
				}
				if (posOnParabola.x >= range - 0.3f && posOnParabola.x <= range + 0.3f)
				{
					break;
				}
				prevPosOnParabola = posOnParabola;
			}
		}

		if (upOrDownThrow == 1)
		{
			aimDir.x = (xVel1 * directionOfParabola) / throwVelocity;
			aimDir.y = (yVel1 * directionOfParabola) / throwVelocity;
			return true;
		}
		else if (upOrDownThrow == -1)
		{
			aimDir.x = (xVel2 * directionOfParabola) / throwVelocity;
			aimDir.y = (yVel2 * directionOfParabola) / throwVelocity;
			return true;
		}
		return false;
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
		GameManager.Instance.gmInputs[inputIndex].mJump = false;
	}

	IEnumerator RealisticInputCatch()
	{
		yield return new WaitForSeconds(0.3f);
		GameManager.Instance.gmInputs[inputIndex].mCatch = false;
	}

	IEnumerator RealisticInputThrow()
	{
		yield return new WaitForSeconds(0.05f);
		GameManager.Instance.gmInputs[inputIndex].mCatch = false;
		yield return new WaitForSeconds(0.05f);
		GameManager.Instance.gmInputs[inputIndex].mChargeThrow = false;
		GameManager.Instance.gmInputs[inputIndex].mCatchRelease = false;
		yield return new WaitForSeconds(0.3f);
		currentState = State.Move;
	}

	IEnumerator RealisticInputX()
	{
		yield return new WaitForSeconds(0.3f);
		isStuck = false;
	}

	IEnumerator WaitToCatch()
	{
		yield return new WaitForSeconds(catchWaitTime);
		letCatch = true;
		waitForBallOnce = true;
	}

	IEnumerator WaitForCatchCoolDown()
	{
		yield return new WaitForSeconds(2.0f);
		onCatchCoolDown = false;
	}

	IEnumerator WaitToCheckEndTarg()
	{
		isTargetCoroutineRunning = true;
		yield return new WaitForSeconds(5.0f);
		isTargetCoroutineRunning = false;
		if (!updateEndTargBeforeCoroutine)
		{
			UpdateEndTarget();
		}
		else
		{
			updateEndTargBeforeCoroutine = false;
		}
	}
}
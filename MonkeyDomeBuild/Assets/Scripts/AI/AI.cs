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
	private bool canJump;

	State currentState;

	// Use this for initialization
	void Start ()
	{
        currentState = State.Idle;

		myCollider = GetComponent<BoxCollider2D>();
		cache_tf = GetComponent<Transform>();
		cache_rb = GetComponent<Rigidbody2D>();
		tempTarg = GameObject.FindGameObjectWithTag("TempTarget");
		canJump = true;

		CalculateMaxJump();
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
		UpdateColour();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
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
            default :
                Debug.LogError("YOU FUCKING BROKE IT!");
                break;
        }

    }

    State ExecuteIdle()
    {
        if(GameManager.Instance.gmUIManager.matchTime < GameManager.Instance.gmUIManager.startMatchTime)
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
		UpdateTarget();

		if(IsInAir && !canJump)
		{
			GameManager.Instance.gmInputs[playerIndex].mJump = false;
		}

		if (characterType is Monkey) //TODO Monkey Move Logic
        {
			if (MoveTarget.y > cache_tf.position.y)
			{
				FindNearestPlatform(MoveTarget);
				if (MoveTarget.y > currEndTarg.y)
				{
					while (currEndTarg.y > cache_tf.position.y + maxJump - (myCollider.size.y - myCollider.offset.y))
					{
						FindNearestPlatform(currEndTarg);
					}
				}
				currEndTargBound = FindEdgeOfPlatform(currEndTarg);

				if (currEndTargBound.x <= cache_tf.transform.position.x - approxJumpDist)
				{
					xInput = CalculateJump(currEndTargBound, false) / characterType.movespeed;
					if ((xInput <= 1.0f && xInput >= -1.0f) && canJump)
					{
						GameManager.Instance.gmInputs[playerIndex].mJump = true;
						canJump = false;
						StartCoroutine(JumpWait());
					}
					else
					{
						xInput = (currEndTarg - cache_tf.position).normalized.x;
					}
				}
				else if (currEndTargBound.x >= cache_tf.transform.position.x + approxJumpDist)
				{
					xInput = CalculateJump(currEndTargBound, true) / characterType.movespeed;
					if ((xInput <= 1.0f && xInput >= -1.0f) && canJump)
					{
						GameManager.Instance.gmInputs[playerIndex].mJump = true;
						canJump = false;
						StartCoroutine(JumpWait());
					}
					else
					{
						xInput = (currEndTarg - cache_tf.position).normalized.x;
					}
				}
			}
			GameManager.Instance.gmInputs[playerIndex].mXY.x = xInput;
		}
        else //TODO Gorilla Move Logic
        {

        }
        return currentState;
    }

	private void UpdateTarget()
	{
		MoveTarget = tempTarg.transform.position;
	}

	private void FindNearestPlatform(Vector3 FinalPos)
	{
		Vector3 position;
		currClosestDist = 0.0f;
		float dist;
		for(float i = 0.0f; i <= 1.0f; i += 0.05f)
		{
			position = Vector3.Lerp(cache_tf.position, FinalPos, i);
			foreach(var T in GameManager.Instance.gmLevelObjectScript.loPlatforms)
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
			if(platPos == T.transform.position)
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

	private float CalculateJump(Vector3 jumpLanding,bool dir) // right is true
	{
		float dx;
		float t;
		if(dir)
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
		float g = cache_rb.gravityScale * Physics2D.gravity.magnitude;
		jumpVelocity = characterType.jumpforce / cache_rb.mass;
		maxJump = (jumpVelocity * jumpVelocity) / (2 * g);
	}

	IEnumerator JumpWait()
	{
		yield return new WaitForSeconds(0.5f);
		canJump = true;
	}
}

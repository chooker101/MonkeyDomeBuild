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

	[SerializeField]
	private Vector3 MoveTarget;

	//private Vector3 ToTarget;

	[SerializeField]
	private Vector3 currEndTarg;

	private float currClosestDist;
	private float maxJump;

	State currentState;

	// Use this for initialization
	void Start ()
	{
        currentState = State.Idle;

		myCollider = GetComponent<BoxCollider2D>();
		cache_tf = GetComponent<Transform>();
		cache_rb = GetComponent<Rigidbody2D>();
		tempTarg = GameObject.FindGameObjectWithTag("TempTarget");

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
		if (characterType is Monkey) //TODO Monkey Move Logic
        {
			if(MoveTarget.y > cache_tf.position.y)
			{
				FindNearestPlatform(MoveTarget);
				if (MoveTarget.y > currEndTarg.y)
				{
					while (currEndTarg.y > cache_tf.position.y + maxJump - (myCollider.size.y - myCollider.offset.y))
					{
						FindNearestPlatform(currEndTarg);
					}
					GameManager.Instance.gmInputs[playerIndex].mXY.x = (currEndTarg - cache_tf.position).normalized.x;
				}
			}
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

	private void CalculateMaxJump()
	{
		float g = cache_rb.gravityScale * Physics2D.gravity.magnitude;
		float iv = characterType.jumpforce / cache_rb.mass;
		maxJump = (iv * iv) / (2 * g);
	}
}

using UnityEngine;
using System.Collections;

public class Monkey : Character
{
	private int myPlayer;
	private Player cacheplayer;

	void Awake()
	{
		myPlayer = this.gameObject.GetComponent<Actor>().whichplayer;
		moveForce = 100f;
		jumpForce = 65f;
		speedLimit = 12f;
		throwForce = 40f;
		downForce = 80f;
		tempDownForce = downForce;
		downForceIncrement = 100f; // per second
		maxDownForce = 200f;
		climbForce = 200f; ;
		climbSpeedLimit = speedLimit;
		normalDrag = 8f;
		climbDrag = 12f;
		cacheplayer = GameManager.Instance.gmPlayers[myPlayer].GetComponent<Player>();
	}

	void Update()
	{
		cacheplayer.CheckInputs();
		cacheplayer.JumpCheck();
		cacheplayer.Aim();
		if (cacheplayer.haveBall)
		{
			cacheplayer.ThrowCheck();
		}
		else
		{
			CatchCheck();
		}
		cacheplayer.mov = cacheplayer.GetComponent<Rigidbody>().velocity;
	}

	void FixedUpdate()
	{
		cacheplayer.Movement();
	}
	protected void CatchCheck()
	{
		if (GameManager.Instance.gmInputs[myPlayer].mCatch && GameManager.Instance.gmBall != null)
		{
			if (!cacheplayer.haveBall && cacheplayer.ballInRange)
			{
				cacheplayer.haveBall = true;
				cacheplayer.ballHolding = GameManager.Instance.gmBall;
				cacheplayer.ballHolding.GetComponent<BallInfo>().UpdateLastThrowMonkey(gameObject);
				Rigidbody ballRigid = cacheplayer.ballHolding.GetComponent<Rigidbody>();
				ballRigid.useGravity = false;
				ballRigid.isKinematic = true;
				ballRigid.position = transform.position;
				cacheplayer.ballHolding.transform.SetParent(transform);
				cacheplayer.stat_ballGrab++;
			}
		}
	}

	public void Mutate()
	{
		
		
		if (cacheplayer.haveBall && cacheplayer.ballHolding != null)
		{
			cacheplayer.haveBall = false;
			cacheplayer.ballHolding.GetComponent<BallInfo>().Reset();
			cacheplayer.ballHolding = null;
		}
		GameObject tempGorilla = (GameObject)Instantiate(GameManager.Instance.gmPlayerPrefab, cacheplayer.GetComponent<Rigidbody>().position, cacheplayer.GetComponent<Rigidbody>().rotation);

		tempGorilla.GetComponent<Renderer>().material = GameManager.Instance.gmPlayers[myPlayer].GetComponent<Renderer>().material;

		cacheplayer = tempGorilla.GetComponent<Player>();

		cacheplayer.characterType = tempGorilla.AddComponent<Gorilla>();

		//List<GameObject> allPlayer = GameManager.Instance.gmPlayers;
		for (int i = 0; i < GameManager.Instance.gmPlayers.Capacity; i++)
			{
				if (GameManager.Instance.gmPlayers[i].GetInstanceID() == gameObject.GetInstanceID())
				{
					GameManager.Instance.gmPlayers[i] = tempGorilla;
					break;
				}
			}
			Destroy(gameObject);
		
	}
}
using UnityEngine;
using System.Collections;

public class Monkey : Character
{
	private int myPlayer;
	private Player cacheplayer;

	public Monkey(int x)
	{
		myPlayer = x;
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

	
	public override void CHUpdate()
	{
		if (cacheplayer.haveBall)
		{
			cacheplayer.ThrowCheck();
		}
		else
		{
			CatchCheck();
		}
	}

	public override void CHFixedUpdate()
	{
		
	}

	protected void CatchCheck()
	{
		if (GameManager.Instance.gmInputs[myPlayer].mCatch && GameManager.Instance.gmBall != null)
		{
			if (!cacheplayer.haveBall && cacheplayer.ballInRange)
			{
				cacheplayer.haveBall = true;
				cacheplayer.ballHolding = GameManager.Instance.gmBall;
				cacheplayer.ballHolding.GetComponent<BallInfo>().UpdateLastThrowMonkey(cacheplayer.gameObject);
				Rigidbody ballRigid = cacheplayer.ballHolding.GetComponent<Rigidbody>();
				ballRigid.useGravity = false;
				ballRigid.isKinematic = true;
				ballRigid.position = cacheplayer.transform.position;
				cacheplayer.ballHolding.transform.SetParent(cacheplayer.transform);
				cacheplayer.stat_ballGrab++;
			}
		}
	}

	public override void Mutate()
	{
		
		if (cacheplayer.haveBall && cacheplayer.ballHolding != null)
		{
			cacheplayer.haveBall = false;
			cacheplayer.ballHolding.GetComponent<BallInfo>().Reset();
			cacheplayer.ballHolding = null;
		}
		cacheplayer.characterType = new Gorilla(myPlayer);
		cacheplayer.GetComponent<Transform>().localScale = gorillaSize;
		/*
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
			Destroy(gameObject);*/

	}
}
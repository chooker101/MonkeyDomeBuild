using UnityEngine;
using System.Collections;

public class Monkey : Character
{
	private int myPlayer;
	private Player cacheplayer;

	public Monkey(int x)
	{
		myPlayer = x;
		horizontalMoveForce = 50f;
		jumpForce = 22f;
		speedLimit = 13f;
		throwForce = 25f;
		downForce = 80f;
		tempDownForce = downForce;
		downForceIncrement = 30f; // per second
		maxDownForce = 200f;
		climbingHorizontalMoveSpeed = 10f;
        climbingVerticalMoveSpeed = 10f;
        climbSpeedLimit = speedLimit;
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
                cacheplayer.ballHolding.GetComponent<BallInfo>().BeingCatch(cacheplayer.gameObject);
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
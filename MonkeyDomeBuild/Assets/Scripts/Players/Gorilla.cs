﻿using UnityEngine;
using System.Collections;

public class Gorilla : Character
{
	private int myPlayer;
	private Player cacheplayer;
	private float timeBeingGorilla = 0f;

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
		timeBeingGorilla += Time.deltaTime;
		cacheplayer.CheckInputs();
		cacheplayer.JumpCheck();
		cacheplayer.Aim();
		CatchCheck();
		StompCheck();
		cacheplayer.mov = cacheplayer.GetComponent<Rigidbody>().velocity;
	}
	void FixedUpdate()
	{
		cacheplayer.Movement();
	}

	protected void CatchCheck()
	{
		if (GameManager.Instance.gmInputs[cacheplayer.whichplayer].mCatch && GameManager.Instance.gmBall != null)
		{
			if (cacheplayer.ballInRange)
			{
				cacheplayer.ballHolding = GameManager.Instance.gmBall;
				cacheplayer.ballHolding.GetComponent<Rigidbody>().position += Vector3.up * 2;
				cacheplayer.ballHolding.GetComponent<BallInfo>().Change();
				cacheplayer.stat_ballGrab++;
			}
		}
	}

	public void Mutate()
	{
		GameObject tempMonkey = (GameObject)Instantiate(GameManager.Instance.gmPlayerPrefab, cacheplayer.GetComponent<Rigidbody>().position, cacheplayer.GetComponent<Rigidbody>().rotation);

		tempMonkey.GetComponent<Renderer>().material = GameManager.Instance.gmPlayers[myPlayer].GetComponent<Renderer>().material;

		cacheplayer = tempMonkey.GetComponent<Player>();

		cacheplayer.characterType = tempMonkey.AddComponent<Monkey>();

		//List<GameObject> allPlayer = GameManager.Instance.gmPlayers;
		for (int i = 0; i < GameManager.Instance.gmPlayers.Capacity; i++)
		{
			if (GameManager.Instance.gmPlayers[i].GetInstanceID() == gameObject.GetInstanceID())
			{
					GameManager.Instance.gmPlayers[i] = tempMonkey;
					break;
			}
		}
		Destroy(gameObject);
	}

	public override float GetTimeBeingGorilla()
	{
		return timeBeingGorilla;
	}
	protected void StompCheck()
	{
		if (GameManager.Instance.gmInputs[myPlayer].mAimStomp)
		{
			for(int i = 0;i < GameManager.Instance.gmPlayers.Capacity; ++i)
			{
				Character p = GameManager.Instance.gmPlayers[i].GetComponent<Character>();
				if (p is Monkey)
				{
					//knock both player off vine for now
					GameManager.Instance.gmPlayers[i].GetComponent<Player>().isClimbing = false;
				}
			}
		}
	}
}

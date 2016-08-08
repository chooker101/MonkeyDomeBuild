using UnityEngine;
using System.Collections;

public class Gorilla : Character
{
	private int myPlayer;
	private Player cacheplayer;
	private float timeBeingGorilla = 0f;

	public Gorilla(int x)
	{
		myPlayer = x;
		horizontalMoveForce = 50f;
		jumpForce = 25f;
		speedLimit = 12f;
		throwForce = 20f;
		downForce = 1000f;
		tempDownForce = downForce;
		downForceIncrement = 100f; // per second
		maxDownForce = 200f;
		climbingHorizontalMoveSpeed = 10f;
        climbingVerticalMoveSpeed = 10f;
        climbSpeedLimit = speedLimit;
		cacheplayer = GameManager.Instance.gmPlayers[myPlayer].GetComponent<Player>();
	}

	public override void CHUpdate()
	{
		timeBeingGorilla += Time.deltaTime;
		
		CatchCheck();
		StompCheck();
		
	}

	public override void CHFixedUpdate()
	{
		
	}

	protected void CatchCheck()
	{
		if (GameManager.Instance.gmInputs[myPlayer].mCatch && GameManager.Instance.gmBall != null)
		{
			if (cacheplayer.ballInRange)
			{
				cacheplayer.ballHolding = GameManager.Instance.gmBall;
				cacheplayer.ballHolding.GetComponent<Rigidbody2D>().position += Vector2.up * 2;
				cacheplayer.ballHolding.GetComponent<BallInfo>().Change(myPlayer);
				cacheplayer.stat_ballGrab++;
			}
		}
	}

	public override void Mutate()
	{
		cacheplayer.characterType = new Monkey(myPlayer);
		cacheplayer.GetComponent<Transform>().localScale = monkeySize;
		/*
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
		*/
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
                Character p = GameManager.Instance.gmPlayers[i].GetComponent<Actor>().characterType;
				if (p is Monkey)
				{
					//knock both player off vine for now
					GameManager.Instance.gmPlayers[i].GetComponent<Player>().isClimbing = false;
				}
			}
            cacheplayer.cam.ScreenShake();
		}
	}
}

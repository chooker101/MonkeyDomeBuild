using UnityEngine;
using System.Collections;

public class Gorilla : Character
{
	private int myPlayer;
	private Player cacheplayer;
	private float timeBeingGorilla = 0f;
    private float chargeCount = 0f;
    private float chargeCompleteTime = 3f;
    private bool canStomp = false;
    private bool isCharging = false;
    private GorillaCharge chargeUI;

	public Gorilla(int x)
	{
		myPlayer = x;
		throwForce = GameManager.Instance.gmMovementManager.gThrowForce;
		jumpforce = GameManager.Instance.gmMovementManager.gJumpForce;
		movespeed = GameManager.Instance.gmMovementManager.gSpeed;
		cacheplayer = GameManager.Instance.gmPlayers[myPlayer].GetComponent<Player>();
        chargeUI = cacheplayer.gameObject.GetComponentInChildren<Canvas>().gameObject.GetComponent<GorillaCharge>();
        if (chargeUI != null)
            chargeUI.MaxChargeTime = chargeCompleteTime;
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
                ScoringManager.Instance.SwitchingScore(GameManager.Instance.gmPlayers[myPlayer], GameManager.Instance.gmBall);
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
    public bool IsCharging
    {
        get { return isCharging; }
    }
	protected void StompCheck()
	{
		if (GameManager.Instance.gmInputs[myPlayer].mAimStomp && canStomp && !cacheplayer.IsInAir)
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
            canStomp = false;
            cacheplayer.cam.ScreenShake();
		}
        else if (GameManager.Instance.gmInputs[myPlayer].mChargeStomp && !canStomp)
        {
            isCharging = true;
            if (chargeCount >= chargeCompleteTime)
            {
                canStomp = true;
                isCharging = false;
            }
            else
            {
                chargeCount += Time.deltaTime;
                if (chargeUI != null)
                    chargeUI.ChargeCount = chargeCount;
            }
        }
        else
        {
            isCharging = false;
            chargeCount = 0;
            if (chargeUI != null)
                chargeUI.ChargeCount = chargeCount;
        }
	}
}

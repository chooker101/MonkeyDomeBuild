﻿using UnityEngine;
using System.Collections;

public class Gorilla : Character
{
	private float timeBeingGorilla = 0f;
    private float chargeCount = 0f;
    private float chargeCompleteTime = 1f;
    private bool canStomp = false;
    private GorillaCharge chargeUI;

	public Gorilla(int x)
	{
		myPlayer = x;
		throwForce = GameManager.Instance.gmMovementManager.gThrowForce;
		jumpforce = GameManager.Instance.gmMovementManager.gJumpForce;
		movespeed = GameManager.Instance.gmMovementManager.gSpeed;
		chargespeed = GameManager.Instance.gmMovementManager.gChargeSpeed;
		cacheplayer = GameManager.Instance.gmPlayers[myPlayer].GetComponent<Player>();
        chargeUI = cacheplayer.gameObject.GetComponentInChildren<Canvas>().gameObject.GetComponent<GorillaCharge>();
        chargespeed = movespeed / 2;
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
        if (GameManager.Instance.gmInputs[myPlayer].mCatch && cacheplayer.ballCanCatch != null)
        {
            if (!Physics2D.Raycast(cacheplayer.transform.position, cacheplayer.ballCanCatch.transform.position - cacheplayer.transform.position,
                Vector3.Distance(cacheplayer.transform.position, cacheplayer.ballCanCatch.transform.position), cacheplayer.layerMask))
            {
                if (cacheplayer.ballInRange)
                {
                    if (cacheplayer.ballCanCatch.GetComponent<BallInfo>().GetHoldingMonkey() != null)
                    {
                        GameManager.Instance.gmScoringManager.GorillaInterceptScore(GameManager.Instance.gmPlayers[myPlayer], cacheplayer.ballCanCatch.GetComponent<BallInfo>().GetHoldingMonkey());
                    }
                    cacheplayer.ballCanCatch.GetComponent<BallInfo>().Change(myPlayer);
                    cacheplayer.stat_ballGrab++;
					GameManager.Instance.gmScoringManager.SwitchingScore(GameManager.Instance.gmPlayers[myPlayer], cacheplayer.ballCanCatch.gameObject);
                }
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
		if (GameManager.Instance.gmInputs[myPlayer].mAimStomp && canStomp)
		{
            /*
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
            */
            if (Mathf.Abs(GameManager.Instance.gmInputs[myPlayer].mXY.x) > 0 || Mathf.Abs(GameManager.Instance.gmInputs[myPlayer].mXY.y) > 0)
            {
                cacheplayer.GorillaDash();
                canStomp = false;
            }
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

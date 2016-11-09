using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class Gorilla : Character
{
	private float timeBeingGorilla = 0f;
    private float chargeCount = 0f;
    private float chargeCompleteTime = 5f;
    private bool canDash = false;
    private bool justDashed = false;
    private float justDashedCount = 0;
    private float justDashedTime = 2f;
    private GorillaCharge chargeUI;

	public Gorilla(int x)
	{
		myPlayer = x;
		throwForce = GameManager.Instance.gmMovementManager.gThrowForce;
		jumpforce = GameManager.Instance.gmMovementManager.gJumpForce;
		movespeed = GameManager.Instance.gmMovementManager.gSpeed;
		chargespeed = GameManager.Instance.gmMovementManager.gChargeSpeed;
		cacheplayer = GameManager.Instance.gmPlayers[myPlayer].GetComponent<Actor>();
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

        if (GameManager.Instance.gmPlayerScripts[myPlayer].characterType is Gorilla)
        {
            if (cacheplayer.GetComponent<Transform>().localScale != GameManager.Instance.gmMovementManager.gScale)
            {
                cacheplayer.GetComponent<Transform>().localScale = GameManager.Instance.gmMovementManager.gScale;
            }
        }
        else
        {
            if (cacheplayer.GetComponent<Transform>().localScale != GameManager.Instance.gmMovementManager.mScale)
            {
                cacheplayer.GetComponent<Transform>().localScale = GameManager.Instance.gmMovementManager.mScale;
            }
        }

    }

	public override void CHFixedUpdate()
	{
		
	}

	protected void CatchCheck()
	{
        if (GameManager.Instance.gmInputs[myPlayer].mCatch && cacheplayer.ballCanCatch != null)
        {
            if (!Physics2D.Raycast(cacheplayer.catchCenter.position, cacheplayer.ballCanCatch.transform.position - cacheplayer.transform.position,
                Vector3.Distance(cacheplayer.catchCenter.position, cacheplayer.ballCanCatch.transform.position), cacheplayer.layerMask))
            {
                if (cacheplayer.ballInRange)
                {
                    if (cacheplayer.ballCanCatch.GetComponent<BallInfo>().IsBall)
                    {
                        // Checks to see if the current scene isn't the pre-game room
                        if (cacheplayer.ballCanCatch.GetComponent<BallInfo>().GetHoldingMonkey() != null && SceneManager.GetActiveScene().name != "PregameRoom")
                        {
							cacheplayer.ballCanCatch.GetComponent<BallInfo>().GetHoldingMonkey().GetComponent<Actor>().ReleaseBall();
							GameManager.Instance.gmScoringManager.GorillaInterceptScore(GameManager.Instance.gmPlayers[myPlayer], cacheplayer.ballCanCatch.GetHoldingMonkey(),cacheplayer.ballCanCatch.gameObject);
                            //On interception check for active audience interception event
                            //if (GameManager.Instance.gmAudienceManager.GetEventActive())
                            //{ 
                            //    if(GameManager.Instance.gmAudienceManager.GetCurrentEvent() == AudienceManager.AudienceEvent.Intercept)
                            //    {
                            //        GameManager.Instance.gmAudienceManager.AudGorillaIntercepted(GameManager.Instance.gmPlayers[myPlayer]);
                            //    }
                            //}
                        }
                        cacheplayer.CaughtBall(cacheplayer.ballCanCatch.gameObject);
                        cacheplayer.ballCanCatch.GetComponent<BallInfo>().Change(myPlayer);
                        cacheplayer.ballCanCatch.GetComponent<BallInfo>().BeingCatch(cacheplayer.gameObject);
                        cacheplayer.stat_ballGrab++;
                    }
                }
            }

        }
    }
	public override void Mutate()
	{
        cacheplayer.GorillaCatchReset();
        cacheplayer.characterType = new Monkey(myPlayer);
		//cacheplayer.GetComponent<Transform>().localScale = monkeySize;

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
        if (GameManager.Instance.gmInputs[myPlayer].mAimStomp && canDash)
        {
            manuallyCharging = false;
            justDashed = true;
            cacheplayer.GorillaDash();
            canDash = false;
        }
        else if (!canDash && !justDashed)
        {
            isCharging = true;
            if (chargeCount >= chargeCompleteTime)
            {
                manuallyCharging = false;
                canDash = true;
                isCharging = false;
            }
            else
            {
                if (GameManager.Instance.gmInputs[myPlayer].mChargeStomp)
                {
                    chargeCount += Time.deltaTime * 3f;
                    manuallyCharging = true;
                }
                else
                {
                    manuallyCharging = false;
                    chargeCount += Time.deltaTime;
                }
                if (chargeUI != null)
                    chargeUI.ChargeCount = chargeCount;
            }
        }
        else if (justDashed)
        {
            if (justDashedCount >= justDashedTime)
            {
                justDashed = false;
                justDashedCount = 0;
            }
            else
            {
                justDashedCount += Time.deltaTime;
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
    private void ResetJustDashed()
    {
        justDashed = false;
    }

}

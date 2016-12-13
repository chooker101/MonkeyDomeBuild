using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Monkey : Character
{
    private CallForBallReset callForBall;
	public Monkey(int x,int y)
	{
		myPlayer = x;
        myInput = y;
		throwForce = GameManager.Instance.gmMovementManager.mThrowForce;
		jumpforce = GameManager.Instance.gmMovementManager.mJumpForce;
		movespeed = GameManager.Instance.gmMovementManager.mSpeed;
		chargespeed = 0.0f;
		cacheplayer = GameManager.Instance.gmPlayers[myPlayer].GetComponent<Actor>();
        callForBall = cacheplayer.gameObject.GetComponentInChildren<Canvas>().gameObject.GetComponent<CallForBallReset>();
        //callForBallImg.SetActive(false);
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
            CallForBall();
		}

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
        //Debug.DrawLine(cacheplayer.transform.position, GameManager.Instance.gmBall.transform.position);
	}
    protected void CallForBall()
    {
        if (GameManager.Instance.gmInputs[myInput].mAimStomp && !callForBall.CallForBallActive && !cacheplayer.IsHoldingBall)
        {
            callForBall.CallForBall();
            GameManager.Instance.gmTrophyManager.CallsForBall(myPlayer);
            AudioEffectManager.Instance.PlayMonkeyCallBallSE();
        }
    }
	protected void CatchCheck()
	{
        if (GameManager.Instance.gmInputs[myInput].mCatch && cacheplayer.ballAround.Count > 0 && cacheplayer.CanCatch && !cacheplayer.IsStunned)
		{
            BallInfo ballToCatch = null;
            float dis = 10f;
			Debug.Log("run");
            for(int i = 0; i < cacheplayer.ballAround.Count; i++)
            {
                if (cacheplayer.ballAround[i].GetCanBeCatch())
                {
                    if (!Physics2D.Raycast(cacheplayer.catchCenter.position, cacheplayer.ballAround[i].transform.position - cacheplayer.transform.position,
                        Vector2.Distance(cacheplayer.catchCenter.position, cacheplayer.ballAround[i].transform.position), cacheplayer.layerMask))
                    {
                        if (Vector2.Distance(cacheplayer.catchCenter.position, cacheplayer.ballAround[i].transform.position) < dis)
                        {
                            ballToCatch = cacheplayer.ballAround[i];
                            dis = Vector2.Distance(cacheplayer.catchCenter.position, cacheplayer.ballAround[i].transform.position);
                        }
                    }
                }
            }
            if (ballToCatch != null)
            {
                if (!cacheplayer.haveBall)
                {
                    if (ballToCatch.BallType == ThrowableType.Trophy)
                    {
                        ballToCatch.GetComponent<TrophyInfo>().BeingCatch();
                    }
                    else if(ballToCatch.BallType == ThrowableType.Ball)
                    {
                        if (GameManager.Instance.IsInMatch)
                        {
                            GameManager.Instance.gmScoringManager.PassingScore(ballToCatch.lastThrowMonkey, cacheplayer.gameObject, ballToCatch);
                        }
                        //GameManager.Instance.gmScoringManager.PassingScore(lastThrowMonkey, who, distanceTravel, travelTime, perfectCatch, numberOfBounce);
                    }
                    cacheplayer.GetComponent<EffectControl>().PlayCatchEffect(ballToCatch.gameObject);
                    if (ballToCatch.IsPerfectCatch(cacheplayer))
                    {
                        cacheplayer.GetComponent<EffectControl>().PlayPerfectCatchEffect(ballToCatch.gameObject);
                    }
                    cacheplayer.CaughtBall(ballToCatch.gameObject);
                    ballToCatch.BeingCatch(cacheplayer.gameObject);
                    cacheplayer.stat_ballGrab++;
                }
            }
            /*
            if (cacheplayer.ballCanCatch.GetCanBeCatch())
            {
                if (!Physics2D.Raycast(cacheplayer.catchCenter.position, cacheplayer.ballCanCatch.transform.position - cacheplayer.transform.position,
                    Vector3.Distance(cacheplayer.catchCenter.position, cacheplayer.ballCanCatch.transform.position), cacheplayer.layerMask))
                {
                    if (!cacheplayer.haveBall && cacheplayer.ballInRange)
                    {
                        if (cacheplayer.ballCanCatch.BallType == ThrowableType.Trophy)
                        {
                            cacheplayer.ballCanCatch.GetComponent<TrophyInfo>().DisableCollider();
                        }
                        cacheplayer.GetComponent<EffectControl>().PlayCatchEffect(cacheplayer.ballCanCatch.gameObject);
                        if (cacheplayer.ballCanCatch.GetComponent<BallInfo>().IsPerfectCatch(cacheplayer))
                        {
                            cacheplayer.GetComponent<EffectControl>().PlayPerfectCatchEffect(cacheplayer.ballCanCatch.gameObject);
                        }
                        cacheplayer.CaughtBall(cacheplayer.ballCanCatch.gameObject);
                        cacheplayer.ballHolding.GetComponent<BallInfo>().BeingCatch(cacheplayer.gameObject);
                        cacheplayer.stat_ballGrab++;
                    }
                }
            }
            */
		}
	}

	public override void Mutate()
	{
        cacheplayer.GetComponent<EffectControl>().PlaySwitchEffect();
		if (cacheplayer.haveBall && cacheplayer.ballHolding != null)
		{
            cacheplayer.ReleaseBall();
		}
		cacheplayer.characterType = new Gorilla(myPlayer,myInput);
        cacheplayer.TempDisableInput(2f);
        //cacheplayer.GetComponent<Transform>().localScale = gorillaSize;


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
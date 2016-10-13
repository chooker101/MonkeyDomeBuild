using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Monkey : Character
{
    private CallForBallReset callForBall;
	public Monkey(int x)
	{
		myPlayer = x;
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
        if (GameManager.Instance.gmInputs[myPlayer].mAimStomp && !callForBall.CallForBallActive && !cacheplayer.IsHoldingBall)
        {
            callForBall.CallForBall();
        }
    }
	protected void CatchCheck()
	{
		if (GameManager.Instance.gmInputs[myPlayer].mCatch && cacheplayer.ballCanCatch != null)
		{
            if (cacheplayer.ballCanCatch.GetCanBeCatch())
            {
                //Vector3 startPos = 
                if (!Physics2D.Raycast(cacheplayer.catchCenter.position, cacheplayer.ballCanCatch.transform.position - cacheplayer.transform.position,
                    Vector3.Distance(cacheplayer.transform.position, cacheplayer.ballCanCatch.transform.position), cacheplayer.layerMask))
                {
                    if (!cacheplayer.haveBall && cacheplayer.ballInRange)
                    {
                        if (!cacheplayer.ballCanCatch.IsBall)
                        {
                            cacheplayer.ballCanCatch.GetComponent<TrophyInfo>().DisableCollider();
                        }
                        cacheplayer.canCharge = false;
                        cacheplayer.haveBall = true;
                        cacheplayer.ballHolding = cacheplayer.ballCanCatch.gameObject;
                        cacheplayer.ballHolding.GetComponent<BallInfo>().BeingCatch(cacheplayer.gameObject);
                        cacheplayer.stat_ballGrab++;
                    }
                }
            }
		}
	}

	public override void Mutate()
	{
		
		if (cacheplayer.haveBall && cacheplayer.ballHolding != null)
		{
            cacheplayer.ReleaseBall();
		}
		cacheplayer.characterType = new Gorilla(myPlayer);
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
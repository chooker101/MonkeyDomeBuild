using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gorilla : Character
{
	private float timeBeingGorilla = 0f;
    private float chargeCount = 0f;
    private float chargeCompleteTime = 1f;
    private bool canDash = false;
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
            if (cacheplayer.GetComponent<Transform>().localScale != gorillaSize)
            {
                cacheplayer.GetComponent<Transform>().localScale = gorillaSize;
            }
        }
        else
        {
            if (cacheplayer.GetComponent<Transform>().localScale != monkeySize)
            {
                cacheplayer.GetComponent<Transform>().localScale = monkeySize;
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
            if (!Physics2D.Raycast(cacheplayer.transform.position, cacheplayer.ballCanCatch.transform.position - cacheplayer.transform.position,
                Vector3.Distance(cacheplayer.transform.position, cacheplayer.ballCanCatch.transform.position), cacheplayer.layerMask))
            {
                if (cacheplayer.ballInRange)
                {
                    if (cacheplayer.ballCanCatch.GetComponent<BallInfo>().IsBall)
                    {
                        // Checks to see if the current scene isn't the pre-game room
                        if (cacheplayer.ballCanCatch.GetComponent<BallInfo>().GetHoldingMonkey() != null && SceneManager.GetActiveScene().name != "PregameRoom")
                        {
                            GameManager.Instance.gmScoringManager.GorillaInterceptScore(GameManager.Instance.gmPlayers[myPlayer], cacheplayer.ballCanCatch.GetHoldingMonkey(),cacheplayer.ballCanCatch.gameObject);
                            //On interception check for active audience interception event
                            if (GameManager.Instance.gmAudienceManager.GetEventActive())
                            { 
                                if(GameManager.Instance.gmAudienceManager.GetCurrentEvent() == AudienceManager.AudienceEvent.Intercept)
                                {
                                    GameManager.Instance.gmAudienceManager.AudGorillaIntercepted(GameManager.Instance.gmPlayers[myPlayer]);
                                }
                            }
                        }
                        cacheplayer.ballCanCatch.GetComponent<BallInfo>().Change(myPlayer);
                        cacheplayer.stat_ballGrab++;
                    }
                }
            }

        }
    }
	public override void Mutate()
	{
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

            }
            cacheplayer.GorillaDash();
            canDash = false;
        }
        else if (GameManager.Instance.gmInputs[myPlayer].mChargeStomp && !canDash)
        {
            isCharging = true;
            if (chargeCount >= chargeCompleteTime)
            {
                canDash = true;
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

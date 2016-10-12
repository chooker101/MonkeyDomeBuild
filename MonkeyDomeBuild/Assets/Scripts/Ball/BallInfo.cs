using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public enum ThrowableType
{
    Ball = 0,
    Trophy = 1
}

public class BallInfo : MonoBehaviour
{
    protected ThrowableType type = ThrowableType.Ball;
    [SerializeField]
    protected GameObject lastThrowMonkey = null;
    protected GameObject holdingMonkey = null;
	public int playerThrewLast = -1;
    protected PhysicsMaterial2D ballMat;
    protected Rigidbody2D m_rigid;
    protected Vector2 startPos;
    public bool isballnear = false;
    public bool timerUp = false;
    //private float timer = 8f;
    //public float count = 0f;
    protected bool perfectCatch = false;
    public float perfectCatchDistance;
    //private float bounciness;

    public float distanceTravel = 0f;
    public float travelTime = 0f;
    public float minCalcDistanceVelocity = 10f;
    public float magnitudeOfVelocity = 0f;
    public int numberOfBounce = 0;
    protected bool canBeCatch = true;

    //public GameObject testMonkey;
    public Material mySpriteColour;

    public float GetDistanceTravel()
    {
        return distanceTravel;
    }
    public bool GetCanBeCatch()
    {
        return canBeCatch;
    }
    public GameObject GetHoldingMonkey()
    {
		return holdingMonkey;
    }
	public GameObject GetLastThrowMonkey()
	{
		return lastThrowMonkey;
	}
    public bool IsBall
    {
        get
        {
            if (type == ThrowableType.Ball)
            {
                return true;
            }
            return false;
        }
    }
    /*public float GetCurrentShotClockTime()
	{
		return count;
	}*/
    void Start ()
    {
        //Debug.Log("BallInfo Start being called");
        startPos = transform.position;
        GameManager.Instance.AddBall(gameObject);
        perfectCatchDistance = 1f;
        m_rigid = GetComponent<Rigidbody2D>();
        ballMat = GetComponent<CircleCollider2D>().sharedMaterial;
        type = ThrowableType.Ball;
        //bounciness = ballMat.bounciness;
        //timer = 8f;
        //PickRandomVictim();
    }
    void Update()
    {
        //Debug.DrawLine(transform.position, transform.position + Vector3.right * perfectCatchDistance);
        //Debug.Log("run" + gameObject.name);
        if (IsBall)
        {
            if (GameManager.Instance.gmShotClockManager.IsShotClockActive)
            {
                if (GameManager.Instance.gmShotClockManager.ShotClockCount >= GameManager.Instance.gmShotClockManager.ShotClockTime)
                {
                    GameManager.Instance.gmShotClockManager.ResetShotClock();
                    Change();
                }
                else
                {
                    if (SceneManager.GetActiveScene().name != "PregameRoom")
                    {
                        GameManager.Instance.gmShotClockManager.ShotClockCount += Time.deltaTime;
                        //count += Time.fixedDeltaTime;
                    }
                }
            }
            UpdateTravelDistance();
        }
        magnitudeOfVelocity = m_rigid.velocity.magnitude;
    }

    void LateUpdate()
    {
        if (holdingMonkey != null)
        {
            m_rigid.transform.position = Vector2.Lerp(m_rigid.transform.position, holdingMonkey.transform.position, 1f);
        }
    }

	public void UpdateLastThrowMonkey(GameObject monkey)
    {
        lastThrowMonkey = monkey;
        //timerUp = true;
        if (IsBall)
        {
            GameManager.Instance.gmShotClockManager.IsShotClockActive = true;
        }
    }

    public void ResetPosition()
    {
        m_rigid.velocity = Vector3.zero;
        m_rigid.position = startPos;
    }

    public void Reset()
    {
        canBeCatch = true;
        holdingMonkey = null;
        playerThrewLast = -1;
        //timerUp = false;
        GameManager.Instance.gmShotClockManager.IsShotClockActive = false;
        m_rigid.isKinematic = false;
        //m_rigid.useGravity = true;
    }
    /*protected void ResetShotCount()
    {
        count = 0f;
    }*/

    public void Change(int index)
    {
        if (lastThrowMonkey == null || GameManager.Instance.gmPlayers[index].GetInstanceID() == lastThrowMonkey.GetInstanceID())
            PickRandomVictim();
        float longestTimeGorilla = 0f;
        //count = 0;
        GameManager.Instance.gmShotClockManager.ResetShotClock();
        GameManager.Instance.gmShotClockManager.IsShotClockActive = false;
        GameObject gorillaToSwitch = null;
        for(int i = 0; i < GameManager.Instance.TotalNumberofPlayers; ++i)
        {
            if (GameManager.Instance.gmPlayerScripts[i].characterType is Gorilla)
            {
                Gorilla gor = (Gorilla)GameManager.Instance.gmPlayerScripts[i].characterType;
                if (longestTimeGorilla < gor.GetTimeBeingGorilla())
                {
                    longestTimeGorilla = gor.GetTimeBeingGorilla();
                    gorillaToSwitch = GameManager.Instance.gmPlayers[i];
                }
            }
        }
        if (gorillaToSwitch != null)
        {
            gorillaToSwitch.GetComponent<Actor>().characterType.Mutate();
        }

        lastThrowMonkey.GetComponent<Actor>().characterType.Mutate();
        //ResetPosition();

        //timerUp = false;
    }

    public void Change()
    {
        float longestTimeGorilla = 0f;
        GameManager.Instance.gmShotClockManager.ResetShotClock();
        GameObject gorillaToSwitch = null;
        for (int i = 0; i < GameManager.Instance.TotalNumberofPlayers; ++i)
        {
            if (GameManager.Instance.gmPlayerScripts[i].characterType is Gorilla)
            {
                Gorilla gor = (Gorilla)GameManager.Instance.gmPlayerScripts[i].characterType;
                if (longestTimeGorilla < gor.GetTimeBeingGorilla())
                {
                    longestTimeGorilla = gor.GetTimeBeingGorilla();
                    gorillaToSwitch = GameManager.Instance.gmPlayers[i];
                }
            }
        }
        if (gorillaToSwitch != null)
        {
            gorillaToSwitch.GetComponent<Actor>().characterType.Mutate();
        }
        lastThrowMonkey.GetComponent<Actor>().characterType.Mutate();
        ResetPosition();
        //timerUp = false;
        lastThrowMonkey.GetComponent<Actor>().ResetTimeScale();
        GameManager.Instance.gmShotClockManager.IsShotClockActive = false;
    }

    public void BeingCatch(GameObject who)
    {
        if (holdingMonkey == null)
        {
            if (who != lastThrowMonkey)
            {
                if (IsBall)
                {
                    GameManager.Instance.gmShotClockManager.ResetShotClock();
                }
                //ResetShotCount();
            }

            canBeCatch = false;
            holdingMonkey = who;
            m_rigid.isKinematic = true;
            if (Vector3.Distance(who.transform.position, transform.position) <= perfectCatchDistance)
                perfectCatch = true;
            if(SceneManager.GetActiveScene().name != "PregameRoom" && IsBall)
            {
                GameManager.Instance.gmScoringManager.PassingScore(lastThrowMonkey, who, distanceTravel, travelTime, perfectCatch, numberOfBounce);
            }
            ResetScoringStats();
            UpdateLastThrowMonkey(who);
            perfectCatch = false;
            numberOfBounce = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Player")
        {
             isballnear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isballnear = false;
        }
    }

    public void PickRandomVictim()
    {
        int index = 0;
        Character victim = null;
        while (victim == null || victim is Gorilla)
        {
            index = Random.Range(0, GameManager.Instance.gmPlayers.Count);
            if(GameManager.Instance.gmPlayerScripts[index]!=null)
                victim = GameManager.Instance.gmPlayerScripts[index].characterType;
        }
        lastThrowMonkey = GameManager.Instance.gmPlayers[index];
    }

    
    void UpdateTravelDistance()
    {
        if (holdingMonkey == null && lastThrowMonkey != null)
        {
            if(m_rigid.velocity.magnitude > minCalcDistanceVelocity)
            {
                distanceTravel += m_rigid.velocity.magnitude * Time.deltaTime;
            }
            travelTime += Time.deltaTime;
        }
    }

	void ResetScoringStats()
	{
		distanceTravel = 0f;
		travelTime = 0f;
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (lastThrowMonkey != null)
        {
            if (other.gameObject.layer==LayerMask.NameToLayer("Floor"))
            {
                numberOfBounce++;
            }
        }
        /*
        if (!IsBall)
        {

        }
        */
    }

    void BounceCount()
    {

    }

}

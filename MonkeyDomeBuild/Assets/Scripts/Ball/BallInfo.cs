using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public enum ThrowableType
{
    Ball = 0,
    Trophy = 1,
    Coconut = 2
}

public class BallInfo : MonoBehaviour
{
    protected ThrowableType type = ThrowableType.Ball;
    [SerializeField]
    public GameObject lastThrowMonkey = null;
    public GameObject holdingMonkey = null;
	public int playerThrewLast = -1;
    public ParticleSystem trailParticle;
    protected PhysicsMaterial2D ballMat;
    protected Rigidbody2D m_rigid;
    protected Vector2 startPos;
    protected Vector3 holdPos;
    public bool isballnear = false;
    public bool timerUp = false;
    //private float timer = 8f;
    //public float count = 0f;
    protected bool perfectCatch = false;
    public float perfectCatchDistance;
    //private float bounciness;

    public float distanceTravel = 0f;
    public float travelTime = 0f;
    public float magnitudeOfVelocity = 0f;
    public int numberOfBounce = 0;
    protected bool canBeCatch = true;
    protected bool canPlaySE = true;
    protected float lastPlaySETime = 0;

    //public GameObject testMonkey;
    public Material mySpriteColour;

    public float GetDistanceTravel()
    {
        return distanceTravel;
    }
    public virtual bool GetCanBeCatch()
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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddBall(gameObject);
        }
        perfectCatchDistance = 1f;
        m_rigid = GetComponent<Rigidbody2D>();
        ballMat = GetComponent<CircleCollider2D>().sharedMaterial;
        type = ThrowableType.Ball;
        trailParticle = GetComponentInChildren<ParticleSystem>();
        trailParticle.startColor = GetComponent<SpriteRenderer>().material.color;
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
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.gmShotClockManager.IsShotClockActive)
                {
                    if (GameManager.Instance.gmShotClockManager.ShotClockCount >= GameManager.Instance.gmShotClockManager.ShotClockTime)
                    {
                        GameManager.Instance.gmShotClockManager.ResetShotClock();

                        if (GameManager.Instance.gmAudienceAnimator != null)
                        GameManager.Instance.gmAudienceAnimator.AudienceAngry();
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
            }
            UpdateTravelDistance();
        }
        magnitudeOfVelocity = m_rigid.velocity.magnitude;
    }

    void LateUpdate()
    {
        if (holdingMonkey != null)
        {
            Vector3 newLoc = holdingMonkey.GetComponent<Actor>().catchCenter.position;
            newLoc.z = transform.position.z;
            m_rigid.transform.position = newLoc;
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
        if (holdingMonkey != null)
        {
            lastThrowMonkey = holdingMonkey;
        }
        canBeCatch = true;
        holdingMonkey = null;
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
        if (SceneManager.GetActiveScene().name != "PregameRoom" && IsBall)
        {
            if (lastThrowMonkey != null)
            {
                if (index != lastThrowMonkey.GetComponent<Actor>().playerIndex)
                {
                    Debug.Log(index);
                    Debug.Log(lastThrowMonkey.GetComponent<Actor>().playerIndex);
                    GameManager.Instance.gmScoringManager.PassingScore(lastThrowMonkey, GameManager.Instance.gmPlayers[index], distanceTravel, travelTime, perfectCatch, numberOfBounce);

                }
            }
        }
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
        Reset();
        lastThrowMonkey.GetComponent<Actor>().characterType.Mutate();
        GameManager.Instance.gmScoringManager.ResetCombo();
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
            AudioEffectManager.Instance.PlayAudienceInterception();
            gorillaToSwitch.GetComponent<Actor>().characterType.Mutate();
        }
        lastThrowMonkey.GetComponent<Actor>().characterType.Mutate();
        ResetPosition();
        Reset();
        GameManager.Instance.gmShotClockManager.IsShotClockActive = false;
        GameManager.Instance.gmScoringManager.ResetCombo();
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
                    if (SceneManager.GetActiveScene().name != "PregameRoom")
                    {
                        AudioEffectManager.Instance.PlayAudienceCatch();
                    }
                }
                //ResetShotCount();
            }

            canBeCatch = false;
            holdingMonkey = who;
            m_rigid.isKinematic = true;
            if (Vector3.Distance(who.GetComponent<Actor>().catchCenter.position, transform.position) <= perfectCatchDistance)
            {
                perfectCatch = true;
                GameManager.Instance.gmTrophyManager.PerformPerfectCatch(who.GetComponent<Actor>().playerIndex);
            }
            AudioEffectManager.Instance.PlayMonkeyCatchSE();
            UpdateLastThrowMonkey(who);
            ResetScoringStats();
            perfectCatch = false;
            numberOfBounce = 0;
        }
    }
    public bool IsPerfectCatch(Actor player)
    {
        if (Vector3.Distance(player.catchCenter.position, transform.position) <= perfectCatchDistance)
        {
            perfectCatch = true;
            return true;
        }
        return false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {     
        if(other.gameObject.tag == "Player")
        {
             isballnear = true;
        }
    }
    void OntriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
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
        distanceTravel += m_rigid.velocity.magnitude * Time.deltaTime;
        travelTime += Time.deltaTime;
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
                if (Time.time - lastPlaySETime > 0.3f)
                {
                    lastPlaySETime = Time.time;
                    if (m_rigid.velocity.magnitude > 3f)
                    {
                        if (travelTime > 1f)
                        {
                            AudioEffectManager.Instance.PlayBallBounceHardSE();
                        }
                        else
                        {
                            AudioEffectManager.Instance.PlayBallBounceSoftSE();
                        }
                    }
                }
                numberOfBounce++;
            }
        }
        /*
        if (!IsBall)
        {

        }
        */
    }
    public ThrowableType BallType
    {
        get
        {
            return type;
        }
    }

    void BounceCount()
    {

    }
    IEnumerator AudioDelay()
    {
        yield return new WaitForSeconds(0.2f);
        canPlaySE = true;
    }

}

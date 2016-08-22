using UnityEngine;
using System.Collections;

public class BallInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject lastThrowMonkey = null;
    private GameObject holdingMonkey = null;
    public int playerThrewLast = -1;
    private PhysicsMaterial2D ballMat;
    private Rigidbody2D m_rigid;
    private Vector2 startPos = Vector2.up * 10;
    public bool isballnear = false;
    public bool timerUp = false;
    private float timer = 8f;
    public float count = 0f;
    private bool perfectCatch = false;
    public float perfectCatchDistance;
    private float bounciness;

    public float distanceTravel = 0f;
    public float travelTime = 0f;
    public float minCalcDistanceVelocity = 10f;
    public float vel = 0f;
    public int numberOfBounce = 0;
    private bool canBeCatch = true;

    public GameObject testMonkey;
    public Material mySpriteColour;

    public float DistanceTravel
    {
        get { return distanceTravel; }
    }
    public bool CanBeCatch
    {
        get { return canBeCatch; }
    }
    public GameObject HoldingMonkey
    {
        get { return holdingMonkey; }
    }
    
    void Start ()
    {
        perfectCatchDistance = 1f;
        m_rigid = GetComponent<Rigidbody2D>();
        ballMat = GetComponent<CircleCollider2D>().sharedMaterial;
        bounciness = ballMat.bounciness;
        timer = 8f;
        //PickRandomVictim();
    }
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.right * perfectCatchDistance);
        if (testMonkey != null)
        Debug.Log(Vector3.Distance(transform.position, testMonkey.transform.position));
        if (timerUp)
        {
            if (count >= timer)
            {
                Change();
            }
            else
            {
                count += Time.deltaTime;
            }
        }
        UpdateTravelDistance();
        vel = m_rigid.velocity.magnitude;
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
        timerUp = true;
    }
    public GameObject GetLastThrowMonkey()
    {
        return lastThrowMonkey;
    }
    public void ResetPosition()
    {
        m_rigid.position = startPos;
    }

    public void Reset()
    {
        canBeCatch = true;
        holdingMonkey = null;
        playerThrewLast = -1;
        count = 0f;
        timerUp = false;
		m_rigid.isKinematic = false;
        //m_rigid.useGravity = true;
    }
    public void Change(int index)
    {
        if (lastThrowMonkey == null || GameManager.Instance.gmPlayers[index].GetInstanceID() == lastThrowMonkey.GetInstanceID()) PickRandomVictim();
        float longestTimeGorilla = 0f;
        count = 0;
        GameObject gorillaToSwitch = null;
        for(int i = 0; i < GameManager.Instance.TNOP; ++i)
        {
            if (GameManager.Instance.gmPlayers[i].GetComponent<Player>().characterType is Gorilla)
            {
                Gorilla gor = (Gorilla)GameManager.Instance.gmPlayers[i].GetComponent<Player>().characterType;
                if (longestTimeGorilla < gor.GetTimeBeingGorilla())
                {
                    longestTimeGorilla = gor.GetTimeBeingGorilla();
                    gorillaToSwitch = GameManager.Instance.gmPlayers[i];
                }
            }
        }
        if (gorillaToSwitch != null)
        {
            gorillaToSwitch.GetComponent<Player>().characterType.Mutate();
        }
        lastThrowMonkey.GetComponent<Player>().characterType.Mutate();
        ResetPosition();
        timerUp = false;
    }
    public void Change()
    {
        float longestTimeGorilla = 0f;
        count = 0;
        GameObject gorillaToSwitch = null;
        for (int i = 0; i < GameManager.Instance.TNOP; ++i)
        {
            if (GameManager.Instance.gmPlayers[i].GetComponent<Player>().characterType is Gorilla)
            {
                Gorilla gor = (Gorilla)GameManager.Instance.gmPlayers[i].GetComponent<Player>().characterType;
                if (longestTimeGorilla < gor.GetTimeBeingGorilla())
                {
                    longestTimeGorilla = gor.GetTimeBeingGorilla();
                    gorillaToSwitch = GameManager.Instance.gmPlayers[i];
                }
            }
        }
        if (gorillaToSwitch != null)
        {
            gorillaToSwitch.GetComponent<Player>().characterType.Mutate();
        }
        lastThrowMonkey.GetComponent<Player>().characterType.Mutate();
        ResetPosition();
        timerUp = false;
    }

    public void BeingCatch(GameObject who)
    {
        if (holdingMonkey == null)
        {
            canBeCatch = false;
            holdingMonkey = who;
            m_rigid.isKinematic = true;
            if (Vector3.Distance(who.transform.position, transform.position) <= perfectCatchDistance)
                perfectCatch = true;
            ScoringManager.Instance.PassingScore(lastThrowMonkey, who, distanceTravel, travelTime, perfectCatch, numberOfBounce);
            ResetScoringStats();
            UpdateLastThrowMonkey(who);
            perfectCatch = false;
            numberOfBounce = 0;
        }
    }
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>() != null)
        {
            isballnear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
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
            victim = GameManager.Instance.gmPlayers[index].GetComponent<Player>().characterType;
        }
        lastThrowMonkey = GameManager.Instance.gmPlayers[index];
    }
    public float GetCurrentShotClockTime()
    {
        return count;
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
            if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Ceiling") || other.gameObject.CompareTag("Wall"))
            {
                numberOfBounce++;
            }
        }
    }
    void BounceCount()
    {

    }
}

using UnityEngine;
using System.Collections;

public class BallInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject lastThrowMonkey = null;
    private GameObject holdingMonkey = null;
    public bool isballnear = false;
    private Rigidbody m_rigid;
    private Vector3 startPos = Vector3.up * 10;
    public bool timerUp = false;
    private float timer = 8f;
    public float count = 0f;
    private PhysicMaterial ballMat;
    [SerializeField]
    private float bounciness;
    
    void Start ()
    {
        m_rigid = GetComponent<Rigidbody>();
        ballMat = GetComponent<SphereCollider>().material;
        bounciness = ballMat.bounciness;
        timer = 8f;
        //PickRandomVictim();
    }
    void Update()
    {
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
    }
    void LateUpdate()
    {
        if (holdingMonkey != null)
        {
            m_rigid.transform.position = Vector3.Lerp(m_rigid.transform.position, holdingMonkey.transform.position, 1f);
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
        holdingMonkey = null;
        count = 0f;
        timerUp = false;
        m_rigid.useGravity = true;
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
            holdingMonkey = who;
            m_rigid.useGravity = false;
        }
    }
	
    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            isballnear = true;
        }
    }

    void OnTriggerExit(Collider other)
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
	
}

using UnityEngine;
using System.Collections;

public class BallInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject lastThrowMonkey = null;
    private Rigidbody m_rigid;
    private Vector3 startPos = Vector3.up * 10;
    public bool timerUp = false;
    private float timer = 1f;
    public float count = 0f;
    private PhysicMaterial ballMat;
    [SerializeField]
    private float bounciness;
    
    void Start ()
    {
        m_rigid = GetComponent<Rigidbody>();
        ballMat = GetComponent<SphereCollider>().material;
        bounciness = ballMat.bounciness;
        //PickRandomVictim();
    }
    void Update()
    {
        if (lastThrowMonkey == null)
        {
            //PickRandomVictim();
        }
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
        count = 0f;
        timerUp = false;
        m_rigid.useGravity = true;
        m_rigid.isKinematic = false;
        transform.parent = null;
    }

    public void Change()
    {
        float longestTimeGorilla = 0f;
        count = 0;
        GameObject gorillaToSwitch = null;
        for(int i = 0; i < GameManager.Instance.TNOP; ++i)
        {
            if (GameManager.Instance.gmPlayers[i].GetComponent<Gorilla>() != null)
            {
                Gorilla gor = (Gorilla)GameManager.Instance.gmPlayers[i].GetComponent<Gorilla>();
                if (longestTimeGorilla < gor.GetTimeBeingGorilla())
                {
                    longestTimeGorilla = gor.GetTimeBeingGorilla();
                    gorillaToSwitch = GameManager.Instance.gmPlayers[i];
                }
            }
        }
        if (gorillaToSwitch != null)
        {
            gorillaToSwitch.GetComponent<Gorilla>().Mutate();
        }
        lastThrowMonkey.GetComponent<Monkey>().Mutate();
        //PickRandomVictim();
        ResetPosition();
        timerUp = false;
    }
	
	/*
    public void PickRandomVictim()
    {
        int index = 0;
        Character victim = null;
        while (victim == null || victim is Gorilla)
        {
            index = Random.Range(0, GameManager.Instance.gmPlayers.Capacity);
            victim = GameManager.Instance.gmPlayers[index].GetComponent<Player>().characterType;
        }
        lastThrowMonkey = GameManager.Instance.gmPlayers[index];
    }
	*/
	
}

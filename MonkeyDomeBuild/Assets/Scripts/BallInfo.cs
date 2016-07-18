using UnityEngine;
using System.Collections;

public class BallInfo : MonoBehaviour {
    [SerializeField]
    private GameObject lastThrowMonkey;
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
        PickRandomVictim();
    }
    void Update()
    {
        if (lastThrowMonkey == null)
        {
            PickRandomVictim();
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
        GameObject[] allPlayer = GameObject.FindGameObjectsWithTag("Player");
        GameObject gorillaToSwitch = null;
        foreach (GameObject g in allPlayer)
        {
            Player p = g.GetComponent<Player>();
            if (p is GorillaAction)
            {
                GorillaAction gor = (GorillaAction)p;
                if (longestTimeGorilla < gor.GetTimeBeingGorilla())
                {
                    longestTimeGorilla = gor.GetTimeBeingGorilla();
                    gorillaToSwitch = g;
                }
            }
        }
        if (gorillaToSwitch != null)
        {
            gorillaToSwitch.GetComponent<GorillaAction>().Mutate();
        }
        lastThrowMonkey.GetComponent<MonkeyAction>().Mutate();
        PickRandomVictim();
        ResetPosition();
        timerUp = false;
    }
    public void PickRandomVictim()
    {
        GameObject[] allPlayer = GameObject.FindGameObjectsWithTag("Player");
        int index = 0;
        Player victim = null;
        while (victim == null || victim is GorillaAction)
        {
            index = Random.Range(0, allPlayer.Length);
            victim = allPlayer[index].GetComponent<Player>();
        }
        lastThrowMonkey = allPlayer[index];
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GorillaAction : Player
{
    private float timeBeingGorilla = 0f;
    void OnEnable()
    {
        moveForce = 100f;
        jumpForce = 65f;
        speedLimit = 12f;
        throwForce = 40f;
        downForce = 80f;
        tempDownForce = downForce;
        downForceIncrement = 100f; // per second
        maxDownForce = 200f;
        climbForce = 200f; ;
        climbSpeedLimit = speedLimit;
        normalDrag = 8f;
        climbDrag = 12f;
    }
    void Update()
    {
        timeBeingGorilla += Time.deltaTime;
        CheckInputs();
        JumpCheck();
        Aim();
        CatchCheck();
        StompCheck();
        mov = m_rigid.velocity;
    }
    void FixedUpdate()
    {
        Movement();
    }
    protected override void CatchCheck()
    {
        if (mCatch && ball != null)
        {
            if (ballInRange)
            {
                ballHolding = ball.transform.parent.gameObject;
                ballHolding.GetComponent<Rigidbody>().position += Vector3.up * 2;
                ballHolding.GetComponent<BallInfo>().Change();
                stat_ballGrab++;
            }
        }
    }
    public override void Mutate()
    {
        GameObject monkey = GameObject.FindGameObjectWithTag("PrefabsRef").GetComponent<PrefabsRef>().monkey;
        if (monkey != null)
        {
            GameObject tempMonkey = (GameObject)Instantiate(monkey, m_rigid.position, m_rigid.rotation);
            tempMonkey.GetComponent<Player>().whichPlayer = whichPlayer;
            tempMonkey.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
            List<GameObject> allPlayer = GameManager.Instance.gmPlayers;
            for(int i = 0; i < allPlayer.Capacity; i++)
            {
                if (allPlayer[i].GetInstanceID() == gameObject.GetInstanceID())
                {
                    allPlayer[i] = tempMonkey;
                    break;
                }
            }
            Destroy(gameObject);
        }
    }
    public float GetTimeBeingGorilla()
    {
        return timeBeingGorilla;
    }
    protected void StompCheck()
    {
        if (mAimStomp)
        {
            GameObject[] allPlayer = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject g in allPlayer)
            {
                Player p = g.GetComponent<Player>();
                if (p is MonkeyAction)
                {
                    //knock both player off vine for now
                    p.isClimbing = false;
                }
            }
        }
    }
}

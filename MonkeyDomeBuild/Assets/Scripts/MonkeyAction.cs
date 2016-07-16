using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonkeyAction : Player
{
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
        CheckInputs();
        JumpCheck();
        Aim();
        if (haveBall)
        {
            ThrowCheck();
        }
        else
        {
            CatchCheck();
        }
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
            if (!haveBall && ballInRange)
            {
                ballHolding = ball.transform.parent.gameObject;
                ballHolding.GetComponent<BallInfo>().UpdateLastThrowMonkey(gameObject);                
                ballHolding.GetComponent<Rigidbody>().useGravity = false;
                ballHolding.GetComponent<Rigidbody>().isKinematic = true;
                ballHolding.transform.SetParent(transform);
                haveBall = true;
                stat_ballGrab++;
            }
        }
    }
    public override void Mutate()
    {
        GameObject gorilla = GameObject.FindGameObjectWithTag("PrefabsRef").GetComponent<PrefabsRef>().gorilla;
        if (gorilla != null)
        {
            if (haveBall && ballHolding != null)
            {
                haveBall = false;
                ballHolding.GetComponent<BallInfo>().Reset();
                ball = null;
                ballHolding = null;
            }
            GameObject tempGorilla = (GameObject)Instantiate(gorilla, m_rigid.position, m_rigid.rotation);
            tempGorilla.GetComponent<Player>().whichPlayer = whichPlayer;
            tempGorilla.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
            List<GameObject> allPlayer = GameManager.Instance.gmPlayers;
            for (int i = 0; i < allPlayer.Capacity; i++)
            {
                if (allPlayer[i].GetInstanceID() == gameObject.GetInstanceID())
                {
                    allPlayer[i] = tempGorilla;
                    break;
                }
            }
            Destroy(gameObject);
        }
    }
}

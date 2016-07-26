﻿using UnityEngine;
using System.Collections;

public class BallInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject lastThrowMonkey = null;
    public GameObject monkeyHolding;
    private Rigidbody m_rigid;
    private Vector3 startPos = Vector3.up * 10;
    public bool timerUp = false;
    private float timer = 5f;
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
    void LateUpdate()
    {
        if (monkeyHolding != null)
        {
            if (m_rigid.useGravity) m_rigid.useGravity = false;
            m_rigid.position = Vector3.Lerp(m_rigid.position, monkeyHolding.GetComponent<Rigidbody>().transform.position, 1f);
        }
        else
        {
            if (!m_rigid.useGravity) m_rigid.useGravity = true;
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
        monkeyHolding = null;
    }

    public void Change()
    {
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
        //PickRandomVictim();
        ResetPosition();
        timerUp = false;
    }
	public void BeingCatch(GameObject who)
    {
        if (monkeyHolding == null)
        {
            monkeyHolding = who;
            m_rigid.useGravity = false;
        }
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

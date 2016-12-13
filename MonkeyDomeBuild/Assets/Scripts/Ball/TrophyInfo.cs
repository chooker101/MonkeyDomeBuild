using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrophyInfo : BallInfo
{
    bool beingThrown = false;
    List<Actor> hitPlayers = new List<Actor>();

    void Start()
    {
        //Debug.Log("TrophyInfo Start being called");
        perfectCatchDistance = 1f;
        m_rigid = GetComponent<Rigidbody2D>();
        ballMat = GetComponent<Collider2D>().sharedMaterial;
        type = ThrowableType.Trophy;
    }
    public void ThrowTrophy()
    {
        GameManager.Instance.gmTrophyManager.ThrewCoconut(holdingMonkey.GetComponent<Actor>().playerIndex);
        if (!beingThrown)
        {
            beingThrown = true;
            StartCoroutine(BeingThrown());
        }

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.collider.GetComponent<Actor>().playerIndex != lastThrowMonkey.GetComponent<Actor>().playerIndex)
            {
                bool canDmg = true;
                if (hitPlayers.Count > 0)
                {
                    for (int i = 0; i < hitPlayers.Count; i++)
                    {
                        if (other.collider.GetComponent<Actor>().playerIndex == hitPlayers[i].playerIndex)
                        {
                            canDmg = false;
                        }
                    }
                    if (canDmg)
                    {
                        hitPlayers.Add(other.collider.GetComponent<Actor>());
                    }
                }
                else
                {
                    hitPlayers.Add(other.collider.GetComponent<Actor>());
                }
                other.collider.GetComponent<Actor>().TempDisableInput(0.5f);
            }
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        OnCollisionEnter2D(other);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !beingThrown)
        {
            isballnear = true;
        }
    }
    void Update()
    {


    }
    public bool IsThrown
    {
        get
        {
            return beingThrown;
        }
    }
    IEnumerator ChangeLayer()
    {
        yield return new WaitForSeconds(1f);
        gameObject.layer = LayerMask.NameToLayer("Ball");
    }
    IEnumerator BeingThrown()
    {
        yield return new WaitForSeconds(0.05f);
        gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(1.5f);
        gameObject.layer = LayerMask.NameToLayer("Ball");
        canBeCatch = true;
        beingThrown = false;
        
    }
    public void BeingCatch()
    {
        canBeCatch = false;
    }


}

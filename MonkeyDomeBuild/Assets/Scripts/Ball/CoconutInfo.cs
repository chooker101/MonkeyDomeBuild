using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoconutInfo : BallInfo
{
    bool inStage = false;
    bool beingThrown = false;
    bool canChangeLayer = true;
    bool canDoDmg = true;
    List<Actor> hitPlayers = new List<Actor>();
    void Start()
    {
        //Debug.Log("TrophyInfo Start being called");
        perfectCatchDistance = 1f;
        m_rigid = GetComponent<Rigidbody2D>();
        ballMat = GetComponent<Collider2D>().sharedMaterial;
        type = ThrowableType.Coconut;
    }
    void OnDisable()
    {
        inStage = false;
        beingThrown = false;
        canChangeLayer = true;
        hitPlayers.Clear();
    }
    public void ThrowCoconut()
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
        if (beingThrown)
        {
            if(other.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //Debug.Log("hit");
                if (other.collider.GetComponent<Actor>().playerIndex != lastThrowMonkey.GetComponent<Actor>().playerIndex)
                {
                    //Debug.Log("stun");
                    //Debug.Log("hit");
                    switch (GameManager.Instance.nextGameModeUI)
                    {
                        case GameManager.GameMode.Battle_Royal:
                            bool canDmg = true;
                            if (hitPlayers.Count > 0)
                            {
                                for(int i = 0; i < hitPlayers.Count; i++)
                                {
                                    if (other.collider.GetComponent<Actor>().playerIndex == hitPlayers[i].playerIndex)
                                    {
                                        canDmg = false;
                                    }
                                }
                                if (canDmg)
                                {
                                    hitPlayers.Add(other.collider.GetComponent<Actor>());
                                    other.collider.GetComponent<Actor>().Damage(lastThrowMonkey.GetComponent<Actor>().playerIndex);
                                }
                            }
                            else
                            {
                                hitPlayers.Add(other.collider.GetComponent<Actor>());
                                other.collider.GetComponent<Actor>().Damage(lastThrowMonkey.GetComponent<Actor>().playerIndex);
                            }
                            break;
                        case GameManager.GameMode.Keep_Away:
                            other.collider.GetComponent<Actor>().TempDisableInput(1f);
                            break;
                    }
                    if (canChangeLayer)
                    {
                        canChangeLayer = false;
                        StartCoroutine(ChangeLayer());
                    }
                    if (canDoDmg)
                    {
                        canDoDmg = false;

                    }
                }
            }
            if(other.collider.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                if (canChangeLayer)
                {
                    canChangeLayer = false;
                    StartCoroutine(ChangeLayer());
                }
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
        if(other.gameObject.layer == LayerMask.NameToLayer("CoconutLayerChanger"))
        {
            if (!inStage)
            {
                inStage = true;
                gameObject.layer = LayerMask.NameToLayer("Ball");
            }
        }
    }
    void Update()
    {
        if (transform.position.y < -100f)
        {
            gameObject.SetActive(false);
        }

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
        switch (GameManager.Instance.nextGameModeUI)
        {
            case GameManager.GameMode.Battle_Royal:
                yield return new WaitForSeconds(0.8f);
                break;
            case GameManager.GameMode.Keep_Away:
                yield return new WaitForSeconds(0.4f);
                break;
        }
        gameObject.layer = LayerMask.NameToLayer("UsedCoconut");
    }
    IEnumerator BeingThrown()
    {
        yield return new WaitForSeconds(0.05f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}

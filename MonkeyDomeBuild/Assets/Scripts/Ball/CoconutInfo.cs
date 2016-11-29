using UnityEngine;
using System.Collections;

public class CoconutInfo : BallInfo
{
    bool inStage = false;
    bool beingThrown = false;
    bool canChangeLayer = true;
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
    }
    public void ThrowCoconut()
    {
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
                    other.collider.GetComponent<Actor>().TempDisableInput(1f);
                    if (canChangeLayer)
                    {
                        canChangeLayer = false;
                        StartCoroutine(ChangeLayer());
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
        yield return new WaitForSeconds(0.4f);
        gameObject.layer = LayerMask.NameToLayer("UsedCoconut");
    }
    IEnumerator BeingThrown()
    {
        yield return new WaitForSeconds(0.05f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}

using UnityEngine;
using System.Collections;

public class TrophyInfo : BallInfo
{
    private bool colliderIsOff = false;
    void Start()
    {
        //Debug.Log("TrophyInfo Start being called");
        perfectCatchDistance = 1f;
        m_rigid = GetComponent<Rigidbody2D>();
        ballMat = GetComponent<Collider2D>().sharedMaterial;
        type = ThrowableType.Trophy;
    }
    public void DisableCollider()
    {
        colliderIsOff = true;
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }
    public void InvokeEnableCollider()
    {
        colliderIsOff = false;
        Invoke("ReactivateCollider", 0.05f);
    }
    private void ReactivateCollider()
    {
        if (GetComponent<PolygonCollider2D>().isTrigger)
        {
            GetComponent<PolygonCollider2D>().isTrigger = false;
        }
    }
    public bool IsColliderOff
    {
        get
        {
            return colliderIsOff;
        }
        set
        {
            colliderIsOff = value;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            isballnear = true;
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (!colliderIsOff)
            {
                if (GetComponent<Collider2D>().isTrigger)
                {
                    GetComponent<Collider2D>().isTrigger = false;
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

}

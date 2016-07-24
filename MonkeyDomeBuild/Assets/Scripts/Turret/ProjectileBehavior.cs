using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour
{
    private Rigidbody _rigid;
    public bool canCollideWithFloor = false;
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (!canCollideWithFloor)
        {
            if (_rigid.velocity.y < 0f)
            {
                canCollideWithFloor = true;
            }
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (canCollideWithFloor)
        {
            if (other.CompareTag("Floor"))
            {
                Vector3 remainLoc = transform.position;
                remainLoc.y = other.transform.position.y + other.transform.localScale.y / 2;
                _rigid.isKinematic = true;
                _rigid.useGravity = false;
                _rigid.transform.position = remainLoc;
                _rigid.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
            }
            else if (other.CompareTag("Wall"))
            {
                Vector3 remainLoc = transform.position;
                float whichSideMultiplier = other.transform.position.x > transform.position.x ? -1f : 1f;
                remainLoc.x = other.transform.position.x + whichSideMultiplier * other.transform.localScale.x / 2;
                _rigid.isKinematic = true;
                _rigid.useGravity = false;
                _rigid.transform.position = remainLoc;
                _rigid.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
                _rigid.transform.eulerAngles = new Vector3(0f, 0f, whichSideMultiplier * 90f);
            }
        }
    }
}

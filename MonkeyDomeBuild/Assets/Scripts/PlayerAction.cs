using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : Player {

    public float moveForce;
    public float speedLimit;
    private Rigidbody m_rigid;
    public Vector3 mov;
    private bool canJump = true;
    private float jumpForce;
    private int layerMask;
    private bool ballInRange = false;
    private GameObject ball = null;
    private GameObject ballHolding = null;
    private bool haveBall = false;


    void Start ()
    {
        moveForce = 40f;
        jumpForce = 15f;
        speedLimit = 8f;
        m_rigid = GetComponent<Rigidbody>();
        layerMask = 1 << LayerMask.NameToLayer("Floor");
    }
	
	void FixedUpdate ()
    {
        Movement();
        JumpCheck();
        CatchCheck();
        ThrowCheck();
        mov = m_rigid.velocity;
	}
    private void Movement()
    {
        bool mLeft = Input.GetKey(KeyCode.A);
        bool mRight = Input.GetKey(KeyCode.D);
        Vector3 movement = new Vector3();
        if (mLeft && Mathf.Abs(m_rigid.velocity.x) < speedLimit && !RayCastSide(-1))
        {
            movement.x = -moveForce;
        }
        else if (mRight && Mathf.Abs(m_rigid.velocity.x) < speedLimit && !RayCastSide(1))
        {
            movement.x = moveForce;
        }
        m_rigid.AddForce(movement);
    }
    private void JumpCheck()
    {
        bool mJump = Input.GetKeyDown(KeyCode.Space);

        if (RayCast(-1))
        {
            canJump = true;
        }
        else
        {
            m_rigid.AddForce(new Vector3(0f, -30f));
        }
        if (mJump && canJump)
        {
            canJump = false;
            m_rigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }
    private void CatchCheck()
    {
        bool mCatch = Input.GetKeyDown(KeyCode.F);
        if (mCatch && ball != null)
        {
            if (!haveBall && ballInRange)
            {
                ballHolding = ball.transform.parent.gameObject;
                ball.transform.parent.GetComponent<Rigidbody>().useGravity = false;
                ball.transform.parent.transform.parent = this.transform;
                haveBall = true;
            }
        }
        if (haveBall && ballHolding != null)
        {
            ballHolding.transform.position = new Vector3(m_rigid.transform.position.x, m_rigid.transform.position.y+0.5f, 0f);
        }
    }
    private void ThrowCheck()
    {

    }
    bool RayCast(int direction)
    {
        bool hit = Physics.Raycast(m_rigid.position, direction * Vector3.up, 0.5f + 0.07f, layerMask);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool RayCastSide(int leftOrRight)
    {
        bool hit = Physics.Raycast(m_rigid.position, leftOrRight * Vector3.right, 0.5f + 0.05f, layerMask);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ballInRange = true;
            ball = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ballInRange = false;
            haveBall = false;
            ball = null;
            ballHolding = null;
        }
    }
}

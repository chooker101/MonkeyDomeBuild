using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : Player
{

    public int whichPlayer;
    public float moveForce;
    public float speedLimit;
    public float jumpForce;
    public float throwForce;
    public Vector3 mov;

    private Rigidbody m_rigid;
    private bool canJump = true;
    private int layerMask;
    private bool ballInRange = false;
    private GameObject ball = null;
    private GameObject ballHolding = null;
    private bool haveBall = false;

    private float mX;
    private float mY;
    private bool mJump;
    private bool mCatch;


    // Player Stats
    public int stat_jump = 0;
    public int stat_throw = 0;
    public int stat_catch = 0;

    void Start()
    {
        whichPlayer = 1;
        moveForce = 40f;
        jumpForce = 30f;
        speedLimit = 8f;
        throwForce = 30f;
        m_rigid = GetComponent<Rigidbody>();
        layerMask = 1 << LayerMask.NameToLayer("Floor");
    }

    void FixedUpdate()
    {
        CheckInputs();
        Movement();
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
    private void CheckInputs()
    {
        switch (whichPlayer)
        {
            case 1:
                mX = Input.GetAxis("p1_joy_x");
                mY = -Input.GetAxis("p1_joy_y");
                mJump = Input.GetButton("p1_jump");
                mCatch = Input.GetButtonDown("p1_catch/throw");
                //temp keyboard input
                if (Input.GetKey(KeyCode.A))
                {
                    mX = -1;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    mX = 1;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    mY = 1;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    mY = -1;
                }
                break;
            case 2:
                mX = Input.GetAxis("p2_joy_x");
                mY = -Input.GetAxis("p2_joy_y");
                mJump = Input.GetButton("p2_jump");
                mCatch = Input.GetButtonDown("p2_catch/throw");
                break;
            case 3:
                mX = Input.GetAxis("p3_joy_x");
                mY = -Input.GetAxis("p3_joy_y");
                mJump = Input.GetButton("p3_jump");
                mCatch = Input.GetButtonDown("p3_catch/throw");
                break;
        }
    }
    private void Movement()
    {
        Vector3 movement = new Vector3();
        if (mX != 0 && Mathf.Abs(m_rigid.velocity.x) < speedLimit)
        {
            if ((mX > 0 && !RayCastSide(1)) || (mX < 0 && !RayCastSide(-1)))
            {
                movement.x = mX * moveForce;
            }
        }
        m_rigid.AddForce(movement);
    }
    private void JumpCheck()
    {
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
            stat_jump++; // Adds one to jump stat
        }
    }
    private void CatchCheck()
    {
        if (mCatch && ball != null)
        {
            if (!haveBall && ballInRange)
            {
                ballHolding = ball.transform.parent.gameObject;
                ball.transform.parent.GetComponent<Rigidbody>().useGravity = false;
                ball.transform.parent.transform.parent = this.transform;
                haveBall = true;
                stat_catch++;
            }
        }
    }
    private void ThrowCheck()
    {
        if (mCatch && haveBall)
        {
            haveBall = false;
            Rigidbody ballRigid = ball.transform.parent.GetComponent<Rigidbody>();
            ballRigid.useGravity = true;
            ballRigid.AddForce(mX*throwForce, mY* throwForce, 0f, ForceMode.Impulse);
            ball.transform.parent.transform.parent = null;
            ball = null;
            stat_throw++;
        }
        if (haveBall && ballHolding != null)
        {
            ballHolding.transform.position = new Vector3(m_rigid.transform.position.x, m_rigid.transform.position.y, 0f);
        }
    }
    private bool RayCast(int direction)
    {
        bool hit = Physics.Raycast(m_rigid.position, direction * Vector3.up, transform.localScale.y/2 + 0.07f, layerMask);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool RayCastSide(int leftOrRight)
    {
        bool hit = Physics.Raycast(m_rigid.position, leftOrRight * Vector3.right, transform.localScale.x/2 + 0.05f, layerMask);
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
    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }
    private void Aim()
    {
        Debug.DrawLine(m_rigid.position, new Vector3(m_rigid.position.x + mX * 2, m_rigid.position.y + mY * 2));

    }


    public bool IsHoldingBall()
    {
        return haveBall;
    }

}

using UnityEngine;
using System.Collections;

public class Player : Actor
{
    public bool isGorilla = false;

    public int whichPlayer = 1;
    public float moveForce;
    public float speedLimit;
    public float jumpForce;
    public float throwForce;
    public Vector3 mov;

    protected Rigidbody m_rigid;
    protected bool canJump = true;
    protected int layerMask;
    protected bool ballInRange = false;
    protected GameObject ball = null;
    protected GameObject ballHolding = null;
    protected bool haveBall = false;

    public bool isClimbing = false;
    public bool canClimb = false;
    public float climbSpeedLimit;
    public float downForce;
    public float climbDrag;
    public float normalDrag;
    public float climbForce;
    public float tempDownForce;
    public float downForceIncrement;
    public float maxDownForce;

    protected float mX;
    protected float mY;
    protected bool mJump;
    protected bool mCatch;
    protected bool mClimb;

    public int stat_jump = 0;
    public int stat_throw = 0;
    public int stat_ballGrab = 0;
    void Start ()
	{
	
	}
    protected void CheckInputs()
    {
        switch (whichPlayer)
        {
            case 1:
                mX = Input.GetAxis("p1_joy_x");
                mY = -Input.GetAxis("p1_joy_y");
                mJump = Input.GetButton("p1_jump");
                mCatch = Input.GetButtonDown("p1_catch/throw");
                mClimb = Input.GetButtonDown("p1_climb");
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
                mClimb = Input.GetButtonDown("p2_climb");
                break;
            case 3:
                mX = Input.GetAxis("p3_joy_x");
                mY = -Input.GetAxis("p3_joy_y");
                mJump = Input.GetButton("p3_jump");
                mCatch = Input.GetButtonDown("p3_catch/throw");
                mClimb = Input.GetButtonDown("p3_climb");
                break;
        }
        if (mClimb)
        {
            if (canClimb && !isClimbing)
            {
                isClimbing = true;
                canJump = true;
            }
            else if (isClimbing)
            {
                isClimbing = false;
            }

        }
        if (mJump)
        {
            if (isClimbing)
            {
                isClimbing = false;
            }
        }
        if (isClimbing)
        {
            if (m_rigid.drag != climbDrag)
            {
                m_rigid.drag = climbDrag;
            }
        }
        else
        {
            if (m_rigid.drag != normalDrag)
            {
                m_rigid.drag = normalDrag;
                tempDownForce = downForce;
            }
        }
    }
    protected void Movement()
    {
        Vector3 movement = new Vector3();
        if (!isClimbing)
        {
            if (mX != 0 && Mathf.Abs(m_rigid.velocity.x) < speedLimit)
            {
                if ((mX > 0 && !RayCastSide(1)) || (mX < 0 && !RayCastSide(-1)))
                {
                    movement.x = mX * moveForce;
                }
            }
        }
        else
        {
            if (mX != 0 || mY != 0)
            {
                if (Mathf.Abs(m_rigid.velocity.x) < climbSpeedLimit)
                {
                    if ((mX > 0 && !RayCastSide(1)) || (mX < 0 && !RayCastSide(-1)))
                    {
                        movement.x = mX * climbForce;
                    }
                }
                if (Mathf.Abs(m_rigid.velocity.y) < climbSpeedLimit)
                {
                    if ((mY > 0 && !RayCast(1)) || (mY < 0 && !RayCast(-1)))
                    {
                        movement.y = mY * climbForce;
                    }
                }

            }
        }

        m_rigid.AddForce(movement);
    }
    protected void JumpCheck()
    {
        if (RayCast(-1))
        {
            canJump = true;
            if (tempDownForce != downForce)
            {
                tempDownForce = downForce;
            }
        }
        else
        {
            if (!isClimbing)
            {
                if (tempDownForce < maxDownForce)
                {
                    tempDownForce += downForceIncrement * Time.deltaTime;
                }
                m_rigid.AddForce(new Vector3(0f, -tempDownForce));
            }
        }
        if (mJump && canJump)
        {
            canJump = false;
            m_rigid.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            stat_jump++; // Adds one to jump stat
        }
    }
    protected void CatchCheck()
    {
        if (mCatch && ball != null)
        {
            if (!haveBall && ballInRange)
            {
                ballHolding = ball.transform.parent.gameObject;
                ball.transform.parent.GetComponent<Rigidbody>().useGravity = false;
                ball.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
                ball.transform.parent.transform.SetParent(transform);
                haveBall = true;
                stat_ballGrab++;
            }
        }
    }
    protected void ThrowCheck()
    {
        if (mCatch && haveBall)
        {
            haveBall = false;
            Rigidbody ballRigid = ball.transform.parent.GetComponent<Rigidbody>();
            ballRigid.useGravity = true;
            ballRigid.isKinematic = false;
            ballRigid.AddForce(mX * throwForce, mY * throwForce, 0f, ForceMode.Impulse);
            ball.transform.parent.transform.parent = null;
            ball = null;
            stat_throw++;
        }
        if (haveBall && ballHolding != null)
        {
            ballHolding.transform.position = new Vector3(m_rigid.transform.position.x, m_rigid.transform.position.y, 0f);
        }
    }
    protected bool RayCast(int direction)
    {
        bool hit = Physics.Raycast(m_rigid.position, direction * Vector3.up, transform.localScale.y / 2 + 0.07f, layerMask);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected bool RayCastSide(int leftOrRight)
    {
        bool hit = Physics.Raycast(m_rigid.position, leftOrRight * Vector3.right, transform.localScale.x / 2 + 0.05f, layerMask);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void Aim()
    {
        Debug.DrawLine(m_rigid.position, new Vector3(m_rigid.position.x + mX * 2, m_rigid.position.y + mY * 2));

    }
    public bool IsHoldingBall()
    {
        return haveBall;
    }
    protected void StompCheck()
    {

    }

}

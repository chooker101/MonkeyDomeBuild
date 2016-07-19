using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour
{
    /*
     * We need this class to:
     * - keep track of how many players are playing
     * - handle players' stats
     * - provide a key to accessing each player's stats 
     */
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
    protected bool mAimStomp;

    public int stat_jump = 0;
    public int stat_throw = 0;
    public int stat_ballGrab = 0;

    protected Character blah = new Gorilla();

    [SerializeField]
	private uint TNOP;
	[SerializeField]
	private uint NOP = 1;
	[SerializeField]
	private uint NOB = 0;

	public void CreatePlayers()
	{
		if (GameManager.Instance.gmPlayers.Contains(null))
		{
			TNOP = NOP + NOB;
			if (TNOP > 0)
			{
				for (int i = 0; i < TNOP; ++i)
				{
					Transform temp = GameManager.Instance.gmSpawnManager.SpawnPoints[i];
					if (NOP > 0)
					{
						GameManager.Instance.gmPlayers[i] = (GameObject)Instantiate(GameManager.Instance.gmPlayerPrefab, temp.position, temp.rotation);
						--NOP;
					}
					else if (NOB > 0)
					{
						GameManager.Instance.gmPlayers[i] = (GameObject)Instantiate(GameManager.Instance.gmPlayerPrefabAI, temp.position, temp.rotation);
						--NOB;
					}
				}
			}
		}
	}
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Floor");
        m_rigid = GetComponent<Rigidbody>();
        if(blah is Gorilla)
        {

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
    protected virtual void CatchCheck()
    {
    }
    protected void ThrowCheck()
    {
        if (mCatch && haveBall)
        {
            haveBall = false;
            ballHolding.GetComponent<BallInfo>().Reset();
            Rigidbody ballRigid = ballHolding.GetComponent<Rigidbody>();
            ballRigid.AddForce(mX * throwForce, mY * throwForce, 0f, ForceMode.Impulse);
            ball = null;
            ballHolding = null;
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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (!ballInRange && !other.transform.parent.GetComponent<Rigidbody>().isKinematic)
            {
                ballInRange = true;
                ball = other.gameObject;
            }
        }
        if (other.gameObject.CompareTag("Vine") && !canClimb)
        {
            canClimb = true;
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
        if (other.gameObject.CompareTag("Vine") && canClimb)
        {
            canClimb = false;
            isClimbing = false;
        }
    }
    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }
    public virtual void Mutate()
    {
    }
}

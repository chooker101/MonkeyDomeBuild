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


	public int whichplayer;
    
    public Vector3 mov;

    public bool canJump = true;
    public int layerMask;
    public bool ballInRange = false;
    public GameObject ballHolding = null;
    public bool haveBall = false;

    public bool isClimbing = false;
    public bool canClimb = false;

    public int stat_jump = 0;
    public int stat_throw = 0;
    public int stat_ballGrab = 0;

    public Character characterType;

    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Floor");
	}

	void Update()
	{
		CheckInputs();
		JumpCheck();
		Aim();
		mov = GetComponent<Rigidbody>().velocity;
		characterType.CHUpdate();
	}

	void FixedUpdate()
	{
		Movement();
		characterType.CHFixedUpdate();
	}

	public virtual void CheckInputs() { }

	public void Movement()
    {
        Vector3 movement = new Vector3();
        if (!isClimbing)
        {
            if (GameManager.Instance.gmInputs[whichplayer].mXY.x != 0 && Mathf.Abs(GetComponent<Rigidbody>().velocity.x) < characterType.speedLimit)
            {
                if ((GameManager.Instance.gmInputs[whichplayer].mXY.x > 0 && !RayCastSide(1)) || (GameManager.Instance.gmInputs[whichplayer].mXY.x < 0 && !RayCastSide(-1)))
                {
                    movement.x = GameManager.Instance.gmInputs[whichplayer].mXY.x * characterType.moveForce;
                }
            }
        }
        else
        {
            if (GameManager.Instance.gmInputs[whichplayer].mXY.x != 0 || GameManager.Instance.gmInputs[whichplayer].mXY.y != 0)
            {
                if (Mathf.Abs(GetComponent<Rigidbody>().velocity.x) < characterType.climbSpeedLimit)
                {
                    if ((GameManager.Instance.gmInputs[whichplayer].mXY.x > 0 && !RayCastSide(1)) || (GameManager.Instance.gmInputs[whichplayer].mXY.x < 0 && !RayCastSide(-1)))
                    {
                        movement.x = GameManager.Instance.gmInputs[whichplayer].mXY.x * characterType.climbForce;
                    }
                }
                if (Mathf.Abs(GetComponent<Rigidbody>().velocity.y) < characterType.climbSpeedLimit)
                {
                    if ((GameManager.Instance.gmInputs[whichplayer].mXY.y > 0 && !RayCast(1)) || (GameManager.Instance.gmInputs[whichplayer].mXY.y < 0 && !RayCast(-1)))
                    {
                        movement.y = GameManager.Instance.gmInputs[whichplayer].mXY.y * characterType.climbForce;
                    }
                }

            }
        }

		GetComponent<Rigidbody>().AddForce(movement);
    }
    public void JumpCheck()
    {
        if (RayCast(-1))
        {
            canJump = true;
            if (characterType.tempDownForce != characterType.downForce)
            {
				characterType.tempDownForce = characterType.downForce;
            }
        }
        else
        {
            if (!isClimbing)
            {
                if (characterType.tempDownForce < characterType.maxDownForce)
                {
					characterType.tempDownForce += characterType.downForceIncrement * Time.deltaTime;
                }
				GetComponent<Rigidbody>().AddForce(new Vector3(0f, -characterType.tempDownForce));
            }
        }
        if (GameManager.Instance.gmInputs[whichplayer].mJump && canJump)
        {
            canJump = false;
			GetComponent<Rigidbody>().AddForce(new Vector3(0, characterType.jumpForce, 0), ForceMode.Impulse);
            stat_jump++; // Adds one to jump stat
        }
    }

    public void ThrowCheck()
    {
        if (GameManager.Instance.gmInputs[whichplayer].mCatch && haveBall)
        {
            haveBall = false;
            ballHolding.GetComponent<BallInfo>().Reset();
            Rigidbody ballRigid = ballHolding.GetComponent<Rigidbody>();
            ballRigid.AddForce(GameManager.Instance.gmInputs[whichplayer].mXY.x * characterType.throwForce, GameManager.Instance.gmInputs[whichplayer].mXY.y * characterType.throwForce, 0f, ForceMode.Impulse);
            ballHolding = null;
            stat_throw++;
        }
    }

    public bool RayCast(int direction)
    {
        bool hit = Physics.Raycast(GetComponent<Rigidbody>().position, direction * Vector3.up, transform.localScale.y / 2 + 0.07f, layerMask);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RayCastSide(int leftOrRight)
    {
        bool hit = Physics.Raycast(GetComponent<Rigidbody>().position, leftOrRight * Vector3.right, transform.localScale.x / 2 + 0.05f, layerMask);
        if (hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Aim()
    {
        Debug.DrawLine(GetComponent<Rigidbody>().position, new Vector3(GetComponent<Rigidbody>().position.x + GameManager.Instance.gmInputs[whichplayer].mXY.x * 2, GetComponent<Rigidbody>().position.y + GameManager.Instance.gmInputs[whichplayer].mXY.y * 2));

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
}

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

	public Vector2 movement = Vector2.zero;
	public CameraController cam;

	//public bool canJump = true;
	public int layerMask;
    public int layerMaskPlayer;
    public bool ballInRange = false;
    public GameObject ballHolding = null;
    public bool haveBall = false;

    public bool isClimbing = false;
    public bool canClimb = false;
	public bool isinair;

	public int stat_jump = 0;
    public int stat_throw = 0;
    public int stat_ballGrab = 0;

    public float maxminInc = 3f;
    public float characterInc = 0f;

    public bool canCharge = false;
    public float holdingCatchCount = 0f;
    public float maxChargeCount = 10f;
    public float chargePerSec = 10f;
    public float chargeThrowRequireCount = 5f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
	protected Rigidbody2D cache_rb;

    public Character characterType;

    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        layerMask = 1 << LayerMask.NameToLayer("Floor");
        layerMaskPlayer = 1 << LayerMask.NameToLayer("Player");
		cache_rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		CheckInputs();
		//JumpCheck();
		if (GameManager.Instance.gmInputs[whichplayer].mJump)
		{
			Jumping();
		}
		Aim();
		characterType.CHUpdate();
	}

	void FixedUpdate()
	{
		MovementVelocity();
		AnimationControl();
		//Movement();
		characterType.CHFixedUpdate();
	}

	public virtual void CheckInputs() { }

	void Jumping()
	{
		if (!isinair)
		{
			isinair = true;
			cache_rb.AddForce(Vector2.up * characterType.jumpforce);
		}
		else
		{
			if (isClimbing)
			{
				isClimbing = false;
				cache_rb.AddForce(Vector2.up * characterType.jumpforce);
			}
			else if(canClimb)
			{
				isClimbing = true;
			}
		}
	}

	void MovementVelocity()
	{
		movement = cache_rb.velocity;
		if (!isClimbing)
		{
			if (!RayCastSide((int)GameManager.Instance.gmInputs[whichplayer].mXY.x))
			{
				movement.x = GameManager.Instance.gmInputs[whichplayer].mXY.x * characterType.movespeed;
			}
		}
		else
		{
			movement.x = GameManager.Instance.gmInputs[whichplayer].mXY.x * characterType.movespeed;
			movement.y = GameManager.Instance.gmInputs[whichplayer].mXY.y * characterType.movespeed;
		}
		cache_rb.velocity = movement;
	}

	/*
	public void Movement()
    {
		movement = Vector2.zero;
        if (!isClimbing)
        {
            //if player is NOT climbing on vines
			if (Mathf.Abs(cache_rb.velocity.x) < characterType.speedLimit + characterInc)
			{
                //if player is NOT trying to run into walls
				if (GameManager.Instance.gmInputs[whichplayer].mXY.x != 0)
				{
					if ((GameManager.Instance.gmInputs[whichplayer].mXY.x > 0 && !RayCastSide(1)) || (GameManager.Instance.gmInputs[whichplayer].mXY.x < 0 && !RayCastSide(-1)))
					{
                        float changeDirectionMultipiler = 1f;
                        if ((GameManager.Instance.gmInputs[whichplayer].mXY.x > 0 && cache_rb.velocity.x < 0) ||
                            (GameManager.Instance.gmInputs[whichplayer].mXY.x < 0 && cache_rb.velocity.x > 0))
                            changeDirectionMultipiler = 1.2f;
						movement.x = GameManager.Instance.gmInputs[whichplayer].mXY.x * characterType.horizontalMoveForce * changeDirectionMultipiler;
					}
				}
			}
			else
			{
				movement.x = characterType.speedLimit + characterInc;
			}
			//apply force on actor
			cache_rb.AddForce(movement);
        }
        else
        {
            //if player is climbing on vines
            if (GameManager.Instance.gmInputs[whichplayer].mXY.x != 0 || GameManager.Instance.gmInputs[whichplayer].mXY.y != 0)
            {
                if ((GameManager.Instance.gmInputs[whichplayer].mXY.x > 0 && !RayCastSide(1)) ||
                    (GameManager.Instance.gmInputs[whichplayer].mXY.x < 0 && !RayCastSide(-1)))
                {
                    //if player is NOT trying to run into walls
                    //if player is NOT trying to run into a gorilla
                    float dir = 1f;
                    if (GameManager.Instance.gmInputs[whichplayer].mXY.x > 0) dir = 1f;
                    else if (GameManager.Instance.gmInputs[whichplayer].mXY.x < 0) dir = -1f;
                    movement.x = GameManager.Instance.gmInputs[whichplayer].mXY.x * characterType.climbingHorizontalMoveSpeed + characterInc * dir;
                }
                if ((GameManager.Instance.gmInputs[whichplayer].mXY.y > 0 && !RayCast(1)) ||
                    (GameManager.Instance.gmInputs[whichplayer].mXY.y < 0 && !RayCast(-1)))
                {
                    float dir = 1f;
                    if (GameManager.Instance.gmInputs[whichplayer].mXY.y > 0) dir = 1f;
                    else if (GameManager.Instance.gmInputs[whichplayer].mXY.y < 0) dir = -1f;
                    movement.y = GameManager.Instance.gmInputs[whichplayer].mXY.y * characterType.climbingVerticalMoveSpeed + characterInc * dir;
                }
            }
			cache_rb.velocity = movement;
        }
    }*/
	/*
    public void JumpCheck()
    {
        if (RayCast(-1))
        {
            //if player is on ground or platform
            canJump = true;
            if (characterType.tempDownForce != characterType.downForce)
            {
				characterType.tempDownForce = characterType.downForce;
            }
        }
        else
        {
            //if player is not on ground
            if (!isClimbing)
            {
                //if player is not climbing
                if (GetComponent<Rigidbody2D>().isKinematic) ChangeIsKinematic();
                if (GetComponent<Rigidbody2D>().velocity.y < 0f && !RayCast(-1))
                {
                    //if player is free falling
                    GetComponent<Rigidbody2D>().AddForce(Vector2.down * characterType.downForce * Time.deltaTime);
                }
				//GetComponent<Rigidbody>().AddForce(new Vector3(0f, -characterType.tempDownForce));
            }
        }
        if (GameManager.Instance.gmInputs[whichplayer].mJump && canJump)
        {
            canJump = false;
            if (GetComponent<Rigidbody2D>().isKinematic && isClimbing) ChangeIsKinematic();
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, characterType.jumpForce),ForceMode2D.Impulse);
            stat_jump++; // Adds one to jump stat PUT IN STAT MANAGER
        }
    }*/

    public void ThrowCheck()
    {
        if (canCharge)
        {
            if (GameManager.Instance.gmInputs[whichplayer].mChargeThrow && haveBall)
            {
                if (holdingCatchCount < maxChargeCount)
                {
                    holdingCatchCount += chargePerSec * Time.deltaTime;
                }
                else
                {
                    holdingCatchCount = maxChargeCount;
                }
            }
            else
            {
                if (holdingCatchCount > 0f)
                {
                    float tempThrowForce = characterType.throwForce;
                    if (holdingCatchCount > chargeThrowRequireCount)
                    {
                        tempThrowForce *= (1 + (holdingCatchCount / maxChargeCount / 2f));
                    }
                    haveBall = false;
                    ballHolding.GetComponent<BallInfo>().Reset();
                    Rigidbody2D ballRigid = ballHolding.GetComponent<Rigidbody2D>();
                    ballRigid.AddForce(new Vector2(GameManager.Instance.gmInputs[whichplayer].mXY.x * tempThrowForce, GameManager.Instance.gmInputs[whichplayer].mXY.y * tempThrowForce), ForceMode2D.Impulse);
                    ballHolding = null;
                    stat_throw++;
                    holdingCatchCount = 0f;
                }
            }
        }
        else
        {
            if (GameManager.Instance.gmInputs[whichplayer].mCatchRelease)
            {
                canCharge = true;
            }
        }

        /*
        if (GameManager.Instance.gmInputs[whichplayer].mCatch && haveBall)
        {
            haveBall = false;
            ballHolding.GetComponent<BallInfo>().Reset();
			Rigidbody2D ballRigid = ballHolding.GetComponent<Rigidbody2D>();
            ballRigid.AddForce(new Vector2(GameManager.Instance.gmInputs[whichplayer].mXY.x * characterType.throwForce, GameManager.Instance.gmInputs[whichplayer].mXY.y * characterType.throwForce),ForceMode2D.Impulse);
            ballHolding = null;
            stat_throw++;
        }
        */
    }
    public bool RayCast(int direction)
    {
        bool hit = false;
        float falloffX = transform.localScale.x / 2 - 0.1f;
        for(int i = 0; i < 3; i++)
        {
            float falloff = transform.position.x;
            if (i != 0) falloff += Mathf.Pow(-1, i) * falloffX;
            RaycastHit2D hitInfo;
            Vector2 checkPos = transform.position;
            checkPos.x = falloff;
            hitInfo = Physics2D.Raycast(checkPos, direction * Vector2.up, transform.localScale.y / 2 + 0.07f, layerMask);
            Debug.DrawLine(checkPos, checkPos + Vector2.up*direction);
            if (hitInfo.collider != null)
            {
                hit = true;
            }
        }
        return hit;
    }
    public bool RayCastGorilla(int leftOrRight)
    {
        RaycastHit2D hitInfo;
        hitInfo = Physics2D.Raycast(GetComponent<Rigidbody2D>().position, leftOrRight * Vector2.right, transform.localScale.x / 2 + 0.05f, layerMaskPlayer);


        return false;
    }

    public bool RayCastSide(float leftOrRight)
    {
		// right = 1    left = -1
		if (leftOrRight > 0.0f || leftOrRight < 0.0f)
		{
			if(leftOrRight > 0.0f)
			{
				leftOrRight = 1.0f;
			}
			else
			{
				leftOrRight = -1.0f;
			}
			bool hit = false;
			float falloffY = transform.localScale.y / 2 - 0.02f;
			for (int i = 0; i < 3; i++)
			{
				float falloff = transform.position.y;
				if (i != 0) falloff += Mathf.Pow(-1, i) * falloffY;
				RaycastHit2D hitInfo;
				Vector2 checkPos = transform.position;
				checkPos.y = falloff;
				hitInfo = Physics2D.Raycast(checkPos, leftOrRight * Vector2.right, transform.localScale.x / 2 + 0.05f, layerMask);
				Debug.DrawLine(checkPos, checkPos + Vector2.right * leftOrRight);
				if (hitInfo.collider != null)
				{
					hit = true;
				}
			}
			return hit;
		}
		return false;
    }

    public void Aim()
    {
        Debug.DrawLine(GetComponent<Rigidbody2D>().position, new Vector2(GetComponent<Rigidbody2D>().position.x + GameManager.Instance.gmInputs[whichplayer].mXY.x * 2, GetComponent<Rigidbody2D>().position.y + GameManager.Instance.gmInputs[whichplayer].mXY.y * 2));
    }

    public bool IsHoldingBall()
    {
        return haveBall;
    }

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Floor"))
		{
			isinair = false;
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Floor"))
		{
			isinair = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Vine") && !canClimb)
		{
			canClimb = true;
		}
		if (other.gameObject.CompareTag("Ball"))
		{
			if (!ballInRange && !other.transform.parent.GetComponent<Rigidbody2D>().isKinematic)
			{
				ballInRange = true;
			}
		}
		if (other.gameObject.CompareTag("Banana"))
		{
			ProjectileBehavior proj = other.GetComponent<ProjectileBehavior>();
			//if (proj == null) return;
			if (proj.GetCanEffectCharacter())
			{
				proj.CollideWithCharacter();
				ReactionToBanana(proj.GetIncAmount());
				Destroy(other.gameObject);
			}
		}

		if (other.gameObject.CompareTag("Poop"))
		{
			ProjectileBehavior proj = other.GetComponent<ProjectileBehavior>();
			//if (proj == null) return;
			if (proj.GetCanEffectCharacter())
			{
				proj.CollideWithCharacter();
				ReactionToPoop(proj.GetIncAmount());
				Destroy(other.gameObject);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Ball"))
		{
			ballInRange = false;
			haveBall = false;
			ballHolding = null;
		}
		if (other.gameObject.CompareTag("Vine"))
		{
			canClimb = false;
			isClimbing = false;
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		OnTriggerEnter2D(other);
	}

    public void ReactionToBanana(float incAmount)
    {
        if (characterInc < maxminInc)
        ChangeInc(incAmount);
    }

    public void ReactionToPoop(float incAmount)
    {
        if (Mathf.Abs(characterInc) < maxminInc)
        ChangeInc(-incAmount);
    }

    protected void ChangeInc(float inc)
    {
        characterInc += inc;
        //Debug.Log(characterInc);
    }

	/*
    protected void ChangeIsKinematic()
    {
        GetComponent<Rigidbody2D>().isKinematic = !GetComponent<Rigidbody2D>().isKinematic;
    }*/

    protected void AnimationControl()
    {
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.x);
        if (GetComponent<Rigidbody2D>().velocity.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 1f || GameManager.Instance.gmInputs[whichplayer].mXY.x != 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
}

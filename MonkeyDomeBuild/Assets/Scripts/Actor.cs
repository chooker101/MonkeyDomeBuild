using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Actor : MonoBehaviour
{
    /*
     * We need this class to:
     * - keep track of how many players are playing
     * - handle players' stats
     * - provide a key to accessing each player's stats 
     */

    public int playerIndex;
    public bool isPlayer;

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
    protected Transform cache_tf;

    public Character characterType;

    private GameObject monkeyCrown;
    private RecordKeeper rk_keeper;

    public BallInfo ballCanCatch;

    bool isDashing = false;
    float dashingCount = 0;
    float dashingTime = 0.5f;
    float dashForce = 30f;

    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        layerMask = 1 << LayerMask.NameToLayer("Floor");
        layerMaskPlayer = 1 << LayerMask.NameToLayer("Player");
		cache_rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		cache_tf = GetComponent<Transform>();

        monkeyCrown = transform.Find("Crown").gameObject;
        monkeyCrown.SetActive(false);

        rk_keeper = FindObjectOfType<RecordKeeper>().GetComponent<RecordKeeper>();
	}

	void Update()
	{
		CheckInputs();
		//JumpCheck();
		if (GameManager.Instance.gmInputs[playerIndex].mJump)
		{
			Jumping();
		}
		Aim();
		characterType.CHUpdate();
        CheckLeader();
        UpdateColour();
	}

    void FixedUpdate()
	{
		MovementVelocity();
		AnimationControl();
		//Movement();
		characterType.CHFixedUpdate();
	}

	public virtual void CheckInputs() { }


    public bool IsInAir
    {
        get { return isinair; }
    }


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
                if (cache_rb.gravityScale == 0)
                    cache_rb.gravityScale = 2;

                cache_rb.AddForce(Vector2.up * characterType.jumpforce);
			}
			else if(canClimb)
			{
				isClimbing = true;
                if (cache_rb.gravityScale != 0)
                    cache_rb.gravityScale = 0;
            }
		}
	}

	void MovementVelocity()
	{
		movement = cache_rb.velocity;
		if (!isClimbing)
		{
			if (!RayCastSide(GameManager.Instance.gmInputs[playerIndex].mXY.x))
			{
                if (!isDashing)
                {
                    if (characterType is Gorilla && characterType.isCharging)
                    {
                        movement.x = GameManager.Instance.gmInputs[playerIndex].mXY.x * characterType.chargespeed;
                    }
                    else
                    {
                        movement.x = GameManager.Instance.gmInputs[playerIndex].mXY.x * characterType.movespeed;
                    }
                }
                else
                {
                    if (dashingCount >= dashingTime)
                    {
                        dashingCount = 0;
                        isDashing = false;
                    }
                    else
                    {
                        dashingCount += Time.fixedDeltaTime;
                    }
                }
			}
		}
		else
		{
            movement.x = GameManager.Instance.gmInputs[playerIndex].mXY.x * characterType.movespeed;
            movement.y = GameManager.Instance.gmInputs[playerIndex].mXY.y * characterType.movespeed;
		}
		cache_rb.velocity = movement;
	}

    public void ThrowCheck()
    {
        if (canCharge)
        {
            if (GameManager.Instance.gmInputs[playerIndex].mChargeThrow && haveBall)
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
                    ballRigid.AddForce(new Vector2(GameManager.Instance.gmInputs[playerIndex].mXY.x * tempThrowForce, GameManager.Instance.gmInputs[playerIndex].mXY.y * tempThrowForce), ForceMode2D.Impulse);
                    ballHolding.GetComponent<BallInfo>().playerThrewLast = playerIndex;
                    ballHolding = null;
                    stat_throw++;
                    holdingCatchCount = 0f;
                }
            }
        }
        else
        {
            if (GameManager.Instance.gmInputs[playerIndex].mCatchRelease)
            {
                canCharge = true;
            }
        }
    }
    public bool RayCast(int direction)
    {
        bool hit = false;
        float falloffX = transform.localScale.x / 2 - 0.1f;
        for (int i = 0; i < 3; i++)
        {
            float falloff = transform.position.x;
            if (i != 0) falloff += Mathf.Pow(-1, i) * falloffX;
            RaycastHit2D hitInfo;
            Vector2 checkPos = transform.position;
            checkPos.x = falloff;
            hitInfo = Physics2D.Raycast(checkPos, direction * Vector2.up, transform.localScale.y / 2 + 0.07f, layerMask);
            Debug.DrawLine(checkPos, checkPos + Vector2.up * direction);
            if (hitInfo.collider != null)
            {
                hit = true;
            }
        }
        return hit;
    }

    public bool RayCastSide(float leftOrRight)
    {
        // right = 1    left = -1
        if (leftOrRight > 0.0f || leftOrRight < 0.0f)
        {

            if (leftOrRight > 0.0f)
            {
                leftOrRight = 1.0f;
            }
            else
            {
                leftOrRight = -1.0f;
            }

            BoxCollider2D cachebox = GetComponent<BoxCollider2D>();

            bool hit = false;
            RaycastHit2D hitInfo;
            Vector2 checkPosStart;
            //checkPos.x = ((cache_tf.position.x - cachebox.offset.x) + (((cachebox.size.x /2) + 0.02f) * leftOrRight)) * cache_tf.localScale.x;
            //checkPos.y = ((cache_tf.position.y - cachebox.offset.y) + ((cachebox.size.y / 2) + 0.02f)) * cache_tf.localScale.y;
            //Debug.Log("CeckPos" + (checkPos.x - cachebox.size.x));
            checkPosStart.x = transform.position.x + (cachebox.size.x / 2 + 0.1f) * leftOrRight * transform.localScale.x;
            checkPosStart.y = transform.position.y - (cachebox.size.y / 2 - cachebox.offset.y) * transform.localScale.y;
            //Vector2 scale = new Vector2(transform.localScale.x, transform.localScale.y);
            Vector2 tempV = checkPosStart;
            tempV.y += (cachebox.size.y * transform.localScale.y) + 0.1f;
            Debug.DrawLine(checkPosStart, tempV);
            hitInfo = Physics2D.Raycast(checkPosStart, Vector2.up, (cachebox.size.y * transform.localScale.y) + 0.1f, layerMask);
            //hitInfo = Physics2D.Raycast(checkPos, Vector2.down, (cachebox.size.y + 0.02f) * cache_tf.localScale.y, layerMask);
            if (hitInfo.collider != null)
            {
                hit = true;
            }
            return hit;
        }
        return false;
    }

    public void Aim()
    {
        Debug.DrawLine(GetComponent<Rigidbody2D>().position, new Vector2(GetComponent<Rigidbody2D>().position.x + GameManager.Instance.gmInputs[playerIndex].mXY.x * 2, GetComponent<Rigidbody2D>().position.y + GameManager.Instance.gmInputs[playerIndex].mXY.y * 2));
    }

    public bool IsHoldingBall()
    {
        return haveBall;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor")|| other.gameObject.CompareTag("Wall"))
        {
            isinair = false;
            if (isDashing)
            {
                for (int i = 0; i < GameManager.Instance.gmPlayers.Capacity; ++i)
                {
                    Character p = GameManager.Instance.gmPlayers[i].GetComponent<Actor>().characterType;
                    if (p is Monkey)
                    {
                        //knock both player off vine for now
                        GameManager.Instance.gmPlayers[i].GetComponent<Player>().isClimbing = false;
                    }
                }
                cam.ScreenShake();
            }
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
        if (other.gameObject.CompareTag("BallTrigger"))
        {
            ballCanCatch = other.gameObject.GetComponentInParent<BallInfo>();
            if (!ballInRange)
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
        if (other.gameObject.CompareTag("BallTrigger"))
        {
            ballInRange = false;
            if (ballHolding != null)
            {
                if (other.gameObject.GetComponentInParent<BallInfo>() == ballHolding.GetComponent<BallInfo>())
                {
                    ballHolding = null;
                    haveBall = false;
                }
            }
        }
        if (other.gameObject.CompareTag("Vine"))
        {
            canClimb = false;
            isClimbing = false;
            if (cache_rb.gravityScale == 0)
                cache_rb.gravityScale = 2;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    public void ReactionToBanana(float incAmount)
    {
        if (characterInc < maxminInc)
            IncrementCharacterInc(incAmount);
    }

    public void ReactionToPoop(float incAmount)
    {
        if (Mathf.Abs(characterInc) < maxminInc)
            IncrementCharacterInc(-incAmount);
    }

    protected void IncrementCharacterInc(float inc)
    {
        characterInc += inc;
    }

    void CheckLeader()
    {
        if (GameManager.Instance.gmScoringManager.p1Score == GameManager.Instance.gmScoringManager.p2Score && GameManager.Instance.gmScoringManager.p2Score == GameManager.Instance.gmScoringManager.p3Score)
        {
            monkeyCrown.SetActive(false);
        }
		else if(
                    (
                    playerIndex == 0 && 
                    GameManager.Instance.gmScoringManager.p1Score >= GameManager.Instance.gmScoringManager.p2Score &&
					GameManager.Instance.gmScoringManager.p1Score >= GameManager.Instance.gmScoringManager.p3Score
                    ) 
                    ||
                    (
                    playerIndex == 1 &&
					GameManager.Instance.gmScoringManager.p2Score >= GameManager.Instance.gmScoringManager.p1Score &&
					GameManager.Instance.gmScoringManager.p2Score >= GameManager.Instance.gmScoringManager.p3Score
                    ) 
                    ||
                    (
                    playerIndex == 2 &&
					GameManager.Instance.gmScoringManager.p3Score >= GameManager.Instance.gmScoringManager.p1Score &&
					GameManager.Instance.gmScoringManager.p3Score >= GameManager.Instance.gmScoringManager.p2Score
                    )
                )
        {
            monkeyCrown.SetActive(true);
        }
        else
        {
            monkeyCrown.SetActive(false);
        }
    }

    private void UpdateColour()
    {
		
        if (rk_keeper != null)
        {
            for (int i = 0; i < rk_keeper.GetComponent<RecordKeeper>().colourPlayers.Length; i++)
            {
                if (playerIndex == i)
                {
                    GetComponent<SpriteRenderer>().material = rk_keeper.GetComponent<RecordKeeper>().colourPlayers[i];
                }
            }
        }
    }

    protected void AnimationControl()
    {
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.x);
        if (cache_rb.velocity.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (cache_rb.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        if (Mathf.Abs(cache_rb.velocity.x) > 1f || GameManager.Instance.gmInputs[playerIndex].mXY.x != 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
    public void GorillaDash()
    {
        isDashing = true;
        Vector2 dashDir = Vector2.zero;
        dashDir.y = 0.4f;
        if (Mathf.Abs(GameManager.Instance.gmInputs[playerIndex].mXY.x) > 0)
        {
            dashDir.x = GameManager.Instance.gmInputs[playerIndex].mXY.x > 0 ? 1f : -1f;
            //dashDir.x = GameManager.Instance.gmInputs[whichplayer].mXY.x > 0 ? 0.5f : -1.2f;
        }
        if (Mathf.Abs(GameManager.Instance.gmInputs[playerIndex].mXY.y) > 0)
        {
            dashDir.y = GameManager.Instance.gmInputs[playerIndex].mXY.y > 0 ? 1f : -1f;
        }
        dashDir *= dashForce;
        GetComponent<Rigidbody2D>().AddForce(dashDir, ForceMode2D.Impulse);
    }
}

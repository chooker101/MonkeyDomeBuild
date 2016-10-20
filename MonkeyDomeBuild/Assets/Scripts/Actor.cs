using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
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
	//public CameraController cam;


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

    private float maxminInc = 2f;
    public float characterInc = 0f;
    private float incAmount = 0.5f;

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
	[SerializeField]
    private GameObject monkeyCrown;
    public BallInfo ballCanCatch;
	[SerializeField]
	protected GameObject shotPointer;
	protected Color col;
    public Transform catchCenter;
    public BoxCollider2D raycastCol;

	protected bool beingSmack = false;
    public bool justJump = false;
    protected bool isDashing = false;
    protected float dashingCount = 0;
    protected float dashingTime = 0.6f;
    protected float dashForce = 40f;
    protected float smackImpulse = 15f;
    protected float disableInputTime = 2f;

    protected bool isCharging = false;
    protected bool canBeInSlowMotion = true;
    protected bool cantHoldAnymore = false;
    protected bool startSlowMo = false;
    protected float slowMoTime = 2f;
    protected float slowMoTimeScale = 0.2f;
    protected float slowMoCount = 0;

    public float speedMultiplier = 1f;

    public string cType = "";

    public bool DisableInput
    {
        get { return beingSmack; }
        set { beingSmack = value; }
    }
    void Start()
    {
		DontDestroyOnLoad(this.gameObject);

        layerMask = 1 << LayerMask.NameToLayer("Floor");
        layerMaskPlayer = 1 << LayerMask.NameToLayer("Player");
		cache_rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		cache_tf = GetComponent<Transform>();
        catchCenter = transform.FindChild("CatchCenter").transform;
        raycastCol = transform.FindChild("RayCastCol").GetComponent<BoxCollider2D>();
        monkeyCrown.SetActive(false);

		
        col = shotPointer.GetComponentInChildren<Image>().color;
    }

	void Update()
	{
        //JumpCheck();
        cType = characterType.ToString();
		if (GameManager.Instance.gmInputs[playerIndex].mJump)
		{
			Jumping();
		}
		Aim();
		characterType.CHUpdate();
        CheckLeader();
	}

    void FixedUpdate()
	{
        MovementVelocity();
        AnimationControl();
		//Movement();
		characterType.CHFixedUpdate();
        if (IsHoldingBall && ballHolding == null)
        {
            ReleaseBall();
        }
	}

	//public virtual void CheckInputs() { }


    public bool IsInAir
    {
        get { return isinair; }
    }
	protected void Jumping()
	{
		if (!isinair)
		{
			isinair = true;
            justJump = true;
            Invoke("ResetJustJump", 0.3f);
            if (ParticlesManager.Instance != null)
            {
                GameObject tempParticle = null;
                if (characterType is Monkey)
                {
                    tempParticle = ParticlesManager.Instance.JumpParticle;
                }
                if (tempParticle != null)
                {
                    tempParticle.SetActive(true);
                    tempParticle.transform.position = transform.position;
                }

            }
            cache_rb.AddForce(Vector2.up * characterType.jumpforce,ForceMode2D.Impulse);
            if (AudioEffectManager.Instance != null)
            {
                AudioEffectManager.Instance.PlayMonkeyJumpSE();
            }

		}
		else
		{
			if (isClimbing)
			{
				isClimbing = false;
                if (cache_rb.gravityScale == 0)
                    cache_rb.gravityScale = 2;

                if (GameManager.Instance.gmInputs[playerIndex].mXY.y >= 0)
                {
                    cache_rb.velocity *= 0.3f;
                    cache_rb.AddForce(Vector2.up * characterType.jumpforce, ForceMode2D.Impulse);
                    if (AudioEffectManager.Instance != null)
                    {
                        AudioEffectManager.Instance.PlayMonkeyJumpSE();
                    }
                }
            }
			else if(canClimb)
			{
				isClimbing = true;
                if (cache_rb.gravityScale != 0)
                    cache_rb.gravityScale = 0;
            }
		}
	}
    protected void ResetJustJump()
    {
        justJump = false;
    }
	protected void MovementVelocity()
	{
        movement = cache_rb.velocity;
        if (!isClimbing)
		{
			if (!RayCastSide(GameManager.Instance.gmInputs[playerIndex].mXY.x))
			{
                if (!isDashing)
                {
                    /*if (characterType is Gorilla && characterType.isCharging)
                    {
                        movement.x = GameManager.Instance.gmInputs[playerIndex].mXY.x * (characterType.chargespeed + characterInc);
                    }
                    else
                    {
                        movement.x = GameManager.Instance.gmInputs[playerIndex].mXY.x * (characterType.movespeed + characterInc);
                    }*/
                    movement.x = GameManager.Instance.gmInputs[playerIndex].mXY.x * (characterType.movespeed + characterInc);
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
            movement.x = GameManager.Instance.gmInputs[playerIndex].mXY.x * (characterType.movespeed + characterInc);
            movement.y = GameManager.Instance.gmInputs[playerIndex].mXY.y * (characterType.movespeed + characterInc);
        }
        if(!beingSmack)
        {
            if (startSlowMo)
            {
                cache_rb.velocity = movement * speedMultiplier;
            }
            else
            {
                cache_rb.velocity = movement;
            }

        }
	}
	public void ThrowCheck()
    {
        if (canCharge)
        {
            if (GameManager.Instance.gmInputs[playerIndex].mChargeThrow && haveBall && !cantHoldAnymore)
            {
                isCharging = true;
                /*
                if (FindObjectOfType<PreGameTimer>() == null)
                {
                    if (Time.timeScale == 1f && canBeInSlowMotion)
                    {
                        canBeInSlowMotion = false;
                        startSlowMo = true;
                    }
                }
                if (startSlowMo)
                {
                    slowMoCount += Time.unscaledDeltaTime;
                    if (slowMoCount < slowMoTime * 0.8f)
                    {
                        Time.timeScale = Mathf.Lerp(Time.timeScale, slowMoTimeScale, Time.unscaledDeltaTime * 5f);
                    }
                    else if(slowMoCount < slowMoTime)
                    {
                        Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.unscaledDeltaTime * 5f);
                    }
                    else
                    {
                        cantHoldAnymore = true;
                    }
                }
                */
                if (!startSlowMo)
                {
                    startSlowMo = true;
                }
                if (startSlowMo)
                {
                    speedMultiplier = Mathf.Lerp(speedMultiplier, slowMoTimeScale, Time.deltaTime * 5f);
                }
                if (holdingCatchCount < maxChargeCount)
                {
                    holdingCatchCount += chargePerSec * Time.unscaledDeltaTime;
                }
                else
                {
                    holdingCatchCount = maxChargeCount;
                }
            }
            else
            {
                cantHoldAnymore = false;
                //ResetTimeScale();
                if (holdingCatchCount > 0f)
                {
                    float tempThrowForce = characterType.throwForce;
                    if (holdingCatchCount > chargeThrowRequireCount)
                    {
                        tempThrowForce *= (1 + (holdingCatchCount / maxChargeCount / 2f));
                    }
                    Rigidbody2D ballRigid = ballHolding.GetComponent<Rigidbody2D>();
                    if (!ballHolding.GetComponent<BallInfo>().IsBall)
                    {
                        if (ballHolding.GetComponent<TrophyInfo>().IsColliderOff)
                        {
                            ballHolding.GetComponent<TrophyInfo>().InvokeEnableCollider();
                        }
                    }
                    ReleaseBall();
                    ballRigid.AddForce(new Vector2(GameManager.Instance.gmInputs[playerIndex].mXY.x * tempThrowForce, GameManager.Instance.gmInputs[playerIndex].mXY.y * tempThrowForce), ForceMode2D.Impulse);
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
    public void ReleaseBall()
    {
        speedMultiplier = 1f;
        startSlowMo = false;
        haveBall = false;
        if (ballHolding != null)
        {
            ballHolding.GetComponent<BallInfo>().Reset();
        }
        ballHolding = null;
        isCharging = false;
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
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

            BoxCollider2D cachebox = raycastCol;
            bool hit = false;
            RaycastHit2D hitInfo;
            Vector2 checkPosStart;
            checkPosStart.x = transform.position.x + (cachebox.size.x / 2 + 0.1f) * leftOrRight * transform.localScale.x;
            checkPosStart.y = transform.position.y - (cachebox.size.y / 2 - cachebox.offset.y) * transform.localScale.y;
            Vector2 tempV = checkPosStart;
            tempV.y += (cachebox.size.y * transform.localScale.y) + 0.1f;
            Debug.DrawLine(checkPosStart, tempV);
            hitInfo = Physics2D.Raycast(checkPosStart, Vector2.up, (cachebox.size.y * transform.localScale.y) + 0.1f, layerMask);
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.CompareTag("Ramp"))
                {
                    hit = false;
                }
                else
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
        if (isCharging)
        {
            col.a = Mathf.Lerp(shotPointer.GetComponentInChildren<Image>().color.a, 1, Time.unscaledDeltaTime * 5f);
			shotPointer.GetComponentInChildren<Image>().color = col;
            if (GameManager.Instance.gmInputs[playerIndex].mXY.x != 0 || GameManager.Instance.gmInputs[playerIndex].mXY.y != 0)
            {
                Vector3 dir = new Vector3(GameManager.Instance.gmInputs[playerIndex].mXY.x, GameManager.Instance.gmInputs[playerIndex].mXY.y, 0);
                Quaternion targetAng = Quaternion.FromToRotation(Vector3.right, dir);
                if (targetAng.eulerAngles.y == 180f)
                {
                    shotPointer.transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(shotPointer.transform.localEulerAngles.z, targetAng.eulerAngles.y, 20 * Time.unscaledDeltaTime));
                }
                else
                {
                    shotPointer.transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(shotPointer.transform.localEulerAngles.z, targetAng.eulerAngles.z, 20 * Time.unscaledDeltaTime));
                }
            }
        }
        else
        {
            col.a = Mathf.Lerp(shotPointer.GetComponentInChildren<Image>().color.a, 0, Time.unscaledDeltaTime * 10f);
            shotPointer.GetComponentInChildren<Image>().color = col;
        }


    }
    public bool IsHoldingBall
    {
        get
        {
            return haveBall;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (isDashing)
            {
                dashingCount = 0;
                isDashing = false;
                for (int i = 0; i < GameManager.Instance.gmPlayers.Capacity; ++i)
                {
                    if (GameManager.Instance.gmPlayers[i] != null)
                    {
                        Character p = GameManager.Instance.gmPlayers[i].GetComponent<Actor>().characterType;
                        if (p is Monkey)
                        {
                            //knock both player off vine for now
                            if(GameManager.Instance.gmPlayers[i].GetComponent<Player>().isClimbing ||
                                !GameManager.Instance.gmPlayers[i].GetComponent<Player>().IsInAir)
                            {
                                GameManager.Instance.gmPlayers[i].GetComponent<Player>().isClimbing = false;
                                GameManager.Instance.gmPlayers[i].GetComponent<Rigidbody2D>().isKinematic = false;
                                GameManager.Instance.gmPlayers[i].GetComponent<Actor>().TempDisableInput();
                                if (GameManager.Instance.gmPlayers[i].GetComponent<Actor>().IsHoldingBall)
                                {
                                    GameManager.Instance.gmPlayers[i].GetComponent<Actor>().ReleaseBall();
                                }
                            }
                        }
                    }
                }
                FindObjectOfType<CameraController>().ScreenShake();
            }
        }
        if (characterType is Gorilla)
        {
            if (other.collider.CompareTag("Player"))
            {
                if (other.collider.GetComponent<Actor>().characterType is Monkey)
                {
                    KnockOffMonkey(other.collider.gameObject);
                }
            }
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (characterType is Gorilla)
        {
            if (other.collider.CompareTag("Player"))
            {
                OnCollisionEnter2D(other);
            }
        }
    }
    protected void KnockOffMonkey(GameObject monkey)
    {
        if (!monkey.GetComponent<Actor>().DisableInput)
        {
            GameManager.Instance.gmTrophyManager.KnockDowns(playerIndex);
        }
        monkey.GetComponent<Actor>().DisableInput = true;
        monkey.GetComponent<Actor>().InvokeEnableInput();
		if (monkey.GetComponent<Actor>().IsHoldingBall) 
		{
			monkey.GetComponent<Actor>().ReleaseBall();
		}
        Vector2 dir = -1*(transform.position - monkey.transform.position).normalized;
        if (!isinair)
        {
            dir.y = 0.3f;
        }
        else if(isinair)
        {
            if (!monkey.GetComponent<Actor>().IsInAir)
            {
                dir.y = 1f;
            }
        }
        monkey.GetComponent<Rigidbody2D>().AddForce(dir * smackImpulse, ForceMode2D.Impulse);
    }
    public void TempDisableInput()
    {
        DisableInput = true;
        InvokeEnableInput();
    }
    public void InvokeEnableInput()
    {
        Invoke("ResetBeingSmack", disableInputTime);
    }
    protected void ResetBeingSmack()
    {
        DisableInput = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (!justJump && isinair)
            {
                isinair = false;
            }
        }
        if (other.gameObject.CompareTag("Vine"))
        {
            if (!canClimb)
            {
                canClimb = true;
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("BallTrigger"))
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
                ReactionToBanana(incAmount);
                Destroy(other.gameObject);
                GameManager.Instance.gmTrophyManager.BananasEaten(playerIndex);
                //Audience call for Bananas event
                if (GameManager.Instance.gmAudienceManager.GetEventActive())
                {
                    if(GameManager.Instance.gmAudienceManager.GetCurrentEvent() == AudienceManager.AudienceEvent.Bananas)
                    {
                        GameManager.Instance.gmAudienceManager.BananaCaught(playerIndex);
                    }
                }
            }
        }

        if (other.gameObject.CompareTag("Poop"))
        {
            ProjectileBehavior proj = other.GetComponent<ProjectileBehavior>();
            //if (proj == null) return;
            if (proj.GetCanEffectCharacter())
            {
                proj.CollideWithCharacter();
                ReactionToPoop(incAmount);
                Destroy(other.gameObject);
                //TODO add poop event logic
                //Audience opinion increase when hit by poop
                GameManager.Instance.gmTrophyManager.BeingHitByPoop(playerIndex);
                GameManager.Instance.gmAudienceManager.HitByPoop(playerIndex);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (!isinair)
            {
                isinair = true;
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("BallTrigger"))
        {
            ballInRange = false;
            if (ballHolding != null)
            {
                if (other.gameObject.GetComponentInParent<BallInfo>() == ballHolding.GetComponent<BallInfo>())
                {
                    ReleaseBall();
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

	protected void CheckLeader()
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
	public void UpdateColour()
    {
        GetComponentInChildren<SpriteRenderer>().material = GameManager.Instance.gmRecordKeeper.colourPlayers[playerIndex];
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
        dashDir.y = 1f;
        if (Mathf.Abs(GameManager.Instance.gmInputs[playerIndex].mXY.x) > 0)
        {
            dashDir.x = GameManager.Instance.gmInputs[playerIndex].mXY.x > 0 ? 1f : -1f;
            //dashDir.x = GameManager.Instance.gmInputs[whichplayer].mXY.x > 0 ? 0.5f : -1.2f;
        }
        if (Mathf.Abs(GameManager.Instance.gmInputs[playerIndex].mXY.y) > 0)
        {
            dashDir.y = GameManager.Instance.gmInputs[playerIndex].mXY.y > 0 ? 1f : -1f;
        }
        dashDir.x *= 0.5f;
        dashDir *= dashForce;
        GetComponent<Rigidbody2D>().AddForce(dashDir, ForceMode2D.Impulse);

        PreGameTimer preGameTimer = FindObjectOfType<PreGameTimer>();
        //Debug.Log("Actor: Gorilla Dashed");
        if (preGameTimer != null)
        {
            preGameTimer.GetComponent<PreGameTimer>().gorillaSmashed = true;
            Debug.Log("Actor: gorillaSmash = true");
        }
    }
    public void ResetTimeScale()
    {
        Time.timeScale = 1;
        startSlowMo = false;
        canBeInSlowMotion = true;
        slowMoCount = 0;
    }
    public void CaughtBall(GameObject ball)
    {
        canCharge = false;
        haveBall = true;
        ballHolding = ball;
    }
}

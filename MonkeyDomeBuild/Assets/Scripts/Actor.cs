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
    public int inputIndex;
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
    public float chargePerSec = 15f;
    public float chargeThrowRequireCount = 5f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    protected Rigidbody2D cache_rb;
    protected Transform cache_tf;

    public Character characterType;
    public GameObject monkeyCrown;
    public BallInfo ballCanCatch;

    [SerializeField]
    protected GameObject PointerPivot;
    [SerializeField]
    protected GameObject PointerCenter;
    [SerializeField]
    protected GameObject pointerBase;

    protected Color col;
    protected Color col1;
    protected Color col2;

    public Transform catchCenter;
    public BoxCollider2D raycastCol;
    public bool gorillaSmashedObject;

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

    protected float holdCount = 0;
    protected float holdTime = 1f;
    protected float speedMultiplier = 1f;

    protected Vector2 storedThrowDir;
    protected float currentDir = 1f;

  

    //public string cType = "";

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


        col1 = PointerCenter.GetComponent<Image>().color;
        col2 = pointerBase.GetComponent<Image>().color;
    }

    void Update()
    {
        //JumpCheck();
        //cType = characterType.ToString();
        if (GameManager.Instance.gmInputs[inputIndex].mJump)
        {
            Jumping();
        }
        Aim();
        characterType.CHUpdate();
        CheckLeader();

        if (isinair && cache_rb.velocity.y < 0f)
        {
            if (isClimbing)
            {
                //animator.SetBool("IsIdle", true);
                animator.SetBool("IsInAirDown", false); 
                animator.SetBool("IsClimbing", true);
                animator.SetBool("IsInAir", false);
            }
            else
            {
                animator.SetBool("IsClimbing", false);
                animator.SetBool("IsInAirDown", true);
                animator.SetBool("IsInAir", false);
                animator.SetBool("IsJumping", false);
            }
        }
        else if (isinair && cache_rb.velocity.y >= 0f)
        {
            if (isClimbing)
            {
                animator.SetBool("IsInAirDown", false);
                animator.SetBool("IsInAir", false);
            }
            else
            {
                animator.SetBool("IsClimbing", false);
            }
            //animator.SetBool("IsStartJump", false);
        }
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
                    //tempParticle = ParticlesManager.Instance.JumpParticle;
                }
                if (tempParticle != null)
                {
                    tempParticle.SetActive(true);
                    tempParticle.transform.position = transform.position;
                }

            }
            cache_rb.AddForce(Vector2.up * characterType.jumpforce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
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
                {
                    cache_rb.gravityScale = 2;
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsIdle", true);
                }

                if (GameManager.Instance.gmInputs[inputIndex].mXY.y >= 0)
                {
                    cache_rb.velocity *= 0.3f;
                    cache_rb.AddForce(Vector2.up * characterType.jumpforce, ForceMode2D.Impulse);
                    animator.SetBool("IsJumping", true);
                    if (AudioEffectManager.Instance != null)
                    {
                        AudioEffectManager.Instance.PlayMonkeyJumpSE();
                    }
                }
            }
            else if (canClimb)
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
            if (!RayCastSide(GameManager.Instance.gmInputs[inputIndex].mXY.x))
            {
                if (characterType is Gorilla && characterType.manuallyCharging)
                {
                    movement.x = GameManager.Instance.gmInputs[inputIndex].mXY.x * (characterType.chargespeed + characterInc);
                }
                else
                {
                    movement.x = GameManager.Instance.gmInputs[inputIndex].mXY.x * (characterType.movespeed + characterInc);
                }
                if (isDashing)
                {
                    if (dashingCount >= dashingTime)
                    {
                        GetComponent<EffectControl>().EndDashEffect();
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
            movement.x = GameManager.Instance.gmInputs[inputIndex].mXY.x * (characterType.movespeed + characterInc);
            movement.y = GameManager.Instance.gmInputs[inputIndex].mXY.y * (characterType.movespeed + characterInc);
        }
        if (!beingSmack)
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
        if (Mathf.Abs(GameManager.Instance.gmInputs[playerIndex].mXY.x) >= 0.4f || Mathf.Abs(GameManager.Instance.gmInputs[playerIndex].mXY.y) >= 0.4f)
        {
            storedThrowDir = GameManager.Instance.gmInputs[playerIndex].mXY;
        }
        else
        {
            //if not aiming thow direction sprite is facing
                if (spriteRenderer.flipX == false)
                storedThrowDir = Vector3.right;
                else
            storedThrowDir = -Vector3.right;
                
                
            
        }
        if (GameManager.Instance.gmInputs[playerIndex].mXY.x >= 0.1f)
        {
            currentDir = 1f;
        }
        else if (GameManager.Instance.gmInputs[playerIndex].mXY.x <= -0.1f)
        {
            currentDir = -1f;
        }
        if (canCharge)
        {
            if ((GameManager.Instance.gmInputs[inputIndex].mChargeThrow && haveBall) && !cantHoldAnymore)
            {
                isCharging = true;
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
                    holdingCatchCount += chargePerSec * Time.deltaTime;
                }
                else
                {
                    holdingCatchCount = maxChargeCount;
                }
                if (holdCount < holdTime)
                {
                    holdCount += Time.deltaTime;
                }
                else
                {
                    holdCount = 0;
                    cantHoldAnymore = true;
                }
            }
            else
            {
                cantHoldAnymore = false;
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
                        if (ballHolding.GetComponent<BallInfo>().BallType == ThrowableType.Trophy)
                        {
                            ballHolding.GetComponent<TrophyInfo>().InvokeEnableCollider();
                        }
                        if (ballHolding.GetComponent<BallInfo>().BallType == ThrowableType.Coconut)
                        {
                            ballHolding.GetComponent<CoconutInfo>().ThrowCoconut();
                        }
                    }
                    ReleaseBall();
                    ballRigid.AddForce(storedThrowDir * tempThrowForce, ForceMode2D.Impulse);
                    stat_throw++;
                    holdingCatchCount = 0f;
                }
            }
        }
        else
        {
            if (GameManager.Instance.gmInputs[inputIndex].mCatchRelease)
            {
                canCharge = true;
            }
        }
    }
    public void ReleaseBall()
    {
        holdCount = 0;
        speedMultiplier = 1f;
        startSlowMo = false;
        haveBall = false;
        if (ballHolding != null)
        {
            ballHolding.GetComponent<BallInfo>().Reset();
        }
        ballCanCatch = null;
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
            //Debug.DrawLine(checkPos, checkPos + Vector2.up * direction);
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
            //Debug.DrawLine(checkPosStart, tempV);
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
           
            if (GameManager.Instance.gmInputs[inputIndex].mXY.x != 0)
            {
                col1.a = Mathf.Lerp(PointerCenter.GetComponentInChildren<Image>().color.a, 1, Time.unscaledDeltaTime * 5f);
                col2.a = Mathf.Lerp(pointerBase.GetComponentInChildren<Image>().color.a, 1, Time.unscaledDeltaTime * 5f);

                PointerCenter.GetComponentInChildren<Image>().color = col1;
                pointerBase.GetComponentInChildren<Image>().color = col2;
            }
            else
            {
                col1.a = Mathf.Lerp(PointerCenter.GetComponentInChildren<Image>().color.a, 0, Time.unscaledDeltaTime * 10f);
                col2.a = Mathf.Lerp(pointerBase.GetComponentInChildren<Image>().color.a, 0, Time.unscaledDeltaTime * 10f);

                PointerCenter.GetComponentInChildren<Image>().color = col1;
                pointerBase.GetComponentInChildren<Image>().color = col2;
            }

            

            //Debug.Log(a);
            var a = Mathf.Lerp(1, 4, holdingCatchCount / maxChargeCount);
            PointerCenter.transform.parent.GetComponent<RectTransform>().localScale = new Vector3(a, a, a);
            if (Mathf.Abs(GameManager.Instance.gmInputs[inputIndex].mXY.x) >= 0.4f || Mathf.Abs(GameManager.Instance.gmInputs[inputIndex].mXY.y) >= 0.4f)
            {
                Vector3 dir = new Vector3(GameManager.Instance.gmInputs[inputIndex].mXY.x, GameManager.Instance.gmInputs[inputIndex].mXY.y, 0);
                Quaternion targetAng = Quaternion.FromToRotation(Vector3.right, dir);
                if (targetAng.eulerAngles.y == 180f)
                {
                    PointerPivot.transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(PointerPivot.transform.localEulerAngles.z, targetAng.eulerAngles.y, 20 * Time.unscaledDeltaTime));
                }
                else
                {
                    PointerPivot.transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(PointerPivot.transform.localEulerAngles.z, targetAng.eulerAngles.z, 20 * Time.unscaledDeltaTime));
                }
            }  
        }
        else
        {
            //shotPointer.GetComponentsInChildren<Image>().color.a;
            col1.a = Mathf.Lerp(PointerCenter.GetComponentInChildren<Image>().color.a, 0, Time.unscaledDeltaTime * 10f);
            col2.a = Mathf.Lerp(pointerBase.GetComponentInChildren<Image>().color.a, 0, Time.unscaledDeltaTime * 10f);

            PointerCenter.GetComponentInChildren<Image>().color = col1;
            pointerBase.GetComponentInChildren<Image>().color = col2;

            PointerCenter.transform.parent.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
    public bool IsHoldingBall
    {
        get
        {
            return haveBall;
        }
    }
    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (isDashing)
            {
                dashingCount = 0;
                isDashing = false;
                GetComponent<EffectControl>().EndDashEffect();
                for (int i = 0; i < GameManager.Instance.TotalNumberofPlayers; ++i)
                {
                    if (GameManager.Instance.gmPlayers[i] != null)
                    {
                        Character p = GameManager.Instance.gmPlayers[i].GetComponent<Actor>().characterType;
                        if (p is Monkey)
                        {
                            //knock both player off vine for now
                            if (GameManager.Instance.gmPlayers[i].GetComponent<Actor>().isClimbing ||
                                !GameManager.Instance.gmPlayers[i].GetComponent<Actor>().IsInAir)
                            {
                                GameManager.Instance.gmPlayers[i].GetComponent<Actor>().isClimbing = false;
                                GameManager.Instance.gmPlayers[i].GetComponent<Rigidbody2D>().isKinematic = false;
                                GameManager.Instance.gmPlayers[i].GetComponent<Actor>().TempDisableInput(disableInputTime * 2);
                                if (GameManager.Instance.gmPlayers[i].GetComponent<Actor>().IsHoldingBall)
                                {
                                    GameManager.Instance.gmPlayers[i].GetComponent<Actor>().ReleaseBall();
                                }
                            }
                        }
                    }
                }
                FindObjectOfType<CameraController>().ScreenShake();
                AudioEffectManager.Instance.PlayAudienceSmash();
                
                PreGameTimer preGameTimer = FindObjectOfType<PreGameTimer>();
                if (preGameTimer != null)
                {
                    preGameTimer.GetComponent<PreGameTimer>().gorillaSmashed = true;
                }
            }
        }
        if (characterType is Gorilla)
        {
            if (other.collider.CompareTag("Player"))
            {
                if (other.collider.GetComponent<Actor>().characterType is Monkey)
                {
                    KnockOffMonkey(other.collider.gameObject);

                    if (isDashing)
                    {
                        PreGameTimer preGameTimer = FindObjectOfType<PreGameTimer>();
                        if (preGameTimer != null)
                        {
                            preGameTimer.GetComponent<PreGameTimer>().gorillaSmashed = true;
                        }
                    }
                }
            }
        }
    }

    protected void OnCollisionStay2D(Collision2D other)
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
        monkey.GetComponent<Actor>().TempDisableInput(disableInputTime);
        if (monkey.GetComponent<Actor>().IsHoldingBall)
        {
            monkey.GetComponent<Actor>().ReleaseBall();
        }
        Vector2 dir = -1 * (transform.position - monkey.transform.position).normalized;
        if (!isinair)
        {
            dir.y = 0.3f;
        }
        else if (isinair)
        {
            if (!monkey.GetComponent<Actor>().IsInAir)
            {
                dir.y = 1f;
            }
        }
        monkey.GetComponent<Rigidbody2D>().AddForce(dir * smackImpulse, ForceMode2D.Impulse);
    }

    public void TempDisableInput(float time)
    {
        GetComponent<EffectControl>().PlayStunEffect();
        DisableInput = true;
        InvokeEnableInput(time);
    }

    public void InvokeEnableInput(float time)
    {
        CancelInvoke("ResetBeingSmack");
        Invoke("ResetBeingSmack", time);
    }

    protected void ResetBeingSmack()
    {
        DisableInput = false;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BallTrigger"))
        {
            if (isDashing)
            {
                GameManager.Instance.gmInputs[inputIndex].mCatch = true;
                if (isPlayer == true)
                {
                    isPlayer = false;
                    StartCoroutine(RealisticDashCatch());
                }
            }
            //Debug.Log("touching ball;");

        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            if (!justJump && isinair)
            {
                animator.SetBool("IsInAirDown", false);
                isinair = false;
                animator.SetBool("IsLanding", true);
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
            if (other.GetComponentInParent<BallInfo>() is CoconutInfo)
            {
                if (!other.GetComponentInParent<CoconutInfo>().IsThrown)
                {
                    ballCanCatch = other.gameObject.GetComponentInParent<BallInfo>();
                    if (!ballInRange)
                    {
                        ballInRange = true;
                    }
                }
            }
            else
            {
                ballCanCatch = other.gameObject.GetComponentInParent<BallInfo>();
                if (!ballInRange)
                {
                    ballInRange = true;
                }
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
                //if (GameManager.Instance.gmAudienceManager.GetEventActive())
                //{
                //    if(GameManager.Instance.gmAudienceManager.GetCurrentEvent() == AudienceManager.AudienceEvent.Bananas)
                //    {
                //        GameManager.Instance.gmAudienceManager.BananaCaught(playerIndex);
                //    }
                //}
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
                GameManager.Instance.gmTrophyManager.BeingHitByPoop(playerIndex);
                //TODO add poop event logic
                //Audience opinion increase when hit by poop
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
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

    protected void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }

    public void ReactionToBanana(float incAmount)
    {
        if (characterInc < maxminInc)
        {
            IncrementCharacterInc(incAmount);
        }
    }

    public void ReactionToPoop(float incAmount)
    {
        if (Mathf.Abs(characterInc) > -maxminInc)
        {
            IncrementCharacterInc(-incAmount);
        }
    }

    protected void IncrementCharacterInc(float inc)
    {
        characterInc += inc;
    }

    protected void CheckLeader()
    {
        bool hasHighestScore = false;
        int highestScore = 0;

        /*
        if (GameManager.Instance.gmScoringManager.p1Score == GameManager.Instance.gmScoringManager.p2Score && GameManager.Instance.gmScoringManager.p2Score == GameManager.Instance.gmScoringManager.p3Score)
        {
            monkeyCrown.SetActive(false);
        }
        else if (
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
        }*/

        if (GameManager.Instance.gmScoringManager.p1Score >= highestScore) { highestScore = GameManager.Instance.gmScoringManager.p1Score; }
        if (GameManager.Instance.gmScoringManager.p2Score >= highestScore) { highestScore = GameManager.Instance.gmScoringManager.p2Score; }
        if (GameManager.Instance.gmScoringManager.p3Score >= highestScore) { highestScore = GameManager.Instance.gmScoringManager.p3Score; }
        if (GameManager.Instance.gmScoringManager.p4Score >= highestScore) { highestScore = GameManager.Instance.gmScoringManager.p4Score; }
        if (GameManager.Instance.gmScoringManager.p5Score >= highestScore) { highestScore = GameManager.Instance.gmScoringManager.p5Score; }

        if (playerIndex == 0 && GameManager.Instance.gmScoringManager.p1Score == highestScore && highestScore != 0) { hasHighestScore = true; }
        else if (playerIndex == 1 && GameManager.Instance.gmScoringManager.p2Score == highestScore && highestScore != 0) { hasHighestScore = true; }
        else if (playerIndex == 2 && GameManager.Instance.gmScoringManager.p3Score == highestScore && highestScore != 0) { hasHighestScore = true; }
        else if (playerIndex == 3 && GameManager.Instance.gmScoringManager.p4Score == highestScore && highestScore != 0) { hasHighestScore = true; }
        else if (playerIndex == 4 && GameManager.Instance.gmScoringManager.p5Score == highestScore && highestScore != 0) { hasHighestScore = true; }
        else { hasHighestScore = false; }

        if (hasHighestScore && !monkeyCrown.activeSelf)
        {
            monkeyCrown.SetActive(true);
        }
        else if (!hasHighestScore && monkeyCrown.activeSelf)
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
        float tempSpeed = Mathf.Abs(cache_rb.velocity.x) > Mathf.Abs(cache_rb.velocity.y) ? Mathf.Abs(cache_rb.velocity.x) : Mathf.Abs(cache_rb.velocity.y);
        if (characterType is Monkey)
        {
            tempSpeed /= GameManager.Instance.gmMovementManager.mSpeed;
        } else
        {
            tempSpeed /= GameManager.Instance.gmMovementManager.gSpeed;
        }
        if (tempSpeed > 1f)
        {
            tempSpeed = 1f;
        }
        if(tempSpeed < .3f)
        {
            tempSpeed = .3f;
        }

        animator.SetFloat("velocity", tempSpeed);
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.x);
        if (cache_rb.velocity.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (cache_rb.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        if (Mathf.Abs(cache_rb.velocity.x) > 1f || GameManager.Instance.gmInputs[inputIndex].mXY.x != 0)
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsIdle", false);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", true);
        }
        animator.SetBool("IsThrowing", isCharging);
    }
    public void GorillaDash()
    {
        isDashing = true;
        Vector2 dashDir = Vector2.zero;
        GetComponent<EffectControl>().PlayDashEffect();
        dashDir.y = 1f;
        if (Mathf.Abs(GameManager.Instance.gmInputs[inputIndex].mXY.x) > 0)
        {
            dashDir.x = GameManager.Instance.gmInputs[inputIndex].mXY.x > 0 ? 1f : -1f;
            //dashDir.x = GameManager.Instance.gmInputs[whichplayer].mXY.x > 0 ? 0.5f : -1.2f;
        }
        if (Mathf.Abs(GameManager.Instance.gmInputs[inputIndex].mXY.y) > 0)
        {
            dashDir.y = GameManager.Instance.gmInputs[inputIndex].mXY.y > 0 ? 1f : -1f;
        }
        dashDir.x *= 0.5f;
        dashDir *= dashForce;
        GetComponent<Rigidbody2D>().AddForce(dashDir, ForceMode2D.Impulse);

        PreGameTimer preGameTimer = FindObjectOfType<PreGameTimer>();
        if (preGameTimer != null)
        {
            preGameTimer.GetComponent<PreGameTimer>().gorillaSmashes++;
        }
    }
    public void ResetTimeScale()
    {
        if (GameManager.Instance.gmPauseManager.isGamePaused == false)
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

    public void GorillaCatchReset()
    {

    }

    public bool IsChargingThrow = false;
    public void SwitchRoomReset()
    {
        ReleaseBall();
        canClimb = false;
        isClimbing = false;
        if (cache_rb.gravityScale == 0)
        {
            cache_rb.gravityScale = 2;
        }
    }

    IEnumerator RealisticDashCatch()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.gmInputs[inputIndex].mCatch = false;
        isPlayer = true;
    }
}

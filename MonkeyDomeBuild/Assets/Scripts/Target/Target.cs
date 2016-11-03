using UnityEngine;
using System.Collections;
public enum TargetAxis
{
    OnGround = 1,
    OnRightSide = 2,
    OnLeftSide = -2,
    OnTop = -1
}
public enum TargetType
{
    Static,
    Moving
}
public class Target : MonoBehaviour
{
    public bool isHit;
    //private int targetTier;
    public Transform targetTransform;
    private TargetManager targetManager;
    public bool targetActive = false;
    public GameObject targetHeadL;
    public GameObject targetHeadM;
    public GameObject targetHeadS;
    public GameObject targetHeadT;

    private GameObject targetHead;

    public Transform hitParticlePivot;

    private Vector3 targetRot;
    private bool canEnableTargetHeadCollider = false;

    public float resetTime = 1;
    public float lifeTime;
    private float initialLifeTime;
    public bool inAlarm = false;

    public TargetAxis targetAxis = TargetAxis.OnGround;
    public TargetType targetType = TargetType.Static;

    public Vector3 moveLoc = Vector3.zero;
    private Vector3 startLoc;
    private Vector3 targetPos;
    float maxDis = 2f;
    float moveSpeed = 2f;

    public float waitTime;
    float prepTime = 5f;
    float warningTime = 2f;


    TargetBase panel;

    void Awake()
    {
        startLoc = transform.position;
        targetPos = moveLoc;
        DisableCollider();
        //myCollider = GetComponentInChildren<Collider2D>();
        targetManager = FindObjectOfType<TargetManager>();
        //targetChild = transform.parent.FindChild("Target").gameObject;
        targetHead = targetHeadL;
        isHit = true;
        targetRot = Vector3.zero;
        Init();

    }
    public TargetBase SetTargetBase
    {
        set
        {
            panel = value;
            panel.SetTarget = this;
        }
    }
    void Init()
    {
        switch (targetAxis)
        {
            case TargetAxis.OnGround:
                targetRot = new Vector3(90f, 0, 0);
                break;
            case TargetAxis.OnLeftSide:
                targetRot = new Vector3(0, 270f, 270f);
                break;
            case TargetAxis.OnRightSide:
                targetRot = new Vector3(0, 90f, 90);
                break;
            case TargetAxis.OnTop:
                targetRot = new Vector3(270f, 0, 180f);
                break;
        }
        targetTransform.localEulerAngles = targetRot;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            targetActive = true;
            TargetSetter();
        }
        if (!inAlarm)
        {
            TargetTime();
        }
        if (targetActive)
        {
            UpdateTargetTime();
        }
        if (targetManager.RallyOn)
        {
            if (waitTime != 0)
            {
                if (waitTime <= prepTime)
                {
                    if (!targetActive)
                    {
                        panel.ChangeTargetState(TargetBaseState.Prep);
                    }
                    waitTime = 0;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }

            }
        }
        if (Vector3.Distance(targetTransform.localEulerAngles, targetRot) > 0.01f)
        {
            targetTransform.localRotation = Quaternion.LerpUnclamped(targetTransform.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * 10f);
        }
        if (Vector3.Distance(targetTransform.localEulerAngles, targetRot) > 0.2f)
        {
            if (canEnableTargetHeadCollider)
            {
                canEnableTargetHeadCollider = false;
                targetHead.GetComponent<CircleCollider2D>().enabled = true;
            }
        }
        if (targetType == TargetType.Moving)
        {
            if (targetPos != Vector3.zero)
            {
                Vector3 tempTargetPos = targetPos;
                if (Mathf.Abs(tempTargetPos.x - transform.position.x) > maxDis)
                {
                    float dir = tempTargetPos.x - transform.position.x > 0 ? 1f : -1f;
                    tempTargetPos.x = transform.position.x + maxDis * dir;
                }
                if (Mathf.Abs(tempTargetPos.y - transform.position.y) > maxDis)
                {
                    float dir = tempTargetPos.y - transform.position.y > 0 ? 1f : -1f;
                    tempTargetPos.y = transform.position.y + maxDis * dir;
                }
                if (Vector3.Distance(transform.position, tempTargetPos) < 0.5f)
                {
                    if (targetPos == moveLoc)
                    {
                        targetPos = startLoc;
                    }
                    else
                    {
                        targetPos = moveLoc;
                    }
                }
                else
                {
                    transform.position = Vector3.LerpUnclamped(transform.position, tempTargetPos, Time.deltaTime * moveSpeed);
                }
            }
        }


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            if (other.GetComponent<BallInfo>().IsBall)
            {
                //TargetSetter(-1);
                if (!isHit)
                {
                    panel.ChangeTargetState(TargetBaseState.Hit);
                    GameManager.Instance.gmScoringManager.HitTargetScore(other.GetComponentInParent<BallInfo>());
                    GameObject particle = ParticlesManager.Instance.TargetHitParticle;
                    particle.SetActive(true);
                    particle.transform.position = hitParticlePivot.position;
                    Vector3 ballPos = other.transform.position;
                    ballPos.z = 0;
                    Vector3 pivotPos = hitParticlePivot.position;
                    pivotPos.z = 0;
                    Quaternion targetAng = Quaternion.FromToRotation(Vector3.right, (ballPos - pivotPos).normalized);
                    particle.transform.rotation = Quaternion.Euler(0, 0, targetAng.eulerAngles.z);
                    particle.GetComponentInChildren<ParticleSystem>().Play();
                    //hitParticlePivot.localEulerAngles = new Vector3(0, 0, targetAng.eulerAngles.z);

                    //hitParticlePivot.GetComponentInChildren<ParticleSystem>().Play();
                    Reset();
                    DisableCollider();
                    // isHit = true;
                    inAlarm = false;
                    targetManager.targetsHitInSequence[targetManager.sequenceIndex] = true;
                    targetManager.sequenceIndex++;
                    targetManager.TargetHit();
                    if (other.GetComponent<BallInfo>().lastThrowMonkey != null)
                    {
                        GameManager.Instance.gmTrophyManager.TargetsHit(other.GetComponent<BallInfo>().lastThrowMonkey.GetComponent<Actor>().playerIndex);
                    }
                    //targetManager.advanceTier = targetManager.CheckRally();
                    ResetTarget();
                }

            }
        }
    }

    public void ResetTarget()
    {
        if (!isHit)
        {
            isHit = true;
            if (targetManager != null)
            {
                targetManager.ActiveTargets -= 1;
            }
        }
    }

    public void DisableCollider()
    {
        //GetComponentInChildren<Collider2D>().enabled = false;
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }
    }
    public void SetTargetHeads(int targetTier)
    {
        switch (targetTier)
        {
            case 0:
                if (targetHead != null)
                    targetHead.SetActive(false);
                targetHead = targetHeadL;
                targetHead.SetActive(true);
                //targetHead.GetComponent<Collider2D>().enabled = true;
                isHit = false;
                break;
            case 1:
                if (targetHead != null)
                    targetHead.SetActive(false);
                targetHead = targetHeadM;
                targetHead.SetActive(true);
                isHit = false;
                break;
            case 2:
                if (targetHead != null)
                    targetHead.SetActive(false);
                targetHead = targetHeadS;
                targetHead.SetActive(true);
                isHit = false;
                break;
            case 3:
                if (targetHead != null)
                    targetHead.SetActive(false);
                targetHead = targetHeadT;
                targetHead.SetActive(true);
                isHit = false;
                break;
            default:
                goto case 0;
        }
        canEnableTargetHeadCollider = true;
        //targetHead.GetComponent<CircleCollider2D>().enabled = true;
    }
    public void TargetSetter()
    {
        // set target axis in editor for each target
        targetRot = Vector3.zero;
        switch (targetAxis)
        {
            case TargetAxis.OnGround:
                targetRot = new Vector3(0, 0, 0);
                break;
            case TargetAxis.OnLeftSide:
                targetRot = new Vector3(0, 0, 270f);
                break;
            case TargetAxis.OnRightSide:
                targetRot = new Vector3(0, 0, 90);
                break;
            case TargetAxis.OnTop:
                targetRot = new Vector3(0, 0, 180f);
                break;
        }
        //transform.localEulerAngles = targetRot;
        if (targetManager != null)
        {
            SetTargetHeads(targetManager.TargetTier);
        }
        else
        {
            SetTargetHeads(1);
        }

    }

    public void TargetTime()
    {
        // starts lifeTime alarm
        if (!isHit)
        {
            if (targetManager != null)
            {
                lifeTime = targetManager.SetLifeTime();
            }
            else
            {
                lifeTime = 5f;
            }
            initialLifeTime = lifeTime;
            inAlarm = true;
        }
    }

    void UpdateTargetTime()
    {
        if (inAlarm)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime < warningTime)
            {
                panel.ChangeTargetState(TargetBaseState.Warning);
            }
            else
            {
                Popup();
            }
            if (lifeTime <= 0)
            {
                // deactive alarm, reset lifeTime
                //TargetSetter(-1f);
                panel.ChangeTargetState(TargetBaseState.Miss);
                Reset();
                if (targetManager != null)
                {
                    lifeTime = targetManager.SetLifeTime();
                }
                else
                {
                    lifeTime = 5f;
                }
                initialLifeTime = lifeTime;
                inAlarm = false;
                targetActive = false;
                DisableCollider();
                ResetTarget();
            }
        }
    }
    void Reset()
    {
        switch (targetAxis)
        {
            case TargetAxis.OnGround:
                targetRot = new Vector3(90f, 0, 0);
                break;
            case TargetAxis.OnLeftSide:
                targetRot = new Vector3(0, 270f, 270f);
                break;
            case TargetAxis.OnRightSide:
                targetRot = new Vector3(0, 90f, 90f);
                break;
            case TargetAxis.OnTop:
                targetRot = new Vector3(270f, 0, 180f);
                break;
        }
        canEnableTargetHeadCollider = false;
        //transform.localEulerAngles = targetRot;
    }
    public void MoveTargets()
    {

    }
    public TargetAxis SetTargetAxis
    {
        set
        {
            targetAxis = value;
        }
    }
    public TargetType SetTargetType
    {
        set
        {
            targetType = value;
        }
    }
    public Vector3 MoveLocation
    {
        set
        {
            moveLoc = value;
        }
    }
    public float WaitTime
    {
        get
        {
            return waitTime;
        }
        set
        {
            waitTime = value;
        }
    }
    public void Popup()
    {
        panel.ChangeTargetState(TargetBaseState.Pop);
    }
    public void RallyStart()
    {
        panel.ChangeTargetState(TargetBaseState.RallyStart);
    }
    public void RallyEnd()
    {
        panel.ChangeTargetState(TargetBaseState.RallyEnd);
    }
    public float InitialLifeTime
    {
        get
        {
            return initialLifeTime;
        }
    }
    public float PrepTime
    {
        get
        {
            return prepTime;
        }
    }
    public float WarningTime
    {
        get
        {
            return warningTime;
        }
    }
}

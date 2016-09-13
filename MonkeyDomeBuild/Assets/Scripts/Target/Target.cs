using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    public bool isHit;
    //private int targetTier;
    private FullTargetRotator targetActivator;
    private GameObject targetParent;
    private GameObject targetChild;
    private Target gameTarget;
    private bool stayTier;
    private TargetManager targetManager;
    public bool targetActive = false;

    private GameObject targetHead;
    private Collider2D myCollider;

    public float resetTime = 1;
    public float lifeTime;
    public bool inAlarm = false;
    public enum TargetAxis
    {
        OnGround = 1,
        OnRightSide = 2,
        OnLeftSide = -2,
        OnTop = -1
    }
    public TargetAxis targetAxis = TargetAxis.OnGround;

    void Start()
    {
        DisableCollider();
        myCollider = GetComponentInChildren<Collider2D>();
        targetManager = FindObjectOfType<TargetManager>();
        targetParent = transform.parent.gameObject;
        targetChild = transform.parent.FindChild("Target").gameObject;
        targetHead = transform.FindChild("Large").gameObject;
        isHit = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!inAlarm)
        {
            TargetTime();
        }
        if (targetActive)
        {
            UpdateTargetTime();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BallTrigger"))
        {
            if (other.GetComponentInParent<BallInfo>().IsBall)
            {
                TargetSetter(-1);
                DisableCollider();
                // isHit = true;
                inAlarm = false;
                targetManager.targetsHitInSequence[targetManager.sequenceIndex] = true;
                targetManager.sequenceIndex++;
                targetManager.hitSum++;
                //targetManager.advanceTier = targetManager.CheckRally();
                if (!isHit)
                {
                    GameManager.Instance.gmScoringManager.HitTargetScore(other.GetComponentInParent<BallInfo>());
                }
                ResetTarget();
            }
        }
    }

	public void ResetTarget(){
		if (!isHit) {
			isHit = true;
			targetManager.ActiveTargets -= 1;
		}
	}

    public void DisableCollider()
    {
        GetComponentInChildren<Collider2D>().enabled = false;
    }

    public void EnableCollider()
    {
        GetComponentInChildren<Collider2D>().enabled = true;
    }

    public void SetTargetHeads(int targetTier)
    {
		if (myCollider != null) 
		{
			myCollider.enabled = true;
		}

		if (targetHead != null) 
		{
			//targetHead.SetActive(false);
		}

        switch (targetTier)
        {
            case 0:
			if(targetHead!=null)
				targetHead.SetActive(false);
                targetHead = transform.FindChild("Large").gameObject;
                targetHead.SetActive(true);
                isHit = false;
                break;
            case 1:
			if(targetHead!=null)
				targetHead.SetActive(false);
                targetHead = transform.FindChild("Medium").gameObject;
                targetHead.SetActive(true);
                isHit = false;
                break;
            case 2:
			if(targetHead!=null)
				targetHead.SetActive(false);
                targetHead = transform.FindChild("Small").gameObject;
                targetHead.SetActive(true);
                isHit = false;
                break;
            case 3:
			if(targetHead!=null)
				targetHead.SetActive(false);
                targetHead = transform.FindChild("Tiny").gameObject;
                targetHead.SetActive(true);
                isHit = false;
                break;
            default:
			if(targetHead!=null)
				targetHead.SetActive(false);
                targetHead = transform.FindChild("Large").gameObject;
                targetHead.SetActive(true);
                isHit = false;
                break;
        }
        targetHead.GetComponent<TargetHead>().myCollider.enabled = true;
    }


    public void TargetSetter(float rotDir)
    {
        // to active target, use TargetSetter(1), to deactivate target, use TargetSetter(-1)
        // set target axis in editor for each target
        Vector3 rotAt = Vector3.zero;
        switch (targetAxis)
        {
            case TargetAxis.OnGround:
                rotAt = Vector3.right;
                break;
            case TargetAxis.OnLeftSide:
                rotAt = Vector3.down;
                break;
            case TargetAxis.OnRightSide:
                rotAt = Vector3.up;
                break;
            case TargetAxis.OnTop:
                rotAt = Vector3.left;
                break;
        }
        targetParent.transform.RotateAround(targetChild.transform.position, rotAt, -90f * rotDir);
    }

    public void TargetTime()
    {
        // starts lifeTime alarm
        if (isHit == false)
        {
            lifeTime = targetManager.SetLifeTime();
            inAlarm = true;
        }
    }

    void UpdateTargetTime()
    {
        if (inAlarm)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                // deactive alarm, reset lifeTime
                TargetSetter(-1f);
                stayTier = false;
                lifeTime = targetManager.SetLifeTime();
                inAlarm = false;
                targetActive = false;
                DisableCollider();
				ResetTarget();
            }
        }
    }

    public void MoveTargets()
    {

    }
}

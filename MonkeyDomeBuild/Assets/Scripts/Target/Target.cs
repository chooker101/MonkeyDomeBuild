using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {
    private bool isHit;
    //private int targetTier;
    private FullTargetRotator targetActivator;
    private GameObject targetParent;
    private GameObject targetChild;
    private Target gameTarget;
    private bool stayTier;
    private TargetManager targetManager;
    public bool targetActive = false;

    private GameObject targetHead;

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

    // Use this for initialization
    void Start () {
        //activateCounter = 0;
        targetManager = FindObjectOfType<TargetManager>();
        targetParent = transform.parent.gameObject;
       // targetActivator = GetComponent<FullTargetRotator>();
        targetChild = transform.parent.FindChild("Target").gameObject;

        targetHead = transform.FindChild("Large").gameObject;

        //targetTier = 0;

        isHit = false;

    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    ActivateTarget();
        //}
        if (!inAlarm)
        {
            TargetTime();
            
        }
        if (targetActive)
        {
            UpdateTargetTime();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            isHit = true;
            //ScoringManager.targetsHit++;
            TargetSetter(-1);
        }
    }


    public void SetTargetHeads(int targetTier)
    {
        // apply stats
        switch (targetTier)
        {
            case 0:
                targetHead.SetActive(false);
                targetHead = transform.FindChild("Large").gameObject;
                targetHead.SetActive(true);
                break;
            case 1:
                targetHead.SetActive(false);
                targetHead = transform.FindChild("Medium").gameObject;
                targetHead.SetActive(true);
                break;
            case 2:
                targetHead.SetActive(false);
                targetHead = transform.FindChild("Small").gameObject;
                targetHead.SetActive(true);
                break;
            case 3:
                targetHead.SetActive(false);
                targetHead = transform.FindChild("Tiny").gameObject;
                targetHead.SetActive(true);
                break;
            default:
                targetHead.SetActive(false);
                targetHead = transform.FindChild("Large").gameObject;
                targetHead.SetActive(true);
                break;
        }
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
        Debug.Log(gameObject.name);
    }

    public void TargetTime()
    {
        // starts lifeTime alarm

        lifeTime = targetManager.SetLifeTime();
        inAlarm = true;
        
       
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

            }
        }
    }

    public void MoveTargets()
    {

    }
}

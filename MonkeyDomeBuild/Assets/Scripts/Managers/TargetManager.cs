using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class TargetManager : MonoBehaviour {

    public float resetTime = 1;
    public float lifeTime;
    public bool inAlarm = false;


    private BallInfo ballInfo;
    private int addScore;

    private bool isHit;
    private int targetTier;
    private bool[] targetsHitInSequence = new bool[5];
    private bool advanceTier;
    private bool stayTier;

    private GameObject[] largeTargets;
    private FullTargetRotator targetActivator;
    private GameObject targetParent;
    private GameObject targetChild;

    public enum TargetAxis
    {
        OnGround=1,
        OnRightSide=2,
        OnLeftSide=-2,
        OnTop=-1
    }
    public TargetAxis targetAxis = TargetAxis.OnGround;

    // Use this for initialization
    void Start () {
        targetParent = transform.parent.gameObject;
        targetActivator = GetComponent<FullTargetRotator>();
        targetChild = transform.parent.FindChild("Target").gameObject;
        targetTier = 0;

        isHit = false;

        largeTargets = GameObject.FindGameObjectsWithTag("Large");

        ballInfo = GetComponent<BallInfo>();

        for (int i = 0; i < targetsHitInSequence.Length; i++)
        {
            targetsHitInSequence[i] = false;
        }
	}
	
	// Update is called once per frame
	void Update () {


        Debug.Log(targetParent.gameObject.name);


        if (Input.GetKeyDown(KeyCode.P))
        {
            TargetSetter(1f);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            TargetSetter(-1f);
        }

        advanceTier = CheckRally();
    }
    void OnTriggerEnter(Collider other)
    {
        // to go in TargetScript
        if (other.CompareTag("Ball"))
        {
            isHit = true;
            ScoringManager.targetsHit++;
        }
    }
    void TargetSetter(float rotDir)
    {
        // to go in TargetScript

        // to active target, use TargetSetter(1), to deactivate target, use TargetSetter(2)
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


    void RallySetter()
    {
        // this method is used to set the lifeTime attribute of targets based on the current targetTier
        for (int i = 0;i < targetsHitInSequence.Length; i++)
        {
            targetsHitInSequence[i] = false;
        }
        switch (targetTier)
        {
            case  0:
                lifeTime = 14f;
                break;
            case  1:
                lifeTime = 10f;
                break;
            case 2:
                lifeTime = 8f;
                break;
            case  3:
                lifeTime = 6f;
                break;

        }
    }
    void StartRally()
    {
        // call this method at the start of each rally. will activate target and deactivate if hit
        isHit = false;
        TargetSetter(1);
        TargetTime();
        if (isHit == true)
        {
            //TargetSetter(-1); do this with reference to target
        }

    }

    void TargetTime()
    {
        // to go in TargetScript

        // starts lifeTime alarm
        inAlarm = true;
        lifeTime -= 1 * Time.deltaTime;
        if (lifeTime <= 0)
        {
            inAlarm = false;
            stayTier = false;
        }
    }

    bool CheckRally()
    {
        // checks if enough targets in one rally are hit to upgrade tier
        int hitSum = 0;
        foreach(bool b in targetsHitInSequence)
        {
            if (b)
            {
                hitSum++;
            } 
            if (hitSum >= 3)
            {
                return true;
            }
            }
        if (hitSum > 0 && hitSum < 3)
        {
            stayTier = true;
        }
        return false;
    }
    void UpdateTierStatus()
    {
        // updates tier status at end of a rally

        if (advanceTier)
        {
            if (targetTier <= 4)
            {
                targetTier++;
                UpgradeTargets();
            }
        }
        else if (!stayTier)
        {
            // downgrade
        }
        ResetTargets();
    }

    void ResetTargets()
    {
        // put them back where they started
        TargetSetter(-1);
    }

    void UpgradeTargets()
    {
        // apply stats
    }

    void DowngradeTargets()
    {
        // apply stats
    }

    void MoveTargets()
    {
        // to go in TargetScript
    }

    void Rally()
    {
        RallySetter();
        StartRally();
        TargetTime();
        UpdateTierStatus();
    }

}

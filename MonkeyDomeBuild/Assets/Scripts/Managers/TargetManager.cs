using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class TargetManager : MonoBehaviour {


    private BallInfo ballInfo;
    private int addScore;

    private bool isHit;
    private int targetTier;
    private bool[] targetsHitInSequence = new bool[5];
    private bool advanceTier;
    private bool stayTier;

    //private GameObject[] largeTargets;
    private GameObject targetParent;
    private GameObject targetHead;
    private Target gameTargets;
    private float startLifeTime;

    // Use this for initialization
    void Start () {

        gameTargets = FindObjectOfType<Target>();

        
        targetParent = transform.parent.gameObject;
        targetTier = 0;
        targetHead = transform.FindChild("Large").gameObject;
        isHit = false;

        ballInfo = GetComponent<BallInfo>();

        for (int i = 0; i < targetsHitInSequence.Length; i++)
        {
            targetsHitInSequence[i] = false;
        }
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(targetHead.name);
        //Debug.Log(targetTier);
        //Debug.Log(targetParent.gameObject.name);


        if (Input.GetKeyDown(KeyCode.P))
        {
            gameTargets.TargetSetter(1f);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            gameTargets.TargetSetter(-1f);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            targetTier = Random.Range(0, 4);
            SetTargetHeads();
        }

        //advanceTier = CheckRally();

        //Rally();
    }


    void RallySetter()
    {
        // this method is used to clear the targetsHitInSequence array
        for (int i = 0;i < targetsHitInSequence.Length; i++)
        {
            targetsHitInSequence[i] = false;
        }
        
    }

    public float SetLifeTime()
    {
        switch (targetTier)
        {
            case 0:
                startLifeTime = 14f;
                break;
            case 1:
                startLifeTime = 10f;
                break;
            case 2:
                startLifeTime = 8f;
                break;
            case 3:
                startLifeTime = 6f;
                break;

        }
        return startLifeTime;
    }

    void StartRally()
    {
        // call this method at the start of each rally. will activate target and deactivate if hit
        isHit = false;
        gameTargets.TargetSetter(1);
        //gameTargets.TargetTime();
        if (isHit == true)
        {
            //TargetSetter(-1); do this with reference to target
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
            if (targetTier < 4)
            {
                targetTier++;
                SetTargetHeads();
            }
        }
        else if (!stayTier)
        {
            SetTargetHeads();
        }
        ResetTargets();
    }

    void ResetTargets()
    {
        // put them back where they started
        gameTargets.TargetSetter(-1);
    }

    void SetTargetHeads()
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

    void DowngradeTargets()
    {
        // apply stats
    }


    void Rally()
    {
        RallySetter();
        StartRally();
        UpdateTierStatus();
    }

}

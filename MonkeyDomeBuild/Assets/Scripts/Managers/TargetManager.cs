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
    private bool rallyOn;
    private int[] activateTimes = new int[5] { 0, 3, 6, 8, 10 };
    private int activateCounter;

    private Target[] gameTargets;
    private float startLifeTime;

    // Use this for initialization
    void Start () {
        activateCounter = 0;
        rallyOn = false;

        gameTargets = FindObjectsOfType<Target>();

        targetTier = 0;

        isHit = false;

        ballInfo = GetComponent<BallInfo>();

        for (int i = 0; i < targetsHitInSequence.Length; i++)
        {
            targetsHitInSequence[i] = false;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.J))
        {
            targetTier = Random.Range(0, 4);
            foreach(Target t in gameTargets)
            {
                t.SetTargetHeads(targetTier);
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M");
            advanceTier = CheckRally();
            Rally();
        }

    }


    void RallySetter()
    {
        Debug.Log("rally setter, clear targetshitinsequence");
        // this method is used to clear the targetsHitInSequence array
        for (int i = 0;i < targetsHitInSequence.Length; i++)
        {
            targetsHitInSequence[i] = false;
        }
        
    }

    public float SetLifeTime()
        // needed by Target script, don't merge into SetTargetHeads
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
        Debug.Log("start rally, raise tagets");
        // call this method at the start of each rally. will activate target and deactivate if hit
        isHit = false;
        ActivateTarget();
        foreach (Target t in gameTargets)
        {
            // make a sequence of targets, not all at the same time!
            if (isHit == true)
            {
                t.TargetSetter(-1f);
                Debug.Log("isHit true, lower targets");
            }
        }

    }

    bool CheckRally()
    {
        Debug.Log("Check Rally");
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
                Debug.Log("Hitsum: " + hitSum);
                return true;
            }
            }
        if (hitSum > 0 && hitSum < 3)
        {
            stayTier = true;
        }
        Debug.Log("Hitsum: " + hitSum);
        return false;
    }
    void UpdateTierStatus()
    {
        Debug.Log("update tier status");
        // updates tier status at end of a rally
        foreach (Target t in gameTargets)
        {
            if (advanceTier)
            {
                if (targetTier < 4)
                {
                    targetTier++;
                    t.SetTargetHeads(targetTier);
                }
            }
            else if (!stayTier)
            {
                t.SetTargetHeads(targetTier);
            }
        }
        Debug.Log("tier status: "+ targetTier);
        rallyOn = false;
        //ResetTargetPositions();
    }

    void ResetTargetPositions()
    {
        // put them back where they started
        foreach (Target t in gameTargets)
        {
            t.TargetSetter(-1f);
        }
    }


    void Rally()
    {
        rallyOn = true;
        RallySetter();
        StartRally();
        UpdateTierStatus();
    }

    void BetweenRallies()
    {
        // timer until next rally
    }

    IEnumerator ActiveWaiter(Target t)
    {
        yield return new WaitForSeconds(activateTimes[activateCounter]);
        t.targetActive = true;
        Debug.Log("Activated");
        t.TargetSetter(1f);
        t.TargetTime();
    }

    public void ActivateTarget()
    {
        int j;
        int i = 0;
        // shuffle gameTargets array here!
        while(i < activateTimes.Length)
        {
            j = Random.Range(0, activateTimes.Length);
            Debug.Log("x: " + activateTimes[i] + " y: " + activateTimes[j]);
            if (i == j)
            {
                j = (j + 1) % activateTimes.Length;
            }
            activateTimes[i] ^= activateTimes[j];
            activateTimes[j] = activateTimes[i] ^ activateTimes[j];
            activateTimes[i] ^= activateTimes[j];
            Debug.Log("x: " + activateTimes[i] + " y: " + activateTimes[j]);
            i++;
        }

        foreach (Target t in gameTargets)
        {
            StartCoroutine(ActiveWaiter(t));
            Debug.Log(activateCounter);
            activateCounter++;
        }
        activateCounter = 0;
    }

}

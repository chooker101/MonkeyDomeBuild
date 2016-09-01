using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class TargetManager : MonoBehaviour
{

    //private BallInfo ballInfo;


    public int hitSum = 0;
    public int targetTier;
    public bool[] targetsHitInSequence = new bool[5];
    public int sequenceIndex = 0;
    public bool advanceTier;
    public float timeBetweenRallies = 10f;

    [SerializeField]
    private Target[] gameTargets;
    private float startLifeTime;
    private int[] activateTimes = new int[5] { 0, 3, 6, 8, 10 };
    private int activateCounter;
    private int addScore;
    private bool isHit;
    private bool rallyOn;

    public Target[] TargetGetter()
    {
        return gameTargets;
    }

    public Target GetTargetAtIndex(int i)
    {
        return gameTargets[i];
    }

    public int GetTargetArrayLength()
    {
        return gameTargets.Length;
    }

    void Start()
    {
        rallyOn = false;
        advanceTier = false;
        activateCounter = 0;
        gameTargets = FindObjectsOfType<Target>();
        targetTier = 0;
        isHit = false;
        //ballInfo = GetComponent<BallInfo>();
        RallySetter();

        // put Rally() here when timer between them works
        Rally();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            foreach (Target t in gameTargets)
            {
                t.SetTargetHeads(targetTier);
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            Rally();
        }
    }


    void RallySetter()
    {
        // this method is used to clear the targetsHitInSequence array
        for (int i = 0; i < targetsHitInSequence.Length; i++)
        {
            targetsHitInSequence[i] = false;
        }
        hitSum = 0;
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
        UpdateTierStatus();
        // call this method at the start of each rally. will activate target and deactivate if hit
        isHit = false;
        ActivateTarget();
        foreach (Target t in gameTargets)
        {
            if (isHit == true)
            {
                t.TargetSetter(-1f);
            }
        }
    }

    public bool CheckRally()
    {
        // checks if enough targets in one rally are hit to upgrade tier
        if (hitSum >= 3)
        {
            return true;
        }

        if (targetTier > 0 && hitSum < 3)
        {
            targetTier--;
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
                advanceTier = false;
            }
        }
        sequenceIndex = 0;
    }

    void ResetTargetPositions()
    {
        // put them back where they started
    }


    void Rally()
    {
        rallyOn = true;
        RallySetter();
        StartRally();
    }

    void BetweenRallies()
    {
        FullRallyWaiter();
    }

    IEnumerator ActiveWaiter(Target t)
    {
        yield return new WaitForSeconds(activateTimes[activateCounter]);
        t.targetActive = true;
        t.SetTargetHeads(targetTier);
        t.TargetSetter(1f);
        t.TargetTime();
    }

    IEnumerator FullRallyWaiter()
    {
        yield return new WaitForSeconds(timeBetweenRallies);
    }

    public void ActivateTarget()
    {
        int j;
        int i = 0;

        while (i < activateTimes.Length)
        {
            j = Random.Range(0, activateTimes.Length);
            if (i == j)
            {
                j = (j + 1) % activateTimes.Length;
            }
            activateTimes[i] ^= activateTimes[j];
            activateTimes[j] = activateTimes[i] ^ activateTimes[j];
            activateTimes[i] ^= activateTimes[j];
            i++;
        }
        foreach (Target t in gameTargets)
        {
            StartCoroutine(ActiveWaiter(t));
            activateCounter++;
            if (activateCounter == 5)
            {
                Rally();
            }
        }
        activateCounter = 0;
    }
}

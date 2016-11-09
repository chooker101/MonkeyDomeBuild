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
    private float timeBetweenRallies = 10f;

    [SerializeField]
    private Target[] gameTargets;
    private float startLifeTime;
    private int[] activateTimes = new int[5] { 0, 3, 6, 8, 10 };
    private int activateCounter;
    private bool playAudienceAudio;

    public bool rallyOn;
    public int activeTargets;

    public int ActiveTargets
    {
        get
        {
            return activeTargets;
        }
        set
        {
            activeTargets = value;
        }
    }

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
    public int TargetTier
    {
        get
        {
            return targetTier;
        }
    }

	void Awake()
	{
		if (FindObjectOfType<TargetManager>() != GameManager.Instance.gmTargetManager)
			GameManager.Instance.gmTargetManager = FindObjectOfType<TargetManager>();
	}

    void Start()
    {
        rallyOn = false;
        advanceTier = false;
        activateCounter = 0;
        gameTargets = FindObjectsOfType<Target>();
        targetTier = 0;
        //ballInfo = GetComponent<BallInfo>();
        RallySetter();
        Invoke("StartRallyDelay", timeBetweenRallies);
    }


    void Update()
    {
        if(hitSum >= 3 && playAudienceAudio)
        {
            AudioEffectManager.Instance.PlayAudienceTargetUp();
            playAudienceAudio = false;
        }

        if (rallyOn && activeTargets == 0)
        {
            StopAllCoroutines();
            Invoke("StartRallyDelay", timeBetweenRallies);
            rallyOn = false;
            foreach (Target t in gameTargets)
            {
                t.RallyEnd();
            }
        }


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

    protected void StartRallyDelay()
    {
        Rally();
    }

    void RallySetter()
    {
        playAudienceAudio = true;
        // this method is used to clear the targetsHitInSequence array
        for (int i = 0; i < targetsHitInSequence.Length; i++)
        {
            targetsHitInSequence[i] = false;
        }
        //hitSum = 0;
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
        //isHit = false;
        ActivateTarget();
        /* foreach (Target t in gameTargets)
         {
             if (isHit == true)
             {
                 t.TargetSetter(-1f);
             }
         }*/
    }

    public bool CheckRally()
    {
        // checks if enough targets in one rally are hit to upgrade tier
        if (hitSum >= 3)
        {
            return true;
        }

        return false;
    }

    void UpdateTierStatus()
    {
        // updates tier status at end of a rally
        if (hitSum >= 3)
        {
            if (targetTier < 3)
            {
                targetTier++;
                //advanceTier = false;
            }
        }
        else if (hitSum == 2)
        {
            
        } else 
        {
            if (targetTier > 0 && hitSum < 3)
            {
                targetTier--;
            }
        }
        hitSum = 0;
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
        //advanceTier = CheckRally();
        StartRally();
        foreach(Target t in gameTargets)
        {
            t.RallyStart();
        }
    }

    void BetweenRallies()
    {
        FullRallyWaiter();
    }

    IEnumerator ActiveWaiter(Target t)
    {
        //activeTargets++;
        yield return new WaitForSeconds(activateTimes[activateCounter] + timeBetweenRallies);
        t.targetActive = true;
        //t.SetTargetHeads(targetTier);
        t.TargetSetter();
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
        foreach(Target t in gameTargets)
        {
            t.WaitTime = 0f;
        }
        List<int> tempTargetIndexs = new List<int>();
        int repeatTimes = 0;
        while (tempTargetIndexs.Count < activateTimes.Length)
        {
            int tempIndex = Random.Range(0, gameTargets.Length);
            bool repeated = false;
            for(int k = 0; k < tempTargetIndexs.Count; k++)
            {
                if (tempTargetIndexs[k] == tempIndex)
                {
                    repeated = true;
                    break;
                }
            }
            if (!repeated)
            {
                tempTargetIndexs.Add(tempIndex);
            }
            else
            {
                repeatTimes++;
                if (repeatTimes > 10)
                {
                    break;
                }
            }
        }
        for(int k = 0; k < tempTargetIndexs.Count; k++)
        {
            activeTargets++;
            gameTargets[tempTargetIndexs[k]].WaitTime = activateTimes[activateCounter] + timeBetweenRallies;
            StartCoroutine(ActiveWaiter(gameTargets[tempTargetIndexs[k]]));
            activateCounter++;
            if (activateCounter == 6)
            {
                Rally();
            }
        }
        /*
        foreach (Target t in gameTargets)
        {
            activeTargets++;
            StartCoroutine(ActiveWaiter(t));
            activateCounter++;
            if (activateCounter == 6)
            {
                Rally();
            }
        }
        */
        activateCounter = 0;
    }
    public bool RallyOn
    {
        get
        {
            return rallyOn;
        }
    }
    public float HitSum
    {
        get
        {
            return hitSum;
        }
    }
    public void TargetHit()
    {
        hitSum++;
    }
}

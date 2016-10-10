using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This script will:
 * - track audience approval rating of players
 *      - which involves paying attention for player behaviors (cheap throws, taunting, risky plays)
 * - manage audience blurbs to correspond with player ratings
 *      - including good/bad feedback and call for certain playstyles to specific players
 * - store information for trophy rewards at end of match
 */

public class AudienceManager : MonoBehaviour
{

    public enum AudienceEvent
    {
        Jump, //=0
        Targets,
        Buzzer,
        Bananas,
        KnockDown,
        Intercept,
        BouncePass,
        PoopStorm
    }

    public enum PlayerNum
    {
        Player1, //=0
        Player2,
        Player3,
        Player4,
        Player5
    }

    private float newEventTimer;
    private float activeEventLifeTime;
    [SerializeField]
    private string activeEventTitle;
    private bool eventActive = false;
    private PlayerNum currentEvenWinner;
    private PlayerNum currentEventLoser;

    public static Dictionary<PlayerNum, int> audPlayerOpinion = new Dictionary<PlayerNum, int>();
    public static Dictionary<PlayerNum, int> eventGoalSuccess = new Dictionary<PlayerNum, int>();

    private AudienceEvent randEvent;
    private AudienceEvent currentEvent;

    private uint cachedTotalNumPlayers;

    //specific event variables for event completion checks
    private int startingTargetTier;
    private int targetUpScore = 10; //TODO test score reward for targets upgraded and adjust
    private int bananasCaughtScore = 1;
    private int gorillaInterceptionScore = 15;
    private int poopOpinionBoost = 5; //TODO test poop opinion boost and adjust

    void Start()
    {
        //initialize the audience opinion levels to 0
        for (int i = 0; i < audPlayerOpinion.Count; ++i)
        {
            audPlayerOpinion.Add((PlayerNum)i, 0);
            eventGoalSuccess.Add((PlayerNum)i, 0);
        }
        EventEnd();
        cachedTotalNumPlayers = GameManager.Instance.TotalNumberofPlayers;
    }

    void Update()
    {
        if(newEventTimer <= 0 && !eventActive)
        {
            StartAudienceEvent();
        }
        if(!eventActive)
        {
            newEventTimer -= Time.fixedDeltaTime;
        }
        if (eventActive)
        {
            switch (currentEvent)
            {

                case AudienceEvent.Jump:
                    JumpEvent();
                    break;
                case AudienceEvent.Targets:
                    TargetUpgradeEvent();
                    break;
                case AudienceEvent.Buzzer:
                    BuzzerBeaterEvent();
                    break;
                case AudienceEvent.Bananas:
                    BananaCatchEvent();
                    break;
                case AudienceEvent.KnockDown:
                    KnockDownMonkeysEvent();
                    break;
                case AudienceEvent.Intercept:
                    GorillaInterceptEvent();
                    break;
                case AudienceEvent.BouncePass:
                    BouncePassEvent();
                    break;
                case AudienceEvent.PoopStorm:
                    PoopStormEvent();
                    break;
            }
        }
    }

    void UpdateAudienceMood()
    {

    }

    void StartAudienceEvent()
    {
        currentEvent = RandomAudienceEvent();

        for (int i = 0; i < eventGoalSuccess.Count; ++i)
        {
            eventGoalSuccess[(PlayerNum)i] = 0;
        }

        switch (currentEvent)
        {

            case AudienceEvent.Jump:
                activeEventTitle = "Jump Around!";
                activeEventLifeTime = UnityEngine.Random.Range(5.0f, 8.0f);
                break;
            case AudienceEvent.Targets:
                activeEventTitle = "Upgrade Targets!";
                startingTargetTier = GameManager.Instance.gmTargetManager.targetTier;
                activeEventLifeTime = UnityEngine.Random.Range(13.0f, 15.0f);
                break;
            case AudienceEvent.Buzzer:
                activeEventTitle = "Buzzer Beater";
                activeEventLifeTime = UnityEngine.Random.Range(10.0f, 13.0f);
                break;
            case AudienceEvent.Bananas:
                activeEventTitle = "Catch Bananas!";
                activeEventLifeTime = UnityEngine.Random.Range(5.0f, 8.0f);
                break;
            case AudienceEvent.KnockDown:
                activeEventTitle = "Knock Down Monkeys!";
                activeEventLifeTime = UnityEngine.Random.Range(13.0f, 16.0f);
                break;
            case AudienceEvent.Intercept:
                activeEventTitle = "Gorilla Intercept!";
                activeEventLifeTime = UnityEngine.Random.Range(8.0f, 10.0f);
                break;
            case AudienceEvent.BouncePass:
                activeEventTitle = "One Bounce Pass!";
                activeEventLifeTime = UnityEngine.Random.Range(3.0f, 5.0f);
                break;
            case AudienceEvent.PoopStorm:
                activeEventTitle = "Poop Storm!";
                activeEventLifeTime = UnityEngine.Random.Range(5.0f, 8.0f);
                break;
        }
        eventActive = true;
        StartCoroutine(EventTimer());
    }

    AudienceEvent RandomAudienceEvent()
    {
        System.Array rand = System.Enum.GetValues(typeof(AudienceEvent));
        randEvent = (AudienceEvent)rand.GetValue(UnityEngine.Random.Range(0, rand.Length));
        return randEvent;
    }

    void EventEnd()
    {
        eventActive = false;
        //TODO add the call to UpdateAudienceOpinion here
        newEventTimer = UnityEngine.Random.Range(8.0f, 12.0f);
    }

    IEnumerator EventTimer()
    {
        //When event is called
        yield return new WaitForSeconds(activeEventLifeTime);
        //after timer finishes
        eventActive = false;
    }

    void JumpEvent()
    {
        for(int i = 0; i < (int)cachedTotalNumPlayers; ++i)
        {
            if (GameManager.Instance.gmInputs[i].mJump)
            {
                ++eventGoalSuccess[(PlayerNum)i];
            }
        }

        //TODO move this to an UpdateAudienceOpinion section so that it will actually get called
        if (!eventActive)
        {
            for(int i = 0; i < (int)cachedTotalNumPlayers; ++i)
            {
                audPlayerOpinion[(PlayerNum)i] += (int)(0.5 * eventGoalSuccess[(PlayerNum)i]);
                eventGoalSuccess[(PlayerNum)i] = 0;
            }
            EventEnd();
        }
    }

    void TargetUpgradeEvent()
    {
        bool targetsUpgraded = false;
        if(GameManager.Instance.gmTargetManager.targetTier > startingTargetTier)
        {
            targetsUpgraded = true;
        }

        if (targetsUpgraded)
        {
            for(int i = 0; i < (int)cachedTotalNumPlayers; ++i)
            {
                if(GameManager.Instance.gmPlayers[i].GetComponent<Player>().characterType is Monkey)
                {
                    audPlayerOpinion[(PlayerNum)i] += targetUpScore;
                }
            }
            eventActive = false;
        }

        if (!eventActive)
        {
            EventEnd();
        }
    }

    void BuzzerBeaterEvent()
    {
        //TODO add audeience throwing bananas to successful player during event
    }

    void BananaCatchEvent()
    {
        
    }

    public void BananaCaught(int pIndex)
    {
        PlayerNum pNum = (PlayerNum)pIndex;
        ++eventGoalSuccess[pNum];
    }

    public void HitByPoop(int pIndex)
    {
        PlayerNum pNum = (PlayerNum)pIndex;
        audPlayerOpinion[pNum] += poopOpinionBoost;
    }

    void KnockDownMonkeysEvent()
    {

    }

    void GorillaInterceptEvent()
    {
        //TODO audience throws bananas at successful player after switch
    }

    public void AudGorillaIntercepted(GameObject plyr)
    {
        PlayerNum pNum = (PlayerNum)plyr.GetComponent<Actor>().playerIndex;
        audPlayerOpinion[pNum] += gorillaInterceptionScore;
    }

    void BouncePassEvent()
    {
        //TODO add audience throwing bananas to successful player during event
    }

    void PoopStormEvent()
    {

    }

    public AudienceEvent GetCurrentEvent()
    {
        return currentEvent;
    }

    public bool GetEventActive()
    {
        return eventActive;
    }

}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class VictoryRoomManager : MonoBehaviour
{
    // Public variables
    public float maxHeight;
    public float minHeight;
    public float speed;
    public float podiumBufferTime;
    public GameObject podiums;
    public GameObject Podium1;
    public GameObject Podium2;
    public GameObject Podium3;
    public GameObject Podium4;
    public GameObject Podium5;
    public List<GameObject> TrophyCase;
    public List<SpriteRenderer> TrophyBgs;

    public GameObject Trophy;
    public Transform[] AwardBananasTrophyPos;
    public Transform[] PerfectCatchTrophyPos;
    public Transform[] TargetsHitTrophyPos;
    public Transform[] BallCallingTrophyPos;
    public Transform[] AwardPoopTrophyPos;
    public Transform[] KnockDownTrophyPos;
    public int a;
    public int b;
    public int c;
    public int d;
    public int e;
    public int f;
    public float victoryTimer;
    public float victoryTimerInvisible;
    public Canvas canvas_victoryTimer;
    // Private Variables

    private float totalScore;
    private bool podiumsReady = false;
    private ScoringManager scoreKeeper;
    private GameManager GameManager;
    private TrophyManager TrophyManager;
    private Vector3 podiumPos1;
    private Vector3 podiumPos2;
    private Vector3 podiumPos3;
    private Vector3 podiumPos4;
    private Vector3 podiumPos5;
    private Vector3 temp;
    private float p1score;
    private float p2score;
    private float p3score;
    private float p4score;
    private float p5score;
    private bool victoryTimerIsVisible = false;
    private bool loadedScene = false;
    private Text text_victoryTimer;
    private GameObject timerTarget;

    // Use this for initialization
    void Start()
    {

        GameManager = FindObjectOfType<GameManager>();
        TrophyManager = GameManager.Instance.gmTrophyManager;
        scoreKeeper = GameManager.Instance.gmScoringManager;
        text_victoryTimer = canvas_victoryTimer.transform.FindChild("VictoryTimer").GetComponent<Text>();
        canvas_victoryTimer.gameObject.SetActive(false);
        timerTarget = transform.FindChild("TimerTarget").gameObject;
        timerTarget.SetActive(false);

        /*
        p1score = scoreKeeper.p1Score / heightDeviser;
        p2score = scoreKeeper.p2Score / heightDeviser;
        p3score = scoreKeeper.p3Score / heightDeviser;
        p4score = scoreKeeper.p4Score / heightDeviser;
        p5score = scoreKeeper.p5Score / heightDeviser;
        */

        totalScore = scoreKeeper.p1Score + scoreKeeper.p2Score + scoreKeeper.p3Score + scoreKeeper.p4Score + scoreKeeper.p5Score;

        if(totalScore != 0)
        {
            p1score = (scoreKeeper.p1Score / totalScore) * (maxHeight - minHeight) + minHeight;
            p2score = (scoreKeeper.p2Score / totalScore) * (maxHeight - minHeight) + minHeight;
            p3score = (scoreKeeper.p3Score / totalScore) * (maxHeight - minHeight) + minHeight;
            p4score = (scoreKeeper.p4Score / totalScore) * (maxHeight - minHeight) + minHeight;
            p5score = (scoreKeeper.p5Score / totalScore) * (maxHeight - minHeight) + minHeight;
        }
        else
        {
            p1score = minHeight;
            p2score = minHeight;
            p3score = minHeight;
            p4score = minHeight;
            p5score = minHeight;
        }

        Podium1 = GameObject.Find("Podium1");
        Podium2 = GameObject.Find("Podium2");
        Podium3 = GameObject.Find("Podium3");
        Podium4 = GameObject.Find("Podium4");
        Podium5 = GameObject.Find("Podium5");


        movePodiums();
        GameManager.Instance.gmSpawnManager.Start();
        moveCase();
        PlaceTrophies();
    }

    // Update is called once per frame
    void Update()
    {
        if(podiumBufferTime >= 0 && !podiumsReady) // Podium buffer time for entering the room
        {
            podiumBufferTime -= Time.deltaTime;

            if(podiumBufferTime <= 0)
            {
                podiumBufferTime = 0;
                podiumsReady = true;
            }
        }

        if (victoryTimerInvisible != 0 && !victoryTimerIsVisible)
        {
            victoryTimerInvisible -= Time.deltaTime;

            if (victoryTimerInvisible <= 0)
            {
                victoryTimerIsVisible = true;
                victoryTimerInvisible = 0;
                timerTarget.SetActive(true);
            }
        }
        if (victoryTimer != 0 && !loadedScene)
        {
            victoryTimer -= Time.deltaTime;

            if(victoryTimer <= 0)
            {
                loadedScene = true;
                victoryTimer = 0;
                GameManager.LoadPregameRoom();
                scoreKeeper.p1Score = 0;
                scoreKeeper.p2Score = 0;
                scoreKeeper.p3Score = 0;
                scoreKeeper.p4Score = 0;
                scoreKeeper.p5Score = 0;
            }
        }

        if (podiumsReady) // Once buffer time has completed
        {
            // Podiums
            if(GameManager.Instance.TotalNumberofPlayers >= 1)
            {
                Podium1.transform.position = Vector3.MoveTowards(Podium1.transform.position,
                    new Vector3(podiumPos1.x, podiumPos1.y + p1score, podiumPos1.z), speed * Time.deltaTime);
                
            }
            if (GameManager.Instance.TotalNumberofPlayers >= 2)
            {
                Podium2.transform.position = Vector3.MoveTowards(Podium2.transform.position, 
                    new Vector3(podiumPos2.x, podiumPos2.y + p2score, podiumPos2.z), speed * Time.deltaTime);
            }
            if (GameManager.Instance.TotalNumberofPlayers >= 3)
            {
                Podium3.transform.position = Vector3.MoveTowards(Podium3.transform.position, 
                    new Vector3(podiumPos3.x, podiumPos3.y + p3score, podiumPos3.z), speed * Time.deltaTime);
            }
            if (GameManager.Instance.TotalNumberofPlayers >= 4)
            {
                Podium4.transform.position = Vector3.MoveTowards(Podium4.transform.position, 
                    new Vector3(podiumPos4.x, podiumPos4.y + p4score, podiumPos4.z), speed * Time.deltaTime);
            }
            if (GameManager.Instance.TotalNumberofPlayers == 5)
            {
                Podium5.transform.position = Vector3.MoveTowards(Podium5.transform.position, 
                    new Vector3(podiumPos5.x, podiumPos5.y + p5score, podiumPos5.z), speed * Time.deltaTime);
            }
        }

        if (victoryTimerIsVisible)
        {
            if (!canvas_victoryTimer.gameObject.activeSelf)
            {
                canvas_victoryTimer.gameObject.SetActive(true);
            }

            text_victoryTimer.text = victoryTimer.ToString("F0");
        }
    }

    void PlaceTrophies()
    {
        if (TrophyManager.a >0)
        {
            a = TrophyManager.a;
            GameObject BananaTrophy = (GameObject)Instantiate(Trophy, AwardBananasTrophyPos[a].transform.position, podiums.transform.rotation);
        }


        if (TrophyManager.b >= 0)
        {
            b = TrophyManager.b;
            GameObject CatchTrophy = (GameObject)Instantiate(Trophy, PerfectCatchTrophyPos[b].transform.position, podiums.transform.rotation);
        }

        if (TrophyManager.c >= 0)
        {
            c = TrophyManager.c;
            GameObject TargetsTrophy = (GameObject)Instantiate(Trophy, TargetsHitTrophyPos[c].transform.position, podiums.transform.rotation);
        }

        if (TrophyManager.d >= 0)
        {
            d = TrophyManager.d;
            GameObject CallingTrophy = (GameObject)Instantiate(Trophy, BallCallingTrophyPos[d].transform.position, podiums.transform.rotation);
        }

        if (TrophyManager.e >= 0)
        {
            e = TrophyManager.e;
            GameObject PoopTrophy = (GameObject)Instantiate(Trophy, AwardPoopTrophyPos[e].transform.position, podiums.transform.rotation);
        }

        if (TrophyManager.f >= 0)
        {
            f = TrophyManager.f;
            GameObject KnockDownTrophy = (GameObject)Instantiate(Trophy, KnockDownTrophyPos[f].transform.position, podiums.transform.rotation);
        }
        
        /*Debug.Log(a);
        Debug.Log(b);
        Debug.Log(c);
        Debug.Log(d);
        Debug.Log(e);
        Debug.Log(f);*/
    }

    void movePodiums()
    {
        temp = podiums.transform.position;

        if (GameManager.TotalNumberofPlayers == 1)
            temp.x = 22.6f;
        if (GameManager.TotalNumberofPlayers == 2)
            temp.x = 18.7f;
        if (GameManager.TotalNumberofPlayers == 3)
            temp.x = 14.6f;
        if (GameManager.TotalNumberofPlayers == 4)
            temp.x = 10.5f;
        if (GameManager.TotalNumberofPlayers == 5)
            temp.x = 6.3f;
        
        podiums.transform.position = temp;

        podiumPos1 = Podium1.transform.position;
        podiumPos2 = Podium2.transform.position;
        podiumPos3 = Podium3.transform.position;
        podiumPos4 = Podium4.transform.position;
        podiumPos5 = Podium5.transform.position;
    }
    void moveCase()
    {
        for(int i = 4; i > GameManager.Instance.TotalNumberofPlayers-1; i--)
        {
            TrophyCase[i].SetActive(false);
        }
        for (int i = 0; i < GameManager.Instance.TotalNumberofPlayers - 1; i++)
        {
            TrophyBgs[i].material = GameManager.Instance.gmRecordKeeper.colourPlayers[i];
            
        }

    }
}

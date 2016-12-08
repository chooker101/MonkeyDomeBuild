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
    public GameObject allPodiums;
    public List<GameObject> podiumList;
    public List<GameObject> podiumListRenderers;
    public List<Vector3> podiumPositions;
    private Vector3 temp;


    public List<GameObject> TrophyCase;
    public List<SpriteRenderer> TrophyBg = new List<SpriteRenderer>();


   
    public Transform[] PointsTrophyPos;
    public GameObject PointsTrophy;
    public Transform[] CatchTrophyPos;
    public GameObject CatchTrophy;
    public Transform[] TargetsTrophyPos;
    public GameObject TargetsTrophy;
    public Transform[] CallingTrophyPos;
    public GameObject CallingTrophy;

    public Transform[] CoconutTrophyPos;
    public GameObject CoconutTrophy;

    public Transform[] TackleTrophyPos;
    public GameObject TackleTrophy;

    [HideInInspector]
    public int a;
    [HideInInspector]
    public int b;
    [HideInInspector]
    public int c;
    [HideInInspector]
    public int d;
    [HideInInspector]
    public int e;
    [HideInInspector]
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

        if (totalScore != 0)
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

        movePodiums();
        GameManager.Instance.gmSpawnManager.Start();
        setCase();
        PlaceTrophies();
    }

    // Update is called once per frame
    void Update()
    {
        if (podiumBufferTime >= 0 && !podiumsReady) // Podium buffer time for entering the room
        {
            podiumBufferTime -= Time.deltaTime;

            if (podiumBufferTime <= 0)
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

            if (victoryTimer <= 0)
            {
                loadedScene = true;
                victoryTimer = 0;
                
                /*scoreKeeper.ClearScores();
                for (int i = 0; i < GameManager.TotalNumberofActors; i++)
                {
                    GameManager.Instance.gmRecordKeeper.SetPlayerMaterial(i, GameManager.gmRecordKeeper.defaultColour);
                    GameManager.Instance.gmPlayers[i].GetComponent<Actor>().UpdateColour();
                }*/
                GameManager.LoadPregameRoom();
            }

            if (podiumsReady) // Once buffer time has completed
            {

                // Podiums
                if (GameManager.Instance.TotalNumberofActors >= 1)
                {
                    podiumList[0].transform.position = Vector3.MoveTowards(podiumList[0].transform.position,
                        new Vector3(podiumPositions[0].x, podiumPositions[0].y + p1score, podiumPositions[0].z), speed * Time.deltaTime);

                }
                if (GameManager.Instance.TotalNumberofActors >= 2)
                {
                    podiumList[1].transform.position = Vector3.MoveTowards(podiumList[1].transform.position,
                        new Vector3(podiumPositions[1].x, podiumPositions[1].y + p2score, podiumPositions[1].z), speed * Time.deltaTime);
                }
                if (GameManager.Instance.TotalNumberofActors >= 3)
                {
                    podiumList[2].transform.position = Vector3.MoveTowards(podiumList[2].transform.position,
                        new Vector3(podiumPositions[2].x, podiumPositions[2].y + p3score, podiumPositions[2].z), speed * Time.deltaTime);
                }
                if (GameManager.Instance.TotalNumberofActors >= 4)
                {
                    podiumList[3].transform.position = Vector3.MoveTowards(podiumList[3].transform.position,
                        new Vector3(podiumPositions[3].x, podiumPositions[3].y + p4score, podiumPositions[3].z), speed * Time.deltaTime);
                }
                if (GameManager.Instance.TotalNumberofActors == 5)
                {
                    podiumList[4].transform.position = Vector3.MoveTowards(podiumList[4].transform.position,
                        new Vector3(podiumPositions[4].x, podiumPositions[4].y + p5score, podiumPositions[4].z), speed * Time.deltaTime);
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
    }

    void PlaceTrophies()
    {
        if (TrophyManager.a > 0)
        {
            a = TrophyManager.a;
            GameObject points_trophy = (GameObject)Instantiate(PointsTrophy, PointsTrophyPos[a].transform.position, allPodiums.transform.rotation);
        }

        if (TrophyManager.b >= 0)
        {
            b = TrophyManager.b;
            GameObject catch_trophy = (GameObject)Instantiate(CatchTrophy, CatchTrophyPos[b].transform.position, allPodiums.transform.rotation);
        }

        if (TrophyManager.c >= 0)
        {
            c = TrophyManager.c;
            GameObject targets_trophy = (GameObject)Instantiate(TargetsTrophy, TargetsTrophyPos[c].transform.position, allPodiums.transform.rotation);
        }

        if (TrophyManager.d >= 0)
        {
            d = TrophyManager.d;
            GameObject calling_trophy = (GameObject)Instantiate(CallingTrophy, CallingTrophyPos[d].transform.position, allPodiums.transform.rotation);
        }

        if (TrophyManager.e >= 0)
        {
            e = TrophyManager.e;
            GameObject coconut_trophy = (GameObject)Instantiate(CoconutTrophy, CoconutTrophyPos[e].transform.position, allPodiums.transform.rotation);
        }

        if (TrophyManager.f >= 0)
        {
            f = TrophyManager.f;
            GameObject tackle_trophy = (GameObject)Instantiate(TackleTrophy, TackleTrophyPos[f].transform.position, allPodiums.transform.rotation);
        }
    }

    void movePodiums()
    {
        temp = allPodiums.transform.position;

        if (GameManager.TotalNumberofActors == 1)
            temp.x = 22.6f;
        if (GameManager.TotalNumberofActors == 2)
            temp.x = 18.7f;
        if (GameManager.TotalNumberofActors == 3)
            temp.x = 14.6f;
        if (GameManager.TotalNumberofActors == 4)
            temp.x = 10.5f;
        if (GameManager.TotalNumberofActors == 5)
            temp.x = 6.3f;

        allPodiums.transform.position = temp;

        podiumPositions[0] = podiumList[0].transform.position;
        podiumPositions[1] = podiumList[1].transform.position;
        podiumPositions[2] = podiumList[2].transform.position;
        podiumPositions[3] = podiumList[3].transform.position;
        podiumPositions[4] = podiumList[4].transform.position;

        for (int i = 4; i >= GameManager.Instance.TotalNumberofActors; i--)
        {
            podiumList[i].SetActive(false);
        }
        for (int i = 0; i < GameManager.Instance.TotalNumberofActors; i++)
        {
            //podiumList[i].SetActive(true);
            var podiumCol = GameManager.Instance.gmRecordKeeper.colourPlayers[i];
            podiumListRenderers[i].GetComponent<Renderer>().material = podiumCol;
        }
    }

    void setCase()
    {
        for (int i = 4; i >= GameManager.Instance.TotalNumberofActors; i--)
        {
            TrophyCase[i].SetActive(false);
        }
        for (int i = 0; i < GameManager.Instance.TotalNumberofActors; i++)
        {
            var bkColor = GameManager.Instance.gmRecordKeeper.colourPlayers[i].color;
            bkColor.a = 0.5f;
            TrophyBg[i].color = bkColor;
        }

    }
}
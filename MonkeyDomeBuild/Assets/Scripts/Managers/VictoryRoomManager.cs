using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VictoryRoomManager : MonoBehaviour
{
    private ScoringManager scoreKeeper;
    private GameManager GameManager;
    private TrophyManager TrophyManager;

    public float speed;
    public float heightDeviser;

    public GameObject Podium1;
    public GameObject Podium2;
    public GameObject Podium3;
    public GameObject Podium4;
    public GameObject Podium5;

    public GameObject TrophyCase;

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

    public GameObject Trophy; 
    public GameObject[] AwardBananasTrophyPos;
    public GameObject[] PerfectCatchTrophyPos;
    public GameObject[] TargetsHitTrophyPos;
    public GameObject[] BallCallingTrophyPos;
    public GameObject[] AwardPoopTrophyPos;
    public GameObject[] KnockDownTrophyPos;

    public int a;
    public int b;
    public int c;
    public int d;
    public int e;
    public int f;


    // Use this for initialization
    void Start()
    {

        GameManager = FindObjectOfType<GameManager>();
        TrophyManager = GameManager.Instance.gmTrophyManager;
        scoreKeeper = GameManager.Instance.gmScoringManager;

        p1score = scoreKeeper.p1Score / heightDeviser;
        p2score = scoreKeeper.p2Score / heightDeviser;
        p3score = scoreKeeper.p3Score / heightDeviser;
        p4score = scoreKeeper.p4Score / heightDeviser;
        p5score = scoreKeeper.p5Score / heightDeviser;

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
        Podium1.transform.position = Vector3.MoveTowards(Podium1.transform.position, new Vector3(podiumPos1.x, podiumPos1.y + p1score, podiumPos1.z), speed * Time.deltaTime);
        Podium2.transform.position = Vector3.MoveTowards(Podium2.transform.position, new Vector3(podiumPos2.x, podiumPos2.y + p2score, podiumPos2.z), speed * Time.deltaTime);
        Podium3.transform.position = Vector3.MoveTowards(Podium3.transform.position, new Vector3(podiumPos3.x, podiumPos3.y + p3score, podiumPos3.z), speed * Time.deltaTime);
        Podium4.transform.position = Vector3.MoveTowards(Podium4.transform.position, new Vector3(podiumPos4.x, podiumPos4.y + p4score, podiumPos4.z), speed * Time.deltaTime);
        Podium5.transform.position = Vector3.MoveTowards(Podium5.transform.position, new Vector3(podiumPos5.x, podiumPos5.y + p5score, podiumPos5.z), speed * Time.deltaTime);
    }

    void PlaceTrophies()
    {
        
        a = TrophyManager.a;
        GameObject BananaTrophy = (GameObject)Instantiate(Trophy, AwardBananasTrophyPos[a].transform.position, transform.rotation);
     
        b = TrophyManager.b;
        GameObject CatchTrophy = (GameObject)Instantiate(Trophy, PerfectCatchTrophyPos[b].transform.position, transform.rotation);
     
        c = TrophyManager.c;
        GameObject TargetsTrophy = (GameObject)Instantiate(Trophy, TargetsHitTrophyPos[c].transform.position, transform.rotation);  

        d = TrophyManager.d;
        GameObject CallingTrophy = (GameObject)Instantiate(Trophy, BallCallingTrophyPos[d].transform.position, transform.rotation);
        
        e = TrophyManager.e;
        GameObject PoopTrophy = (GameObject)Instantiate(Trophy, AwardPoopTrophyPos[e].transform.position, transform.rotation);
     
        f = TrophyManager.f;
        GameObject KnockDownTrophy = (GameObject)Instantiate(Trophy, KnockDownTrophyPos[f].transform.position, transform.rotation);
        
        Debug.Log(a);
        Debug.Log(b);
        Debug.Log(c);
        Debug.Log(d);
        Debug.Log(e);
        Debug.Log(f);
    }

    void movePodiums()
    {
        temp = transform.position;

        if (GameManager.TotalNumberofPlayers == 1)
            temp.x = 15f;
        if (GameManager.TotalNumberofPlayers == 2)
            temp.x = 11;
        if (GameManager.TotalNumberofPlayers == 3)
            temp.x = 7f;
        if (GameManager.TotalNumberofPlayers == 4)
            temp.x = 3;
        if (GameManager.TotalNumberofPlayers == 5)
            temp.x = -1;

        transform.position = temp;

        podiumPos1 = Podium1.transform.position;
        podiumPos2 = Podium2.transform.position;
        podiumPos3 = Podium3.transform.position;
        podiumPos4 = Podium4.transform.position;
        podiumPos5 = Podium5.transform.position;
    }
    void moveCase()
    {
        temp = TrophyCase.transform.position;

        if (GameManager.TotalNumberofPlayers == 1)
            temp.y = 15.7f;
        if (GameManager.TotalNumberofPlayers == 2)
            temp.y = 19.45f;
        if (GameManager.TotalNumberofPlayers == 3)
            temp.y = 23.1f;
        if (GameManager.TotalNumberofPlayers == 4)
            temp.y = 26.85f;
        if (GameManager.TotalNumberofPlayers == 5)
            temp.y = 30.5f;

        TrophyCase.transform.position = temp;
    }
}

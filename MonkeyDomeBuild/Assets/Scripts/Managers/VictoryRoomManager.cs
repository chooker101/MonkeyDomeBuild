using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class VictoryRoomManager : MonoBehaviour
{
    public ScoringManager scoreKeeper;
    public GameManager gameManager;

    public float speed;
    public float heightDeviser;

    public GameObject Podium1;
    public GameObject Podium2;
    public GameObject Podium3;
    public GameObject Podium4;
    public GameObject Podium5;

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

    // Use this for initialization
    void Start()
    {
        scoreKeeper = FindObjectOfType<ScoringManager>();
        gameManager = FindObjectOfType<GameManager>();

        p1score = scoreKeeper.p1Score / heightDeviser;
        p2score = scoreKeeper.p2Score / heightDeviser;
        p3score = scoreKeeper.p3Score / heightDeviser;
        p4score = scoreKeeper.p4Score / heightDeviser;
        p5score = scoreKeeper.p5Score / heightDeviser;


        temp = transform.position;


        if( gameManager.TotalNumberofPlayers == 1 )
            temp.x = 15f;
        if( gameManager.TotalNumberofPlayers == 2 )
            temp.x = 11;
        if( gameManager.TotalNumberofPlayers == 3 )
            temp.x = 7f;
        if( gameManager.TotalNumberofPlayers == 4 )
            temp.x = 3;
        if( gameManager.TotalNumberofPlayers == 5 )
            temp.x = -1;

        transform.position = temp;


        Podium1 = GameObject.Find( "Podium1" );
        Podium2 = GameObject.Find( "Podium2" );
        Podium3 = GameObject.Find( "Podium3" );
        Podium4 = GameObject.Find( "Podium4" );
        Podium5 = GameObject.Find( "Podium5" );

        podiumPos1 = Podium1.transform.position;
        podiumPos2 = Podium2.transform.position;
        podiumPos3 = Podium3.transform.position;
        podiumPos4 = Podium4.transform.position;
        podiumPos5 = Podium5.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Podium1.transform.position = Vector3.MoveTowards( Podium1.transform.position, new Vector3( podiumPos1.x, podiumPos1.y + p1score, podiumPos1.z ), speed * Time.deltaTime );
        Podium2.transform.position = Vector3.MoveTowards( Podium2.transform.position, new Vector3( podiumPos2.x, podiumPos2.y + p2score, podiumPos2.z ), speed * Time.deltaTime );
        Podium3.transform.position = Vector3.MoveTowards( Podium3.transform.position, new Vector3( podiumPos3.x, podiumPos3.y + p3score, podiumPos3.z ), speed * Time.deltaTime );
        Podium4.transform.position = Vector3.MoveTowards( Podium4.transform.position, new Vector3( podiumPos4.x, podiumPos4.y + p4score, podiumPos4.z ), speed * Time.deltaTime );
        Podium5.transform.position = Vector3.MoveTowards( Podium5.transform.position, new Vector3( podiumPos5.x, podiumPos5.y + p5score, podiumPos5.z ), speed * Time.deltaTime );

        if( scoreKeeper == null )
            scoreKeeper = FindObjectOfType<ScoringManager>();

        if( gameManager == null )
            gameManager = FindObjectOfType<GameManager>();
    }
}

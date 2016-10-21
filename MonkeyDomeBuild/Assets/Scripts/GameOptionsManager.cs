using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOptionsManager : MonoBehaviour {

    public bool defaultOptions;

    public float defaultMatchTime;
    public float defaultShotClock;


    public UIManager UIManager;
    public Text matchtimeText;
    public float MatchTime;

    public ShotClockManager ShotClockManager;
    public Text ShottimeText;
    public  float ShotClock;

    //public GameManager GameManager;
    //public Text numPlayersText;
    //public uint tempPlayers;

    void SetTimers()
    {
        //UIManager.startMatchTime = MatchTime;
        ShotClockManager.shotClockTime = ShotClock;
    }

    void Awake()
    {
  

        ShotClockManager = ShotClockManager.FindObjectOfType<ShotClockManager>();

        //GameManager = GameManager.FindObjectOfType<GameManager>();



    }
	// Use this for initialization
	void Start () {
        if (defaultOptions == true)
        {
            MatchTime = defaultMatchTime;
            ShotClock = defaultShotClock;

            //GameManager.TotalNumberofPlayers = tempPlayers;
        }

        //UIManager.startMatchTime = MatchTime;
        ShotClockManager.shotClockTime = ShotClock;
    }

 
	
	// Update is called once per frame
	void Update () {
        matchtimeText.text = MatchTime.ToString();
        ShottimeText.text = ShotClock.ToString();
        //numPlayersText.text = tempPlayers.ToString();

 

    }

    public void AddMatchClock()
    {
        if (UIManager.startMatchTime >= 0f)
        { UIManager.noTime = false; }
        MatchTime  += 1;
    }

    public void LowerMatchClock()
    {
        if (MatchTime <= 1f)
        { UIManager.noTime = true; MatchTime = 0f; }
        else
        MatchTime -= 1f;
    }

    public void AddShotClock()
    {
        ShotClock += 1;
    }

    public void LowerShotClock()
    {
        if (ShotClock > 3f)
            ShotClock -= 1;
    }

    /*
    public void AddPlayers()
    {
        if(GameManager.TotalNumberofPlayers <5)
            tempPlayers += 1;
    }

    public void LowerPlayers()
    {
        if (tempPlayers > 1)
            tempPlayers -= 1;
    }
    */

  public void restartCurrentScene()
      {
          int scene = SceneManager.GetActiveScene().buildIndex;
          SceneManager.LoadScene(scene, LoadSceneMode.Single);
        SetTimers();
      }

}

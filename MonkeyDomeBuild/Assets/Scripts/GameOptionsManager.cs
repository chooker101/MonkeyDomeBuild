using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOptionsManager : MonoBehaviour {
    public bool GameoptionsChanged; 

    public UIManager Matchtime;
    public Text matchtimeText;
    public float tempMatchTime;


    public ShotClockManager Shotclock;
    public Text ShottimeText;
    public float tempShotClock;


    public GameManager GameManager;
    public Text numPlayersText;
    public uint tempPlayers;

    void Awake()
    {
        Matchtime = UIManager.FindObjectOfType<UIManager>();

        Shotclock = ShotClockManager.FindObjectOfType<ShotClockManager>();

        GameManager = GameManager.FindObjectOfType<GameManager>();


        DontDestroyOnLoad(this.gameObject);
    }
	// Use this for initialization
	void Start () {
        if (GameoptionsChanged == true)
        {
            Matchtime.startMatchTime = tempMatchTime;
            Shotclock.shotClockTime = tempShotClock;
            GameManager.TotalNumberofPlayers = tempPlayers;
        }
        else
        {
            tempMatchTime = Matchtime.startMatchTime;
            tempShotClock = Shotclock.shotClockTime;
            tempPlayers = GameManager.TotalNumberofPlayers;
        }


            

    }
	
	// Update is called once per frame
	void Update () {
        matchtimeText.text = tempMatchTime.ToString();
        ShottimeText.text = tempShotClock.ToString();
        numPlayersText.text = tempPlayers.ToString();

        if (GameManager.TotalNumberofPlayers == tempPlayers && tempShotClock == Shotclock.shotClockTime && tempMatchTime == Matchtime.startMatchTime)
        { GameoptionsChanged = false; }
        if (GameManager.TotalNumberofPlayers != tempPlayers || tempShotClock != Shotclock.shotClockTime || tempMatchTime != Matchtime.startMatchTime)
        { GameoptionsChanged = true; }

    }

    public void AddMatchClock()
    {
        if (Matchtime.startMatchTime >= 0f)
        { Matchtime.noTime = false; }
        tempMatchTime  += 1;
    }

    public void LowerMatchClock()
    {
        if (tempMatchTime <= 1f)
        { Matchtime.noTime = true; tempMatchTime = 0f; }
        else
        tempMatchTime -= 1f;
    }

    public void AddShotClock()
    {
        tempShotClock += 1;
    }

    public void LowerShotClock()
    {
        if (tempShotClock > 3f)
            tempShotClock -= 1;
    }

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

  public void restartCurrentScene()
      {
          int scene = SceneManager.GetActiveScene().buildIndex;
          SceneManager.LoadScene(scene, LoadSceneMode.Single);
      }

}

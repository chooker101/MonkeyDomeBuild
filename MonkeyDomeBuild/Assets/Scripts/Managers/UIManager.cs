using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Text p1Score;
    public Text p2Score;
    public Text p3Score;
    public Text matchTimeText;
    public bool noTime;
    public float matchTime;
    public Text debugLog;
    public float startMatchTime;

    public Text targetTierUI;
    public Text targetsInSequenceUI;

    public GameManager GameManager;

    public Text shotClockText;
    public Text shotClockPlayerText;
    public GameObject ShotClockInactive;
    public GameObject ShotClockActive;
    public GameObject ShotClockWarning;
    private bool playerHoldingBall = false;
    private bool loadedScene = false;
    public GameObject shotClock;
    public GameObject matchTimer;
    void Awake()
	{
        GameManager.Instance.gmGameOptionsManager.UIManager = this;
        if (FindObjectOfType<UIManager>() != GameManager.Instance.gmUIManager)
			GameManager.Instance.gmUIManager = FindObjectOfType<UIManager>();
	}

    void Start ()
    {
        //startMatchTime = GameManager.Instance.gmGameOptionsManager.MatchTime;
        matchTime = startMatchTime;
        shotClockPlayerText.text = "";
        SetToGameMode(GameManager.Instance.nextGameModeUI);
	}
	public void SetToGameMode(GameManager.GameMode gameMode)
    {
        //set ui to match gamemode requirement
        switch (gameMode)
        {
            case GameManager.GameMode.Keep_Away:

                break;
            case GameManager.GameMode.Battle_Royal:
                shotClock.SetActive(false);
                matchTimer.transform.localPosition = new Vector3(0, 0, 0);
                break;
        }
    }
	void LateUpdate ()
	{
        // Shot Clock Image States
        float time = GameManager.Instance.gmShotClockManager.ShotClockTime - GameManager.Instance.gmShotClockManager.ShotClockCount;
        if (time < 0)
        {
            AudioEffectManager.Instance.PlayShotClockBuzzSE();
            time = 0f;
        }

        for(int i = 0; i < GameManager.Instance.TotalNumberofActors; i++)
        {
            if (GameManager.Instance.gmPlayers[i].GetComponent<Actor>().IsHoldingBall)
            {
                if(GameManager.Instance.gmPlayers[i].GetComponent<Actor>().ballHolding.GetComponent<BallInfo>().BallType == ThrowableType.Ball)
                {
                    playerHoldingBall = true;
                    shotClockPlayerText.text = "P" + (i + 1).ToString();
                    break;
                }
            }
            else
            {
                playerHoldingBall = false;
            }
        }

        // Set the state of the shot clock image
        if (time > 4f && playerHoldingBall && !ShotClockActive.gameObject.activeSelf)
        {
            shotClockText.color = Color.black;
            ShotClockActive.gameObject.SetActive(true);
            ShotClockInactive.gameObject.SetActive(false);
            ShotClockWarning.gameObject.SetActive(false);
        }
        else if (time > 2f && time <= 4f && playerHoldingBall && !ShotClockActive.gameObject.activeSelf)
        {
            shotClockText.color = Color.black;
            ShotClockActive.gameObject.SetActive(true);
            ShotClockInactive.gameObject.SetActive(false);
            ShotClockWarning.gameObject.SetActive(false);
        }
        else if (time <= 2f && playerHoldingBall && !ShotClockWarning.gameObject.activeSelf)
        {
            shotClockText.color = Color.black;
            ShotClockActive.gameObject.SetActive(false);
            ShotClockInactive.gameObject.SetActive(false);
            ShotClockWarning.gameObject.SetActive(true);
        }
        else if (!playerHoldingBall && !ShotClockInactive.gameObject.activeSelf)
        {
            shotClockText.color = Color.black;
            ShotClockActive.gameObject.SetActive(false);
            ShotClockInactive.gameObject.SetActive(true);
            ShotClockWarning.gameObject.SetActive(false);
        }

        if (!noTime)
        {
            if (GameManager.Instance.playerCanMove)
            {
                if (matchTime > 0)
                {
                    matchTime -= Time.deltaTime;
                }
                else if (matchTime <= 0)
                {
                    matchTime = 0;
                    GameManager.Instance.gmRecordKeeper.scoreEndPlayers[0] = GameManager.Instance.gmScoringManager.p1Score;
                    GameManager.Instance.gmRecordKeeper.scoreEndPlayers[1] = GameManager.Instance.gmScoringManager.p2Score;
                    GameManager.Instance.gmRecordKeeper.scoreEndPlayers[2] = GameManager.Instance.gmScoringManager.p3Score;

                    for (int i = 0; i < GameManager.Instance.TotalNumberofActors; i++)
                    {
                        if (GameManager.Instance.gmPlayers[i].GetComponent<Actor>().characterType is Gorilla)
                        {
                            GameManager.Instance.gmPlayers[i].GetComponent<Actor>().characterType.Mutate();
                            GameManager.Instance.gmRecordKeeper.playerGorilla = -1;
                        }
                    }
                    GameManager.Instance.gmTrophyManager.CheckallWinners();
                    //Debug.Log(GameManager.Instance.gmTrophyManager.a);
                    if (!loadedScene)
                    {
                        loadedScene = true;
                        GameManager.Instance.LoadTrophyRoom();
                    }
                    //SceneManager.LoadScene("VictoryRoom");
                    //GameManager.Instance.SwitchRooms();
                }
                if (GameManager.Instance.nextGameModeUI == GameManager.GameMode.Battle_Royal)
                {
                    int activePlayer = 0;
                    for(int i = 0; i < GameManager.Instance.gmPlayerScripts.Count; i++)
                    {
                        if (!GameManager.Instance.gmPlayerScripts[i].IsDead)
                        {
                            activePlayer++;
                        }
                    }
                    if (activePlayer <= 1)
                    {
                        GameManager.Instance.gmTrophyManager.CheckallWinners();
                        if (!loadedScene)
                        {
                            loadedScene = true;
                            GameManager.Instance.LoadTrophyRoom();
                        }
                    }
                }
            }
        }

        p1Score.text =
            GameManager.Instance.gmScoringManager.p1Score.ToString();
            ;
        p2Score.text =
            GameManager.Instance.gmScoringManager.p2Score.ToString();
        ;
        p3Score.text =
            GameManager.Instance.gmScoringManager.p3Score.ToString();
        ;

        if (noTime == false)
        {
            matchTimeText.text = matchTime.ToString("F0");
        }
        else
            matchTimeText.text = "∞";

            debugLog.text = // Displays all debug info
            "PLAYER 1 STATS: \n" +
            "Mov: " + GameManager.Instance.gmPlayers[0].GetComponent<Actor>().movement + "\n\n" +
            "# Jumps: " + GameManager.Instance.gmPlayers[0].GetComponent<Actor>().stat_jump + "\n" +
            "# Ball Grabs: " + GameManager.Instance.gmPlayers[0].GetComponent<Actor>().stat_ballGrab + "\n" +
            "# Throws: " + GameManager.Instance.gmPlayers[0].GetComponent<Actor>().stat_throw + "\n\n"
            //"Targets Hit: " + ScoringManager.targetsHit;
            //"Audience Attitude: " + "\n" +
            //"Audience Target: " + "\n"
            ;

        TargetsUI();

    }
    void TargetsUI()
    {
        targetTierUI.text = GameManager.Instance.gmTargetManager.targetTier.ToString();
        targetsInSequenceUI.text = GameManager.Instance.gmTargetManager.HitSum.ToString();
    }

}

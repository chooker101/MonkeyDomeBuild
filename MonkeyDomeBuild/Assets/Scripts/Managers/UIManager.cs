using UnityEngine;
using System.Collections;
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
    public Text shotClock;
    public float startMatchTime;


    // Use this for initialization
    void Start ()
    {
        matchTime = startMatchTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (noTime == false)
        {
            if (matchTime > 0)
            {
                matchTime -= Time.fixedDeltaTime;
            }
            else if (matchTime <= 0)
            {
                matchTime = 0;
                //GameManager.Instance.gmRecordKeeper.scoreEndPlayers[0] = GameManager.Instance.gmScoringManager.p1Score;
				//GameManager.Instance.gmRecordKeeper.scoreEndPlayers[1] = GameManager.Instance.gmScoringManager.p2Score;
				//GameManager.Instance.gmRecordKeeper.scoreEndPlayers[2] = GameManager.Instance.gmScoringManager.p3Score;
                SceneManager.LoadScene("PregameRoom");
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
            matchTimeText.text = // time of the match left
       matchTime.ToString("F2");
        }
        else
            matchTimeText.text = "∞";

            debugLog.text = // Displays all debug info
            "PLAYER 1 STATS: \n" +
            "Mov: " + GameManager.Instance.gmPlayers[0].GetComponent<Player>().movement + "\n\n" +
            "# Jumps: " + GameManager.Instance.gmPlayers[0].GetComponent<Player>().stat_jump + "\n" +
            "# Ball Grabs: " + GameManager.Instance.gmPlayers[0].GetComponent<Player>().stat_ballGrab + "\n" +
            "# Throws: " + GameManager.Instance.gmPlayers[0].GetComponent<Player>().stat_throw + "\n\n"
            //"Targets Hit: " + ScoringManager.targetsHit;
            //"Audience Attitude: " + "\n" +
            //"Audience Target: " + "\n"
            ;
    }
}

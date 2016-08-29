using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreGameTimer : MonoBehaviour
{

    public Text timerText;
    public float pregameTimer;
    public float trophyRoomTimer;
    public GameObject spinner;
    GameObject newSpinner;
    static string gameState = "null";

    private bool spinnerSpawned = false;
    private RecordKeeper rk_keeper;

    // Use this for initialization
    void Start()
    {
        rk_keeper = FindObjectOfType<RecordKeeper>().GetComponent<RecordKeeper>();

        if(gameState == "null")
        {
            gameState = "pregame";
        }
        else if(gameState == "game")
        {
            gameState = "trophyroom";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == "pregame")
        {
            if (!spinnerSpawned && pregameTimer <= 10)
            {
                newSpinner = (GameObject)Instantiate(spinner,spinner.transform.position,spinner.transform.rotation);
                spinnerSpawned = true;
            }
            if (pregameTimer > 0)
            {
                pregameTimer -= Time.deltaTime;
            }
            else if (pregameTimer <= 0)
            {
                pregameTimer = 0;
                gameState = "game";
                SceneManager.LoadScene("testingroom");
                Destroy(newSpinner, 1f);
            }

            timerText.text = "Pre-game Room\n" + pregameTimer.ToString("F2");
        }
        else if(gameState == "trophyroom")
        {
            if (trophyRoomTimer > 0)
            {
                trophyRoomTimer -= Time.deltaTime;
            }
            else if (trophyRoomTimer <= 0)
            {
                trophyRoomTimer = 0;
                gameState = "pregame";

                for(int i = 0; i<rk_keeper.scoreEndPlayers.Length; i++)
                {
                    rk_keeper.scoreEndPlayers[i] = 0;
                }
            }
            timerText.text =
                (
                    "Trophy Room\n" + trophyRoomTimer.ToString("F2") + 
                    "FINAL SCORES:\n" +
                    "P1 - " + rk_keeper.scoreEndPlayers[0] + 
                    "\nP2 - " + rk_keeper.scoreEndPlayers[1] + 
                    "\nP3 - " + rk_keeper.scoreEndPlayers[2]
                );
        }
    }
    public static void ChangeGameState(string state)
    {
        gameState = state;
    }
}

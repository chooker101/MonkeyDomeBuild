using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreGameTimer : MonoBehaviour
{

    public Text timerText;
    public float pregameTimer;
    public float trophyRoomTimer;
    public GameObject spinner;
    GameObject newSpinner;
    static string gameState = "null";

    bool spinnerSpawned = false;

    // Use this for initialization
    void Start()
    {
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
            if (!spinnerSpawned)
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
                Application.LoadLevel("testingroom");
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
            }
            timerText.text = "Trophy Room\n" + trophyRoomTimer.ToString("F2");
        }
    }
    public static void ChangeGameState(string state)
    {
        gameState = state;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreGameTimer : MonoBehaviour
{

    Text text;
    public float pregameTimer;
    public float trophyRoomTimer;
    static string gameState = "null";

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
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
            if (pregameTimer > 0)
            {
                pregameTimer -= Time.deltaTime;
            }
            else if (pregameTimer <= 0)
            {
                pregameTimer = 0;
                gameState = "game";
                Application.LoadLevel("testingroom");
            }

            text.text = "Pre-game Room\n" + pregameTimer.ToString("F2");
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
            text.text = "Trophy Room\n" + trophyRoomTimer.ToString("F2");
        }
    }
    public static void ChangeGameState(string state)
    {
        gameState = state;
    }
}

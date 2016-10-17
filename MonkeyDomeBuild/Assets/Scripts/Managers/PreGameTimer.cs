using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreGameTimer : MonoBehaviour
{
    public bool debugStartMatch = false;
    public Text timerText;
    public Text gorillaSmashText;
    //public float pregameTimer;
    public float trophyRoomTimer;
    public GameObject spinner;
    public Material resetMaterial;

    GameObject newSpinner;
    static string gameState = "null";

    private bool spinnerSpawned = false;
    public bool gorillaSmashed = false;
    private GameObject[] colourTargets;

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
        colourTargets = GameObject.FindGameObjectsWithTag("ColourTarget");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            debugStartMatch = true;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            gorillaSmashed = true;
        }
        // PREGAME ROOM
        if(gameState == "pregame")
        {
            // spawns a spinner that chooses a player to be a gorilla once all targets are hit.
            if (!spinnerSpawned && (AllTargetsHit()||debugStartMatch)) 
            {
                newSpinner = (GameObject)Instantiate(spinner,spinner.transform.position,spinner.transform.rotation);
                spinnerSpawned = true;
            }
            // Sets the Gorilla Smash text once the gorilla has been chosen
            if (spinnerSpawned && newSpinner != null)
            {
                if (newSpinner.GetComponent<ApeSpinner>().setGorilla)
                {
                    gorillaSmashText.text = "Gorilla Smash!";
                }
                else
                {
                    gorillaSmashText.text = "";
                }
            }

            /*
            if (pregameTimer > 0 && !gorillaSmashed)
            {
                pregameTimer -= Time.deltaTime;
            }*/
            if(newSpinner != null)
            {
                if (debugStartMatch)
                {
                    //gorillaSmashed = true;
                }
                if (newSpinner.GetComponent<ApeSpinner>().setGorilla && gorillaSmashed)
                {
                    //pregameTimer = 0;
                    gameState = "game";
                    SceneManager.LoadScene("mb_level02_v2");
                    Destroy(newSpinner, 1f);
                }
            }

            timerText.text = "Pre-game Room\n"; //+ pregameTimer.ToString("F2");
        }
        // TROPHY ROOM
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

                for(int i = 0; i< GameManager.Instance.GetComponent<RecordKeeper>().scoreEndPlayers.Length; i++)
                {
                    GameManager.Instance.GetComponent<RecordKeeper>().scoreEndPlayers[i] = 0;
                    GameManager.Instance.gmScoringManager.p1Score = 0;
                    GameManager.Instance.gmScoringManager.p2Score = 0;
                    GameManager.Instance.gmScoringManager.p3Score = 0;

                }
                for(int i = 0; i < GameManager.Instance.gmRecordKeeper.colourPlayers.Length; i++)
                {
                    GameManager.Instance.gmRecordKeeper.colourPlayers[i] = resetMaterial;
                }
            }
            timerText.text =
                (
                    "Trophy Room\n" + trophyRoomTimer.ToString("F2") + 
                    "\nFINAL SCORES:\n" +
                    "P1 - " + GameManager.Instance.GetComponent<RecordKeeper>().scoreEndPlayers[0] + 
                    "\nP2 - " + GameManager.Instance.GetComponent<RecordKeeper>().scoreEndPlayers[1] + 
                    "\nP3 - " + GameManager.Instance.GetComponent<RecordKeeper>().scoreEndPlayers[2]
                );
        }
    }

    public static void ChangeGameState(string state)
    {
        gameState = state;
    }

    private bool AllTargetsHit()
    {
        if (GameManager.Instance.TotalNumberofPlayers >= 3)
        {
            for (int i = 0; i < colourTargets.Length; i++)
            {
                // Checks to see if any targets aren't hit. Is any aren't, returns false.
                if (colourTargets[i].GetComponent<ColourChanger>().IsActivated)
                {
                    if (!colourTargets[i].GetComponent<ColourChanger>().isHit)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;

    }
}

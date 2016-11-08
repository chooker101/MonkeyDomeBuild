using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreGameTimer : MonoBehaviour
{
    public Object gameScene;
    public bool debugStartMatch = false;
    public Text timerText;
    public Text gorillaSmashText;
    //public float pregameTimer;
    public float trophyRoomTimer;
    public GameObject spinner;
    public Material resetMaterial;
    public GameObject sign_catch;
    public GameObject sign_jump;
    public GameObject sign_callForBall;
    public GameObject sign_gorillaSmash;
    public GameObject sign_climbVines;
    public GameObject sign_throw;
    public GameObject sign_throwLong;

    GameObject newSpinner;
    static string gameState = "null";

    private bool spinnerSpawned = false;
    public bool gorillaSet = false;
    public bool gorillaSmashed = false;
    private GameObject[] colourTargets;
    private bool loadedScene = false;

    // Use this for initialization
    void Start()
    {
        if(gameState == "null")
        {
            gameState = "pregame";
            sign_catch.SetActive(true);
            sign_jump.SetActive(true);
            sign_callForBall.SetActive(true);
            sign_climbVines.SetActive(true);
            sign_throw.SetActive(true);
            sign_throwLong.SetActive(true);

            sign_gorillaSmash.SetActive(false);

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
            if(!AllTargetsHit() && !sign_catch.activeSelf)
            {
                sign_catch.SetActive(true);
                sign_jump.SetActive(true);
                sign_callForBall.SetActive(true);
                sign_climbVines.SetActive(true);
                sign_throw.SetActive(true);
                sign_throwLong.SetActive(true);

                sign_gorillaSmash.SetActive(false);
            }
            else if(AllTargetsHit() && spinnerSpawned)
            {
                if (!gorillaSet && sign_catch.activeSelf)
                {
                    sign_catch.SetActive(false);
                    sign_jump.SetActive(false);
                    sign_callForBall.SetActive(false);
                    sign_climbVines.SetActive(false);
                    sign_throw.SetActive(false);
                    sign_throwLong.SetActive(false);
                }
                else if (gorillaSet && !sign_gorillaSmash.activeSelf)
                {
                    sign_gorillaSmash.SetActive(true);
                }
            }

            // spawns a spinner that chooses a player to be a gorilla once all targets are hit.
            if (!spinnerSpawned && (AllTargetsHit()||debugStartMatch)) 
            {
                newSpinner = (GameObject)Instantiate(spinner,spinner.transform.position,spinner.transform.rotation);
                spinnerSpawned = true;
            }
            // Sets the Gorilla Smash text once the gorilla has been chosen
            if (spinnerSpawned && newSpinner != null)
            {
                if (gorillaSet)
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

            if (debugStartMatch)
            {
                //gorillaSmashed = true;
            }
            if (gorillaSet && gorillaSmashed && !loadedScene)
            {
                //pregameTimer = 0;
                loadedScene = true;
                gameState = "game";
                GameManager.Instance.StartMatch();
                //StartCoroutine(LoadScene());
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
                for(int i = 0; i < GameManager.Instance.gmRecordKeeper.colourPlayers.Count; i++)
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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum GameState
{
    Null,
    Pregame_PickingColours,
    Pregame_SpinnerSpinning,
    Pregame_GorillaSet,
    Game_Start
}
public enum TutorialState
{
    Jump,
    Catch,
    Climb
}
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
    public GameState gameState = GameState.Null;
    public TutorialState tutorialState = TutorialState.Jump;
    public bool gorillaSet = false;
    public bool gorillaSmashed = false;
    private List<int> monkeysActions = new List<int>();
    public bool allMonkeysJumped = false;
    public bool allMonkeysCatchBall = false;
    public bool allMonkeysClimbed = false;
    public bool allMonkeysThrew = false;
    public bool allMonkeysCalled = false;

    private bool spinnerSpawned = false;
    private GameObject[] colourTargets;
    private bool loadedScene = false;

    // Use this for initialization
    void Start()
    {
        gameState = GameState.Pregame_PickingColours;
        sign_catch.SetActive(false);
        sign_jump.SetActive(true);
        sign_callForBall.SetActive(true);
        sign_climbVines.SetActive(false);
        sign_throw.SetActive(true);
        sign_throwLong.SetActive(true);

        sign_gorillaSmash.SetActive(false);
        
        colourTargets = GameObject.FindGameObjectsWithTag("ColourTarget");
        for(int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
        {
            //monkeysJumped.Add
        }
        StartCoroutine(TutorialSequence());
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

        // Check to see what signs come up
        //if (!monkeysJumped)
        //{
            //for(int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
            //{

            //}
        //}

        // Check the game state to set
        if (!AllTargetsHit()) { gameState = GameState.Pregame_PickingColours; }
        else if(AllTargetsHit() && spinnerSpawned && !gorillaSet) { gameState = GameState.Pregame_SpinnerSpinning; }
        else if(AllTargetsHit() && spinnerSpawned && gorillaSet) { gameState = GameState.Pregame_GorillaSet; }

        // Defines actions for the room based on the game state
        /*if(gameState == GameState.Pregame_PickingColours)
        {
            if(!sign_catch.activeSelf)
            {
                sign_catch.SetActive(true);
                sign_jump.SetActive(true);
                sign_callForBall.SetActive(true);
                sign_climbVines.SetActive(true);
                sign_throw.SetActive(true);
                sign_throwLong.SetActive(true);

                sign_gorillaSmash.SetActive(false);

                for (int i = 0; i < colourTargets.Length; i++)
                {
                    Transform targetPivot = colourTargets[i].transform.FindChild("Pivot");
                    targetPivot.gameObject.SetActive(true);
                }
            }
        }
        else if(gameState == GameState.Pregame_SpinnerSpinning)
        {
            if(sign_catch.activeSelf)
            {
                sign_catch.SetActive(false);
                sign_jump.SetActive(false);
                sign_callForBall.SetActive(false);
                sign_climbVines.SetActive(false);
                sign_throw.SetActive(false);
                sign_throwLong.SetActive(false);

                for (int i = 0; i < colourTargets.Length; i++)
                {
                    Transform targetPivot = colourTargets[i].transform.FindChild("Pivot");
                    targetPivot.gameObject.SetActive(false);
                }
            }
        }
        else if (gameState == GameState.Pregame_GorillaSet)
        {
            if(!sign_gorillaSmash.activeSelf)
            {
                sign_gorillaSmash.SetActive(true);
            }
        }*/

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
            gameState = GameState.Game_Start;
            GameManager.Instance.StartMatch();
        }

        timerText.text = "Pre-game Room\n"; //+ pregameTimer.ToString("F2");
        
        /*
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
        }*/
    }

    public void ChangeGameState(GameState state)
    {
        gameState = state;
    }

    public bool AllTargetsHit()
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
    IEnumerator TutorialSequence()
    {
        while (!sign_catch.activeInHierarchy)
        {
            if (monkeysActions.Count >= GameManager.Instance.TotalNumberofPlayers)
            {
                sign_catch.SetActive(true);
                monkeysActions.Clear();
                tutorialState = TutorialState.Catch;
            }
            yield return null;
        }
        yield return null;
        while (!sign_climbVines.activeInHierarchy)
        {
            if (monkeysActions.Count >= GameManager.Instance.TotalNumberofPlayers)
            {
                sign_climbVines.SetActive(true);
                monkeysActions.Clear();
            }
            yield return null;
        }
        yield return null;
    }
    public void AddMonkeyAction()
    {
        monkeysActions.Add(0);
    }
}

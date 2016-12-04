using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum GameState
{
    Null,
    Pregame_PickingColours,
    Pregame_allTargetsHit,
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
    public GameObject spinner;
    public Material resetMaterial;
    public GameObject sign_catch;
    public GameObject sign_jump;
    public GameObject sign_callForBall;
    public GameObject sign_gorillaSmash;
    public GameObject sign_climbVines;
    public GameObject sign_throw;
    public GameObject sign_throwLong;
    public GameObject sign_buttons;
    public GameObject sign_chooseColour;
    public GameState gameState = GameState.Null;
    public TutorialState tutorialState = TutorialState.Jump;
    public bool gorillaSet = false;
    public bool gorillaSmashed = false;
    public int gorillaSmashes = 0;
    public bool allMonkeysJumped = false;
    public bool allMonkeysCatchBall = false;
    public bool allMonkeysClimbed = false;
    public bool allMonkeysThrew = false;
    public bool allMonkeysCalled = false;
    public float allTargetsHitTimer;

    private GameObject ballReturn;
    private List<int> monkeysActions = new List<int>();
    private GameObject newSpinner;
    private bool spinnerSpawned = false;
    private GameObject[] colourTargets;
    private bool loadedScene = false;

    // Use this for initialization
    void Start()
    {
        gameState = GameState.Pregame_PickingColours;
        sign_catch.SetActive(false);
        sign_jump.SetActive(false);
        sign_callForBall.SetActive(false);
        sign_climbVines.SetActive(false);
        sign_throw.SetActive(false);
        sign_throwLong.SetActive(false);
        sign_buttons.SetActive(true);
        sign_chooseColour.SetActive(true);

        sign_gorillaSmash.SetActive(false);

        ballReturn = FindObjectOfType<BallReturn>().gameObject;
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
        if(gorillaSmashes >= 3)
        {
            gorillaSmashed = true;
        }

        // Check the game state to set
        if (!AllTargetsHit()) { gameState = GameState.Pregame_PickingColours; }
        else if(AllTargetsHit() && !spinnerSpawned)
        {
            gameState = GameState.Pregame_allTargetsHit;
        }
        else if(AllTargetsHit() && spinnerSpawned && !gorillaSet)
        {
            gameState = GameState.Pregame_SpinnerSpinning;
        }
        else if(AllTargetsHit() && spinnerSpawned && gorillaSet) { gameState = GameState.Pregame_GorillaSet; }

        // Defines actions for the room based on the game state
        if(gameState == GameState.Pregame_PickingColours)
        {
            if(!sign_catch.activeSelf)
            {
                //sign_catch.SetActive(true);
                //sign_jump.SetActive(true);
                //sign_callForBall.SetActive(true);
                //sign_climbVines.SetActive(true);
                //sign_throw.SetActive(true);
                //sign_throwLong.SetActive(true);
                sign_buttons.SetActive(true);
                sign_chooseColour.SetActive(true);

                sign_gorillaSmash.SetActive(false);
                /*
                for (int i = 0; i < colourTargets.Length; i++)
                {
                    Transform targetPivot = colourTargets[i].transform.FindChild("Pivot");
                    targetPivot.gameObject.SetActive(true);
                }*/
            }
        }
        else if (gameState == GameState.Pregame_allTargetsHit)
        {
            if (GameManager.Instance.nextGameModeUI == GameManager.GameMode.Keep_Away)
            {
                if (sign_chooseColour.activeSelf)
                {
                    sign_catch.SetActive(false);
                    sign_jump.SetActive(false);
                    sign_callForBall.SetActive(false);
                    sign_climbVines.SetActive(false);
                    sign_throw.SetActive(false);
                    sign_throwLong.SetActive(false);
                    sign_buttons.SetActive(false);
                    sign_chooseColour.SetActive(false);

                    ballReturn.GetComponent<BallReturn>().checkEnable();
                    ballReturn.SetActive(false);
                    for (int i = 0; i < colourTargets.Length; i++)
                    {
                        Transform targetPivot = colourTargets[i].transform.FindChild("Pivot");
                        targetPivot.gameObject.SetActive(false);
                    }
                }
            }
            else if(GameManager.Instance.nextGameModeUI == GameManager.GameMode.Battle_Royal)
            {
                if (!loadedScene)
                {
                    loadedScene = true;
                    gameState = GameState.Game_Start;
                    GameManager.Instance.StartMatch();
                }
            }

        }
        else if(gameState == GameState.Pregame_SpinnerSpinning)
        {
            
        }
        else if (gameState == GameState.Pregame_GorillaSet)
        {
            if (!sign_gorillaSmash.activeSelf)
            {
                sign_gorillaSmash.SetActive(true);
            }
        }

        // spawns a spinner that chooses a player to be a gorilla once all targets are hit.
        if (!spinnerSpawned && (AllTargetsHit() || debugStartMatch) && GameManager.Instance.nextGameModeUI == GameManager.GameMode.Keep_Away) 
        {
            allTargetsHitTimer -= Time.deltaTime;

            if(allTargetsHitTimer <= 0)
            {
                allTargetsHitTimer = 0;
                newSpinner = (GameObject)Instantiate(spinner, spinner.transform.position, spinner.transform.rotation);
                spinnerSpawned = true;
            }
        }

        /*if (debugStartMatch)
        {
            gorillaSmashed = true;
        }*/
        if (gorillaSet && gorillaSmashed && !loadedScene)
        {
            loadedScene = true;
            gameState = GameState.Game_Start;
            GameManager.Instance.StartMatch();
        }
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
        /*
        while (!sign_catch.activeInHierarchy)
        {
            if (monkeysActions.Count >= GameManager.Instance.TotalNumberofPlayers)
            {
                //sign_catch.SetActive(true);
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
        }*/
        yield return null;
        
    }
    public void AddMonkeyAction()
    {
        monkeysActions.Add(0);
    }
}

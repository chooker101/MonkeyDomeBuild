using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum PN
    {
        P1, //=0
        P2,
        P3,
        P4,
        P5,
        Length
    }

	public enum Scene : int
	{
		mb_level04_v2,
		stuff,

		Length
	}
	public enum GameMode : int
	{
		Keep_Away,
		Battle_Royal,

		Length
	}

	public uint TotalNumberofActors; //Number of Actors
    [SerializeField]
    private uint NumberOfPlayersToBuild = 1; //Number of Players
    [SerializeField]
    private uint NumberOfBotsToBuild = 0; //Number of AI
    [SerializeField]
    private bool RandomGorilla = true;
    [SerializeField]
    private int PlayerGorilla;

	public uint NumberOfPlayers;


    private static GameManager s_Instance;
    public static GameManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<GameManager>();
            }
            return s_Instance;
        }
    }
    public List<GameObject> gmPlayers;
    public List<Actor> gmPlayerScripts;
    public SpawnManager gmSpawnManager;
    public GameObject gmSpawnObject;
    public ButtonManager gmButtonManager;
    public GameObject gmPlayerPrefab;
    public GameObject gmPlayerPrefabAI;
    public List<GameObject> gmBalls; // 0 IS ALWAYS MAIN BALL GMBALL
    public UIManager gmUIManager;
    public List<InputManager> gmInputs;
    public RecordKeeper gmRecordKeeper;
    public MovementManager gmMovementManager;
    public ScoringManager gmScoringManager;
    public TargetManager gmTargetManager;
    public ShotClockManager gmShotClockManager;
    public TrophyManager gmTrophyManager;
    public AudienceManager gmAudienceManager;
    public AudienceAnimator gmAudienceAnimator;
    public LevelObjectsScript gmLevelObjectsScript;
    public GameOptionsManager gmGameOptionsManager;
    public VolumeManager gmVolumeManager;
    public PauseManager gmPauseManager;
    public TurretsManager gmTurretManager;
    public CurtainsController gmCurtainController;
    public BallColorManager gmBallColorManager;
	public Vector2 allDpad;
    public string nextRoom;
    public bool playerCanMove = true;
    protected bool nextIsMatch = false;
	public Scene nextSceneUI;
	public GameMode nextGameModeUI;
	public int numOfBotsUI;

	bool displayController1Name;
    bool displayController2Name;
    bool displayController3Name;
    bool displayController4Name;
    bool displayController5Name;

    [SerializeField]
    private List<int> InputIndecies;


    public bool noGorilla = false;

    private GameManager() { }

    void Start()
    {
        if (s_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            TotalNumberofActors = NumberOfPlayersToBuild + NumberOfBotsToBuild;
			NumberOfPlayers = NumberOfPlayersToBuild;
			for (int i = 0; i < 5;i++)
			{
				InputIndecies[i] = -1;
			}
			gmPlayers.Clear();
			gmPlayerScripts.Clear();
			CreateInputs();
            CreatePlayers();
        }
		gmPlayers.TrimExcess();
		gmPlayerScripts.TrimExcess();
		gmBalls.Clear();
    }

    void Update()
    {
        if (playerCanMove)
        {
            UpdateInputs();
        }

    }

    public void CreateInputs()
    {
        for (int i = 0; i < 5; ++i)
        {
            Instance.gmInputs[i] = (InputManager)ScriptableObject.CreateInstance("InputManager");
        }
    }

    public void UpdateInputs()
    {
		allDpad.x = Input.GetAxis("pX_dpad_x");
		allDpad.y = Input.GetAxis("pX_dpad_y");
		if (allDpad == Vector2.zero)
		{
			//use ps input
		}
		

        if (Instance.gmPlayerScripts[(int)PN.P1].isPlayer)
        {
			if (Input.GetJoystickNames().Length >= 1)
			{
				if(Input.GetJoystickNames()[0] != null)
				{
					Instance.gmInputs[(int)PN.P1].mXY.x = Input.GetAxis("p1_joy_x");
					Instance.gmInputs[(int)PN.P1].mXY.y = -Input.GetAxis("p1_joy_y");
					Instance.gmInputs[(int)PN.P1].mAimStomp = Input.GetButtonDown("p1_aim/stomp");
					Instance.gmInputs[(int)PN.P1].mChargeStomp = Input.GetButton("p1_aim/stomp");

					if (Input.GetJoystickNames()[0] == "Wireless Controller")
					{
						Instance.gmInputs[(int)PN.P1].mJump = Input.GetButtonDown("p1_ps4_jump");
						Instance.gmInputs[(int)PN.P1].mCatch = Input.GetButtonDown("p1_ps4_catch/throw");
						Instance.gmInputs[(int)PN.P1].mChargeThrow = Input.GetButton("p1_ps4_catch/throw");
						Instance.gmInputs[(int)PN.P1].mCatchRelease = Input.GetButtonUp("p1_ps4_catch/throw");
						Instance.gmInputs[(int)PN.P1].mStart = Input.GetButtonDown("p1_ps4_start");
					}
					else if (Input.GetJoystickNames()[0] == "XBOX 360 For Windows (Controller)")
					{
						Instance.gmInputs[(int)PN.P1].mJump = Input.GetButtonDown("p1_jump");
						Instance.gmInputs[(int)PN.P1].mCatch = Input.GetButtonDown("p1_catch/throw");
						Instance.gmInputs[(int)PN.P1].mChargeThrow = Input.GetButton("p1_catch/throw");
						Instance.gmInputs[(int)PN.P1].mCatchRelease = Input.GetButtonUp("p1_catch/throw");
						Instance.gmInputs[(int)PN.P1].mStart = Input.GetButtonDown("p1_start");
					}
                    else
                    {
                        Instance.gmInputs[(int)PN.P1].mJump = Input.GetButtonDown("p1_jump");
                        Instance.gmInputs[(int)PN.P1].mCatch = Input.GetButtonDown("p1_catch/throw");
                        Instance.gmInputs[(int)PN.P1].mChargeThrow = Input.GetButton("p1_catch/throw");
                        Instance.gmInputs[(int)PN.P1].mCatchRelease = Input.GetButtonUp("p1_catch/throw");
                        Instance.gmInputs[(int)PN.P1].mStart = Input.GetButtonDown("p1_start");
                    }
				}
				else
				{
					if (Input.GetJoystickNames()[0] == "Wireless Controller")
						Instance.gmInputs[(int)PN.P1].mJump = Input.GetButtonDown("p1_ps4_jump");
					else if (Input.GetJoystickNames()[0] == "XBOX 360 For Windows (Controller)")
						Instance.gmInputs[(int)PN.P1].mJump = Input.GetButtonDown("p1_jump");
				}
			}
        }

 
			if (Input.GetJoystickNames().Length >= 2)
			{
				if (Input.GetJoystickNames()[1] != null)
				{
					if (Instance.gmPlayerScripts[InputIndecies[(int)PN.P2]].isPlayer)
					{
						Instance.gmInputs[(int)PN.P2].mXY.x = Input.GetAxis("p2_joy_x");
						Instance.gmInputs[(int)PN.P2].mXY.y = -Input.GetAxis("p2_joy_y");
						Instance.gmInputs[(int)PN.P2].mAimStomp = Input.GetButtonDown("p2_aim/stomp");
						Instance.gmInputs[(int)PN.P2].mChargeStomp = Input.GetButton("p2_aim/stomp");

						if (Input.GetJoystickNames()[1] == "Wireless Controller")
						{
							Instance.gmInputs[(int)PN.P2].mJump = Input.GetButtonDown("p2_ps4_jump");
							Instance.gmInputs[(int)PN.P2].mCatch = Input.GetButtonDown("p2_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P2].mChargeThrow = Input.GetButton("p2_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P2].mCatchRelease = Input.GetButtonUp("p2_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P2].mStart = Input.GetButtonDown("p2_ps4_start");
						}
						else if (Input.GetJoystickNames()[1] == "XBOX 360 For Windows (Controller)")
						{
							Instance.gmInputs[(int)PN.P2].mJump = Input.GetButtonDown("p2_jump");
							Instance.gmInputs[(int)PN.P2].mCatch = Input.GetButtonDown("p2_catch/throw");
							Instance.gmInputs[(int)PN.P2].mChargeThrow = Input.GetButton("p2_catch/throw");
							Instance.gmInputs[(int)PN.P2].mCatchRelease = Input.GetButtonUp("p2_catch/throw");
							Instance.gmInputs[(int)PN.P2].mStart = Input.GetButtonDown("p2_start");
						}
                        else
                        {
                            Instance.gmInputs[(int)PN.P2].mJump = Input.GetButtonDown("p2_jump");
                            Instance.gmInputs[(int)PN.P2].mCatch = Input.GetButtonDown("p2_catch/throw");
                            Instance.gmInputs[(int)PN.P2].mChargeThrow = Input.GetButton("p2_catch/throw");
                            Instance.gmInputs[(int)PN.P2].mCatchRelease = Input.GetButtonUp("p2_catch/throw");
                            Instance.gmInputs[(int)PN.P2].mStart = Input.GetButtonDown("p2_start");
                        }
					}
					else
					{
						if (Input.GetJoystickNames()[1] == "Wireless Controller")
							Instance.gmInputs[(int)PN.P2].mJump = Input.GetButtonDown("p2_ps4_jump");
						else if (Input.GetJoystickNames()[1] == "XBOX 360 For Windows (Controller)")
							Instance.gmInputs[(int)PN.P2].mJump = Input.GetButtonDown("p2_jump");
					}
				}
            }
		
		if (Input.GetJoystickNames().Length >= 3)
			{
				if (Input.GetJoystickNames()[2] != null)
				{
					if (Instance.gmPlayerScripts[InputIndecies[(int)PN.P3]].isPlayer)
					{
						Instance.gmInputs[(int)PN.P3].mXY.x = Input.GetAxis("p3_joy_x");
						Instance.gmInputs[(int)PN.P3].mXY.y = -Input.GetAxis("p3_joy_y");
						Instance.gmInputs[(int)PN.P3].mAimStomp = Input.GetButtonDown("p3_aim/stomp");
						Instance.gmInputs[(int)PN.P3].mChargeStomp = Input.GetButton("p3_aim/stomp");

						if (Input.GetJoystickNames()[2] == "Wireless Controller")
						{
							Instance.gmInputs[(int)PN.P3].mJump = Input.GetButtonDown("p3_ps4_jump");
							Instance.gmInputs[(int)PN.P3].mCatch = Input.GetButtonDown("p3_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P3].mChargeThrow = Input.GetButton("p3_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P3].mCatchRelease = Input.GetButtonUp("p3_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P3].mStart = Input.GetButtonDown("p3_ps4_start");
						}
						else if (Input.GetJoystickNames()[2] == "XBOX 360 For Windows (Controller)")
						{
							Instance.gmInputs[(int)PN.P3].mJump = Input.GetButtonDown("p3_jump");
							Instance.gmInputs[(int)PN.P3].mCatch = Input.GetButtonDown("p3_catch/throw");
							Instance.gmInputs[(int)PN.P3].mChargeThrow = Input.GetButton("p3_catch/throw");
							Instance.gmInputs[(int)PN.P3].mCatchRelease = Input.GetButtonUp("p3_catch/throw");
							Instance.gmInputs[(int)PN.P3].mStart = Input.GetButtonDown("p3_start");
						}
                        else
                        {
                            Instance.gmInputs[(int)PN.P3].mJump = Input.GetButtonDown("p3_jump");
                            Instance.gmInputs[(int)PN.P3].mCatch = Input.GetButtonDown("p3_catch/throw");
                            Instance.gmInputs[(int)PN.P3].mChargeThrow = Input.GetButton("p3_catch/throw");
                            Instance.gmInputs[(int)PN.P3].mCatchRelease = Input.GetButtonUp("p3_catch/throw");
                            Instance.gmInputs[(int)PN.P3].mStart = Input.GetButtonDown("p3_start");
                        }
					}
					else
					{
						if (Input.GetJoystickNames()[2] == "Wireless Controller")
							Instance.gmInputs[(int)PN.P3].mJump = Input.GetButtonDown("p3_ps4_jump");
						else if (Input.GetJoystickNames()[2] == "XBOX 360 For Windows (Controller)")
							Instance.gmInputs[(int)PN.P3].mJump = Input.GetButtonDown("p3_jump");
					}
				}
            }
       
			if (Input.GetJoystickNames().Length >= 4)
			{
				if (Input.GetJoystickNames()[3] != null)
				{
					if (Instance.gmPlayerScripts[InputIndecies[(int)PN.P4]].isPlayer)
					{
						Instance.gmInputs[(int)PN.P4].mXY.x = Input.GetAxis("p4_joy_x");
						Instance.gmInputs[(int)PN.P4].mXY.y = -Input.GetAxis("p4_joy_y");
						Instance.gmInputs[(int)PN.P4].mAimStomp = Input.GetButtonDown("p4_aim/stomp");
						Instance.gmInputs[(int)PN.P4].mChargeStomp = Input.GetButton("p4_aim/stomp");

						if (Input.GetJoystickNames()[3] == "Wireless Controller")
						{
							Instance.gmInputs[(int)PN.P4].mJump = Input.GetButtonDown("p4_ps4_jump");
							Instance.gmInputs[(int)PN.P4].mCatch = Input.GetButtonDown("p4_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P4].mChargeThrow = Input.GetButton("p4_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P4].mCatchRelease = Input.GetButtonUp("p4_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P4].mStart = Input.GetButtonDown("p4_ps4_start");
						}
						else if (Input.GetJoystickNames()[3] == "XBOX 360 For Windows (Controller)")
						{
							Instance.gmInputs[(int)PN.P4].mJump = Input.GetButtonDown("p4_jump");
							Instance.gmInputs[(int)PN.P4].mCatch = Input.GetButtonDown("p4_catch/throw");
							Instance.gmInputs[(int)PN.P4].mChargeThrow = Input.GetButton("p4_catch/throw");
							Instance.gmInputs[(int)PN.P4].mCatchRelease = Input.GetButtonUp("p4_catch/throw");
							Instance.gmInputs[(int)PN.P4].mStart = Input.GetButtonDown("p4_start");
						}
                        else
                        {
                            Instance.gmInputs[(int)PN.P4].mJump = Input.GetButtonDown("p4_jump");
                            Instance.gmInputs[(int)PN.P4].mCatch = Input.GetButtonDown("p4_catch/throw");
                            Instance.gmInputs[(int)PN.P4].mChargeThrow = Input.GetButton("p4_catch/throw");
                            Instance.gmInputs[(int)PN.P4].mCatchRelease = Input.GetButtonUp("p4_catch/throw");
                            Instance.gmInputs[(int)PN.P4].mStart = Input.GetButtonDown("p4_start");
                        }
					}
					else
					{
						if (Input.GetJoystickNames()[3] == "Wireless Controller")
							Instance.gmInputs[(int)PN.P4].mJump = Input.GetButtonDown("p4_ps4_jump");
						else if (Input.GetJoystickNames()[3] == " XBOX 360 For Windows (Controller)")
							Instance.gmInputs[(int)PN.P4].mJump = Input.GetButtonDown("p4_jump");
					}
				}
            }
       
			if (Input.GetJoystickNames().Length >= 5)
			{
				if (Input.GetJoystickNames()[4] != null)
				{

					if (Instance.gmPlayerScripts[InputIndecies[(int)PN.P5]].isPlayer)
					{
						Instance.gmInputs[(int)PN.P5].mXY.x = Input.GetAxis("p5_joy_x");
						Instance.gmInputs[(int)PN.P5].mXY.y = -Input.GetAxis("p5_joy_y");
						Instance.gmInputs[(int)PN.P5].mAimStomp = Input.GetButtonDown("p5_aim/stomp");
						Instance.gmInputs[(int)PN.P5].mChargeStomp = Input.GetButton("p5_aim/stomp");

						if (Input.GetJoystickNames()[4] == "Wireless Controller")
						{
							Instance.gmInputs[(int)PN.P5].mJump = Input.GetButtonDown("p5_ps4_jump");
							Instance.gmInputs[(int)PN.P5].mCatch = Input.GetButtonDown("p5_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P5].mChargeThrow = Input.GetButton("p5_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P5].mCatchRelease = Input.GetButtonUp("p5_ps4_catch/throw");
							Instance.gmInputs[(int)PN.P5].mStart = Input.GetButtonDown("p5_ps4_start");
						}
						else if (Input.GetJoystickNames()[4] == "XBOX 360 For Windows (Controller)")
						{
							Instance.gmInputs[(int)PN.P5].mJump = Input.GetButtonDown("p5_jump");
							Instance.gmInputs[(int)PN.P5].mCatch = Input.GetButtonDown("p5_catch/throw");
							Instance.gmInputs[(int)PN.P5].mChargeThrow = Input.GetButton("p5_catch/throw");
							Instance.gmInputs[(int)PN.P5].mCatchRelease = Input.GetButtonUp("p5_catch/throw");
							Instance.gmInputs[(int)PN.P5].mStart = Input.GetButtonDown("p5_start");
						}
                        else
                        {
                            Instance.gmInputs[(int)PN.P5].mJump = Input.GetButtonDown("p5_jump");
                            Instance.gmInputs[(int)PN.P5].mCatch = Input.GetButtonDown("p5_catch/throw");
                            Instance.gmInputs[(int)PN.P5].mChargeThrow = Input.GetButton("p5_catch/throw");
                            Instance.gmInputs[(int)PN.P5].mCatchRelease = Input.GetButtonUp("p5_catch/throw");
                            Instance.gmInputs[(int)PN.P5].mStart = Input.GetButtonDown("p5_start");
                        }
					}
					else
					{
						if (Input.GetJoystickNames()[4] == "Wireless Controller")
							Instance.gmInputs[(int)PN.P5].mJump = Input.GetButtonDown("p5_ps4_jump");
						else if (Input.GetJoystickNames()[4] == " XBOX 360 For Windows (Controller)")
							Instance.gmInputs[(int)PN.P5].mJump = Input.GetButtonDown("p5_jump");
					}
				}
            }
        
    }

    public void CreatePlayers()
    {
        if (TotalNumberofActors > 0)
        {
            //if(Instance.gmRecordKeeper != null && Instance.gmRecordKeeper.playerGorilla >= 0) // Get gorilla from Record keeper first if possible.
            //{
            //    PlayerGorilla = Instance.gmRecordKeeper.playerGorilla;
            //    //Debug.Log("Game Manager: Gorilla chosen from Record Keeper: " + PlayerGorilla.ToString());
            //}
            //else 
            if (RandomGorilla)
            {
                //Debug.Log("Game Manager: Gorilla chosen by RandomGorilla: " + PlayerGorilla.ToString());
                PlayerGorilla = RandGor();
            }

            for (int i = 0; i < TotalNumberofActors; ++i)
            {
                Transform temp = Instance.gmSpawnManager.SpawnPoints[i];
                if (NumberOfPlayersToBuild > 0)
                {
					gmPlayers.Add(null);
					gmPlayerScripts.Add(null);
                    gmPlayers[i] = (GameObject)Instantiate(Instance.gmPlayerPrefab, temp.position, temp.rotation);
                    gmPlayerScripts[i] = Instance.gmPlayers[i].GetComponent<Player>();
                    gmPlayerScripts[i].isPlayer = true;
                    gmPlayerScripts[i].inputIndex = i;
                    InputIndecies[i] = i;
                    --NumberOfPlayersToBuild;
                }
                else if (NumberOfBotsToBuild > 0)
                {
					gmPlayers.Add(null);
					gmPlayerScripts.Add(null);
					gmPlayers[i] = (GameObject)Instantiate(Instance.gmPlayerPrefabAI, temp.position, temp.rotation);
                    gmPlayerScripts[i] = Instance.gmPlayers[i].GetComponent<AI>();
                    gmPlayerScripts[i].isPlayer = false;
                    gmPlayerScripts[i].inputIndex = i;
                    InputIndecies[i] = i;
                    --NumberOfBotsToBuild;
                }

                gmPlayers[i].GetComponent<Actor>().playerIndex = i;

                if (PlayerGorilla == i)
                {
                    if (!noGorilla)
                    {
                        Instance.gmPlayers[i].GetComponent<Actor>().characterType = new Gorilla(i, i);
                        Instance.gmPlayers[i].GetComponent<Transform>().localScale = Instance.gmPlayers[i].GetComponent<Actor>().characterType.gorillaSize;
                    }
                    else
                    {
                        Instance.gmPlayers[i].GetComponent<Actor>().characterType = new Monkey(i,i);

                    }
                }
                else
                {
                    Instance.gmPlayers[i].GetComponent<Actor>().characterType = new Monkey(i, i);
                }
            }
        }

    }

    public int RandGor()
    {
        return Random.Range(0, (int)TotalNumberofActors);
    }

    public void AddBall(GameObject ball)
    {
        gmBalls.Add(ball);
    }

	public void AddBot()
	{
		gmPlayers.Add(null);
		gmPlayerScripts.Add(null);
		Transform temp = Instance.gmSpawnManager.SpawnPoints[(int)TotalNumberofActors];
		gmPlayers[(int)TotalNumberofActors] = (GameObject)Instantiate(Instance.gmPlayerPrefabAI, temp.position, temp.rotation);
		gmPlayerScripts[(int)TotalNumberofActors] = Instance.gmPlayers[(int)TotalNumberofActors].GetComponent<AI>();
		Instance.gmPlayerScripts[(int)TotalNumberofActors].isPlayer = false;
		Instance.gmPlayerScripts[(int)TotalNumberofActors].playerIndex = (int)TotalNumberofActors;
		for (int i =0;i < 5; ++i)
		{
			if(InputIndecies[i] == -1)
			{
				gmPlayerScripts[i].inputIndex = i;
				Instance.gmPlayers[(int)TotalNumberofActors].GetComponent<Actor>().characterType = new Monkey((int)TotalNumberofActors, i);
				InputIndecies[i] = (int)TotalNumberofActors;
				break;
			}
		}
		TotalNumberofActors++;

	}

    public int AddPlayer(int inIndex)
    {
		if (TotalNumberofActors <= 5)
		{
			gmPlayers.Add(null);
			gmPlayerScripts.Add(null);
			Transform temp = Instance.gmSpawnManager.SpawnPoints[(int)TotalNumberofActors];
			Instance.gmPlayers[(int)TotalNumberofActors] = (GameObject)Instantiate(Instance.gmPlayerPrefab, temp.position, temp.rotation);
			Instance.gmPlayerScripts[(int)TotalNumberofActors] = Instance.gmPlayers[(int)TotalNumberofActors].GetComponent<Player>();
			Instance.gmPlayerScripts[(int)TotalNumberofActors].isPlayer = true;
			Instance.gmPlayers[(int)TotalNumberofActors].GetComponent<Actor>().playerIndex = (int)TotalNumberofActors;
			Instance.gmPlayers[(int)TotalNumberofActors].GetComponent<Actor>().characterType = new Monkey((int)TotalNumberofActors, inIndex);
			gmPlayerScripts[(int)TotalNumberofActors].inputIndex = inIndex;
			InputIndecies[inIndex] = (int)TotalNumberofActors;
			TotalNumberofActors++;
			NumberOfPlayers++;
			return (int)TotalNumberofActors - 1;
		}
		else
		{
			return -1;
		}
    }

    public void RemovePlayer()
    {
		if (TotalNumberofActors >= 1)
		{
			Destroy(Instance.gmPlayers[(int)TotalNumberofActors - 1]);
			Instance.gmPlayers[(int)TotalNumberofActors - 1] = null;
			Instance.gmPlayerScripts[(int)TotalNumberofActors - 1] = null;
			gmPlayers.TrimExcess();
			gmPlayerScripts.TrimExcess();
			TotalNumberofActors--;
			NumberOfPlayers--;
		}
	}

	public void RemoveBot()
	{
		for (int i = (int)TotalNumberofActors - 1; i >= 0; i--)
		{
			if (!gmPlayerScripts[i].isPlayer)
			{
				Destroy(Instance.gmPlayers[i]);
				Instance.gmPlayers[i] = null;
				gmPlayers.RemoveAt(i);
				Instance.gmPlayerScripts[i] = null;
				gmPlayerScripts.RemoveAt(i);
				gmPlayers.TrimExcess();
				gmPlayerScripts.TrimExcess();
				TotalNumberofActors--;
				break;
			}
		}
	}
	public void SwitchRooms()
    {
        gmBalls.Clear();
        gmTurretManager.Reset();
    }
    IEnumerator LoadMatchScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Instance.gmCurtainController.CloseCurtain();

        yield return new WaitForSeconds(3f);
        playerCanMove = false;
        foreach(InputManager im in Instance.gmInputs)
        {
            im.Reset();
        }
        for (int i = 0; i < gmPlayers.Count; i++)
        {
            if (gmPlayers[i] != null)
            {
                gmPlayers[i].GetComponent<Actor>().SwitchRoomReset();
            }
        }
        Instance.SwitchRooms();
        SceneManager.LoadScene(nextRoom);
        if (nextRoom == "PregameRoom")
        {
            gmScoringManager.ClearScores();
            for (int i = 0; i < TotalNumberofActors; i++)
            {
                Instance.gmRecordKeeper.SetPlayerMaterial(i, gmRecordKeeper.defaultColour);
                Instance.gmPlayers[i].GetComponent<Actor>().UpdateColour();
            }
        }
        if (gmUIManager != null)
        {
            gmUIManager.SetToGameMode(nextGameModeUI);
        }
        yield return new WaitForSeconds(1f);
        Instance.gmCurtainController.OpenCurtain();
        yield return new WaitForSeconds(2f);
        if (nextIsMatch)
        {
            FindObjectOfType<StartMatchCountDown>().StartCountDown();
        }
        else
        {
            playerCanMove = true;
        }
    }
    public void StartMatch()
    {
        nextRoom = "mb_level04_v2";
        nextIsMatch = true;
        StartCoroutine(LoadMatchScene(2f));
    }
    public void StartMatch(string level)
    {
        nextRoom = level;
        nextIsMatch = true;
        StartCoroutine(LoadMatchScene(2f));
    }
    public void LoadTrophyRoom()
    {
        nextRoom = "VictoryRoom";
        nextIsMatch = false;
        StartCoroutine(LoadMatchScene(0f));
    }
    public void LoadPregameRoom()
    {
        nextRoom = "PregameRoom";
        nextIsMatch = false;
        StartCoroutine(LoadMatchScene(0f));
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public uint TotalNumberofPlayers; //Number of Actors
	[SerializeField]
	private uint NumberOfPlayersToBuild = 1; //Number of Players
	[SerializeField]
	private uint NumberOfBotsToBuild = 0; //Number of AI
	[SerializeField]
	private bool RandomGorilla = true;
	[SerializeField]
	private int PlayerGorilla;


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
	public bool noGorilla = false;

	private GameManager() { }

	void Awake()
	{
		DontDestroyOnLoad(this);
		TotalNumberofPlayers = NumberOfPlayersToBuild + NumberOfBotsToBuild;
        /*BallInfo tempInfo = FindObjectOfType<BallInfo>();
        if (tempInfo != null)
            gmBall = FindObjectOfType<BallInfo>().gameObject;*/
        CreateInputs();
		CreatePlayers();
	}

	void Update()
	{
		UpdateInputs();
	}

	public void CreateInputs()
	{
		for (int i = 0; i < (int)PN.Length; ++i)
		{
			Instance.gmInputs[i] = (InputManager)ScriptableObject.CreateInstance("InputManager");
		}
	}

	public void UpdateInputs()
	{
		if (Instance.gmPlayerScripts[(int)PN.P1].isPlayer)
		{
			Instance.gmInputs[(int)PN.P1].mXY.x = Input.GetAxis("p1_joy_x");
			Instance.gmInputs[(int)PN.P1].mXY.y = -Input.GetAxis("p1_joy_y");
			Instance.gmInputs[(int)PN.P1].mJump = Input.GetButtonDown("p1_jump");
			Instance.gmInputs[(int)PN.P1].mCatch = Input.GetButtonDown("p1_catch/throw");
			Instance.gmInputs[(int)PN.P1].mChargeThrow = Input.GetButton("p1_catch/throw");
			Instance.gmInputs[(int)PN.P1].mCatchRelease = Input.GetButtonUp("p1_catch/throw");
			Instance.gmInputs[(int)PN.P1].mAimStomp = Input.GetButtonDown("p1_aim/stomp");
			Instance.gmInputs[(int)PN.P1].mChargeStomp = Input.GetButton("p1_aim/stomp");
		}
		if (Instance.gmPlayerScripts[(int)PN.P2] != null)
		{
			if (Instance.gmPlayerScripts[(int)PN.P2].isPlayer)
			{
				Instance.gmInputs[(int)PN.P2].mXY.x = Input.GetAxis("p2_joy_x");
				Instance.gmInputs[(int)PN.P2].mXY.y = -Input.GetAxis("p2_joy_y");
				Instance.gmInputs[(int)PN.P2].mJump = Input.GetButtonDown("p2_jump");
				Instance.gmInputs[(int)PN.P2].mCatch = Input.GetButtonDown("p2_catch/throw");
				Instance.gmInputs[(int)PN.P2].mChargeThrow = Input.GetButton("p2_catch/throw");
				Instance.gmInputs[(int)PN.P2].mCatchRelease = Input.GetButtonUp("p2_catch/throw");
				Instance.gmInputs[(int)PN.P2].mAimStomp = Input.GetButtonDown("p2_aim/stomp");
				Instance.gmInputs[(int)PN.P2].mChargeStomp = Input.GetButton("p2_aim/stomp");
            }
		}

		if (Instance.gmPlayerScripts[(int)PN.P3] != null)
		{
			if (Instance.gmPlayerScripts[(int)PN.P3].isPlayer)
			{
				Instance.gmInputs[(int)PN.P3].mXY.x = Input.GetAxis("p3_joy_x");
				Instance.gmInputs[(int)PN.P3].mXY.y = -Input.GetAxis("p3_joy_y");
				Instance.gmInputs[(int)PN.P3].mJump = Input.GetButtonDown("p3_jump");
				Instance.gmInputs[(int)PN.P3].mCatch = Input.GetButtonDown("p3_catch/throw");
				Instance.gmInputs[(int)PN.P3].mChargeThrow = Input.GetButton("p3_catch/throw");
				Instance.gmInputs[(int)PN.P3].mCatchRelease = Input.GetButtonUp("p3_catch/throw");
				Instance.gmInputs[(int)PN.P3].mAimStomp = Input.GetButtonDown("p3_aim/stomp");
				Instance.gmInputs[(int)PN.P3].mChargeStomp = Input.GetButton("p3_aim/stomp");
			}
		}
        if (Instance.gmPlayerScripts[(int)PN.P4] != null)
        {
            if (Instance.gmPlayerScripts[(int)PN.P4].isPlayer)
            {
                Instance.gmInputs[(int)PN.P4].mXY.x = Input.GetAxis("p4_joy_x");
                Instance.gmInputs[(int)PN.P4].mXY.y = -Input.GetAxis("p4_joy_y");
                Instance.gmInputs[(int)PN.P4].mJump = Input.GetButtonDown("p4_jump");
                Instance.gmInputs[(int)PN.P4].mCatch = Input.GetButtonDown("p4_catch/throw");
                Instance.gmInputs[(int)PN.P4].mChargeThrow = Input.GetButton("p4_catch/throw");
                Instance.gmInputs[(int)PN.P4].mCatchRelease = Input.GetButtonUp("p4_catch/throw");
                Instance.gmInputs[(int)PN.P4].mAimStomp = Input.GetButtonDown("p4_aim/stomp");
                Instance.gmInputs[(int)PN.P4].mChargeStomp = Input.GetButton("p4_aim/stomp");
            }
        }
        if (Instance.gmPlayerScripts[(int)PN.P5] != null)
        {
            if (Instance.gmPlayerScripts[(int)PN.P5].isPlayer)
            {
                Instance.gmInputs[(int)PN.P5].mXY.x = Input.GetAxis("p5_joy_x");
                Instance.gmInputs[(int)PN.P5].mXY.y = -Input.GetAxis("p5_joy_y");
                Instance.gmInputs[(int)PN.P5].mJump = Input.GetButtonDown("p5_jump");
                Instance.gmInputs[(int)PN.P5].mCatch = Input.GetButtonDown("p5_catch/throw");
                Instance.gmInputs[(int)PN.P5].mChargeThrow = Input.GetButton("p5_catch/throw");
                Instance.gmInputs[(int)PN.P5].mCatchRelease = Input.GetButtonUp("p5_catch/throw");
                Instance.gmInputs[(int)PN.P5].mAimStomp = Input.GetButtonDown("p5_aim/stomp");
                Instance.gmInputs[(int)PN.P5].mChargeStomp = Input.GetButton("p5_aim/stomp");
            }
        }
    }

	public void CreatePlayers()
	{
		if (Instance.gmPlayers.Contains(null))
		{
			if (TotalNumberofPlayers > 0)
			{
                if(Instance.gmRecordKeeper != null && Instance.gmRecordKeeper.playerGorilla >= 0) // Get gorilla from Record keeper first if possible.
                {
                    PlayerGorilla = Instance.gmRecordKeeper.playerGorilla;
                }
                else if (RandomGorilla)
                {
                    PlayerGorilla = RandGor();
                }

                for (int i = 0; i < TotalNumberofPlayers; ++i)
				{
					Transform temp = Instance.gmSpawnManager.SpawnPoints[i];
					if (NumberOfPlayersToBuild > 0)
					{
						Instance.gmPlayers[i] = (GameObject)Instantiate(Instance.gmPlayerPrefab, temp.position, temp.rotation);
                        Instance.gmPlayerScripts[i] = Instance.gmPlayers[i].GetComponent<Player>();
                        Instance.gmPlayerScripts[i].isPlayer = true;
                        --NumberOfPlayersToBuild;
					}
					else if (NumberOfBotsToBuild > 0)
					{
						Instance.gmPlayers[i] = (GameObject)Instantiate(Instance.gmPlayerPrefabAI, temp.position, temp.rotation);
                        Instance.gmPlayerScripts[i] = Instance.gmPlayers[i].GetComponent<AI>();
                        Instance.gmPlayerScripts[i].isPlayer = false;
                        --NumberOfBotsToBuild;
					}
                    
					Instance.gmPlayers[i].GetComponent<Actor>().playerIndex = i;

					if (PlayerGorilla == i)
					{
						if (!noGorilla)
						{
							Instance.gmPlayers [i].GetComponent<Actor> ().characterType = new Gorilla (i);
							Instance.gmPlayers [i].GetComponent<Transform> ().localScale = Instance.gmPlayers [i].GetComponent<Actor> ().characterType.gorillaSize;
						} else 
						{
							Instance.gmPlayers[i].GetComponent<Actor>().characterType = new Monkey(i);
						}
					}
					else
					{
						Instance.gmPlayers[i].GetComponent<Actor>().characterType = new Monkey(i);
					}
				}
			}
		}
	}

	public int RandGor()
	{
		return Random.Range(0, (int)TotalNumberofPlayers);
	}
}

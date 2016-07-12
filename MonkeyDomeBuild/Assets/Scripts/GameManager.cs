using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public enum PlayerName
	{
		P1,
		P2,
		P3,
		P4,
		P5,
		Length
	}

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
	public SpawnManager gmSpawnManager;
	public ButtonManager gmButtonManager;
	public GameObject gmPlayerPrefab;
	public GameObject gmPlayerPrefabAI;
	public UIManager gmUIManager;
	public Actor gmActorScript;

	void Awake()
	{
		//gmActorScript.CreatePlayers();
	}
}

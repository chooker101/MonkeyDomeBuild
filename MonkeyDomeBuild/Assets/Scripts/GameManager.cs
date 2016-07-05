using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
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

	public Player gmplayer;
	//public ButtonManager gmbuttonmanager;
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour
{
	/*
     * We need this class to:
     * - keep track of how many players are playing
     * - handle players' stats
     * - provide a key to accessing each player's stats 
     */


	[SerializeField]
	private uint TNOP;
	[SerializeField]
	private uint NOP = 1;
	[SerializeField]
	private uint NOB = 0;

	public void CreatePlayers()
	{
		if (GameManager.Instance.gmPlayers.Contains(null))
		{
			TNOP = NOP + NOB;
			if (TNOP > 0)
			{
				for (int i = 0; i < TNOP; ++i)
				{
					Transform temp = GameManager.Instance.gmSpawnManager.SpawnPoints[i];
					if (NOP > 0)
					{
						GameManager.Instance.gmPlayers[i] = (GameObject)Instantiate(GameManager.Instance.gmPlayerPrefab, temp.position, temp.rotation);
						--NOP;
					}
					else if (NOB > 0)
					{
						GameManager.Instance.gmPlayers[i] = (GameObject)Instantiate(GameManager.Instance.gmPlayerPrefabAI, temp.position, temp.rotation);
						--NOB;
					}
				}
			}
		}
	}

	// Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}

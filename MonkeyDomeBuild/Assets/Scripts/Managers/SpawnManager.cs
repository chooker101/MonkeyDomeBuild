using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
	public enum Points
	{
		P1,
		P2,
		P3,
		P4,
		P5,
		Length
	}

	public Transform[] SpawnPoints;

	void Awake()
	{
		if (this.gameObject != GameManager.Instance.gmSpawnObject)
		{
			GameManager.Instance.gmSpawnObject = this.gameObject;
		}

		if (GetComponent<SpawnManager>() != GameManager.Instance.gmSpawnManager)
		{
			GameManager.Instance.gmSpawnManager = GetComponent<SpawnManager>();
		}

		Spawn(this.gameObject);
	}

	public void Start()
	{
		for (int j = 0; j < (int)GameManager.Instance.TotalNumberofPlayers; j++)
		{
			if (GameManager.Instance.gmPlayers[j] != null)
			{
				GameManager.Instance.gmPlayers[j].transform.position = SpawnPoints[j].position;
			}
		}
	}

	public void Spawn(GameObject obj)
	{
		int i = 0;
		foreach (Transform T in obj.GetComponentsInChildren<Transform>())
		{
			if (T != obj.GetComponent<Transform>())
			{
				if (SpawnPoints[i] != T)
				{
					SpawnPoints[i] = T;
				}
				i++;
				if (i > 4)
				{
					break;
				}
			}
		}
	}
}

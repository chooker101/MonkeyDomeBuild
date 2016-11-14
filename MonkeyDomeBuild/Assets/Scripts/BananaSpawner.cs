using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BananaSpawner : MonoBehaviour
{
    public GameObject bananaPropPrefab;
    public Transform bananaHolder;
    public List<Transform> spawnLocs = new List<Transform>();
    public List<int> playerScores = new List<int>();
    public List<int> playerBananaSpawned = new List<int>();
    List<float> bananaSpawnCount = new List<float>();
    public int scorePerBanana = 5;
    float bananaSpawnTime = 0.3f;
    float bananaSideForce = 1f;

    List<GameObject> pooledBananas = new List<GameObject>();
    int bananaPoolAmount = 2000;


    public GameObject[] Baskets;
    void Start()
    {
        for (int i = 0; i < GameManager.Instance.gmPlayers.Count; i++)
        {
            playerScores.Add(0);
            playerBananaSpawned.Add(0);
            bananaSpawnCount.Add(0f);
        }

        for(int i = 0; i < bananaPoolAmount; i++)
        {
            pooledBananas.Add(Instantiate(bananaPropPrefab));
            pooledBananas[i].transform.SetParent(bananaHolder);
            pooledBananas[i].SetActive(false);
        }


		for (uint i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
		{
			Baskets[i].SetActive(true);
		}

		for (uint i = GameManager.Instance.TotalNumberofPlayers; i <= 4; i++)
		{
			Baskets[i].SetActive(false);
		}

        if (GameManager.Instance.TotalNumberofPlayers == 1)
            transform.position = new Vector3(transform.position.x + 20, transform.position.y, transform.position.z);
        else if (GameManager.Instance.TotalNumberofPlayers == 2)
            transform.position = new Vector3(transform.position.x + 15, transform.position.y, transform.position.z);
        else if (GameManager.Instance.TotalNumberofPlayers == 3)
            transform.position = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);
        else if (GameManager.Instance.TotalNumberofPlayers == 4)
            transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
        else if (GameManager.Instance.TotalNumberofPlayers == 5)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            playerScores[Random.Range(0, playerScores.Count)] += 5;
        }
        UpdateScores();
        SpawnBanana();
    }
    void SpawnBanana()
    {
        for(int i = 0; i < playerScores.Count; i++)
        {
            if ((playerScores[i] / scorePerBanana) > playerBananaSpawned[i])
            {
                if (bananaSpawnCount[i] >= bananaSpawnTime)
                {
                    for(int x = 0; x < pooledBananas.Count; x++)
                    {
                        if (!pooledBananas[x].activeInHierarchy)
                        {
                            pooledBananas[x].transform.position = spawnLocs[i].position;
                            pooledBananas[x].transform.rotation = spawnLocs[i].rotation;
                            pooledBananas[x].SetActive(true);
							Vector3 dir = Vector3.right * Random.Range (-1f, 1f) + Vector3.forward * Random.Range (-1f, 1f) + Vector3.down * Random.Range (1f, 3f);
                            pooledBananas[x].GetComponent<Rigidbody>().AddRelativeTorque(pooledBananas[x].transform.right * Random.Range(-1f, 1f) * 50f, ForceMode.Impulse);
                            pooledBananas[x].GetComponent<Rigidbody>().AddRelativeTorque(pooledBananas[x].transform.up * Random.Range(-1f, 1f) * 50f, ForceMode.Impulse);
                            pooledBananas[x].GetComponent<Rigidbody>().AddForce(dir * bananaSideForce, ForceMode.Impulse);
                            playerBananaSpawned[i]++;
                            bananaSpawnCount[i] = 0;
                            break;
                        }
                    }
                }
                else
                {
                    bananaSpawnCount[i] += Time.deltaTime;
                }
            }
        }
    }
    void UpdateScores()
    {
        for(int i = 0; i < GameManager.Instance.gmPlayers.Count; i++)
        {
            playerScores[i] = GameManager.Instance.gmScoringManager.GetScore(i);
        }
    }
}

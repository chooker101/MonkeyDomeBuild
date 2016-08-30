using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BananaSpawner : MonoBehaviour
{
    public GameObject bananaPropPrefab;
    public List<Transform> spawnLocs = new List<Transform>();
    public List<int> playerScores = new List<int>();
    List<int> playerBananaSpawned = new List<int>();
    List<float> bananaSpawnCount = new List<float>();
    float bananaSpawnTime = 0.3f;
    float bananaSideForce = 2f;

    void Start()
    {
        playerScores.Add(0);
        playerScores.Add(0);
        playerScores.Add(0);
        for(int i = 0; i < playerScores.Count; i++)
        {
            playerBananaSpawned.Add(0);
            bananaSpawnCount.Add(0f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            playerScores[Random.Range(0, playerScores.Count)] += 5;
        }
        SpawnBanana();
    }
    void SpawnBanana()
    {
        for(int i = 0; i < playerScores.Count; i++)
        {
            if (playerScores[i] != playerBananaSpawned[i])
            {
                if (bananaSpawnCount[i] >= bananaSpawnTime)
                {
                    GameObject tempBanana = (GameObject)Instantiate(bananaPropPrefab, spawnLocs[i].position, spawnLocs[i].rotation);
                    tempBanana.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Random.Range(-1f, 1f) * bananaSideForce, ForceMode2D.Impulse);
                    playerBananaSpawned[i]++;
                    bananaSpawnCount[i] = 0;
                }
                else
                {
                    bananaSpawnCount[i] += Time.deltaTime;
                }
            }
        }
    }
}

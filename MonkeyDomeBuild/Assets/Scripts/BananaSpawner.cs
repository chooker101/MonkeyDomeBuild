﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BananaSpawner : MonoBehaviour
{
    public GameObject bananaPropPrefab;
    public Transform bananaHolder;
    public List<Transform> spawnLocs = new List<Transform>();
    public List<int> playerScores = new List<int>();
    List<int> playerBananaSpawned = new List<int>();
    List<float> bananaSpawnCount = new List<float>();
    float bananaSpawnTime = 0.3f;
    float bananaSideForce = 2f;

    List<GameObject> pooledBananas = new List<GameObject>();
    int bananaPoolAmount = 500;

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
        for(int i = 0; i < bananaPoolAmount; i++)
        {
            pooledBananas.Add(Instantiate(bananaPropPrefab));
            pooledBananas[i].transform.SetParent(bananaHolder);
            pooledBananas[i].SetActive(false);
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
                    for(int x = 0; x < pooledBananas.Count; x++)
                    {
                        if (!pooledBananas[x].activeInHierarchy)
                        {
                            pooledBananas[x].transform.position = spawnLocs[i].position;
                            pooledBananas[x].transform.rotation = spawnLocs[i].rotation;
                            pooledBananas[x].SetActive(true);
                            Vector3 dir = Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f);
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
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoconutSpawner : MonoBehaviour
{
    public GameObject coconutPrefab;
    private List<GameObject> pooledCoconut = new List<GameObject>();
    public bool gameEnd = false;
    private int maxCoconutOnStage = 8;
    void Start()
    {
        StartCoroutine(SpawnCoconut(Random.Range(2, 4)));
        int pooledAmount = 20;
        for(int i = 0; i < pooledAmount; i++)
        {
            GameObject tempCoconut = (GameObject)Instantiate(coconutPrefab, transform.position, Quaternion.identity);
            pooledCoconut.Add(tempCoconut);
            tempCoconut.SetActive(false);
        }
    }
    void Update()
    {

    }
    IEnumerator SpawnCoconut(int spawnAmount)
    {

        if (GetCurrentActiveCoconut() < maxCoconutOnStage)
        {           
            for (int i = 0; i < spawnAmount; i++)
            {
                Vector3 newCoconutPos = transform.position;
                newCoconutPos.z = -5f;
                newCoconutPos.x = Random.Range(transform.position.x - 30f, transform.position.x + 30f);
                GameObject tempCoconut = GetAvaliableCoconut();
                if (tempCoconut != null)
                {
                    tempCoconut.SetActive(true);
                    tempCoconut.layer = LayerMask.NameToLayer("UsedCoconut");
                    tempCoconut.transform.position = newCoconutPos;
                }
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            }
        }
        yield return new WaitForSeconds(Random.Range(10f, 20f));
        StartCoroutine(SpawnCoconut(Random.Range(2, 4)));
    }
    private GameObject GetAvaliableCoconut()
    {
        for (int i = 0; i < pooledCoconut.Count; i++)
        {
            if (!pooledCoconut[i].activeInHierarchy)
            {
                return pooledCoconut[i];
            }
        }
        return null;
    }
    private int GetCurrentActiveCoconut()
    {
        int ans = 0;
        for (int i = 0; i < pooledCoconut.Count; i++)
        {
            if (pooledCoconut[i].activeInHierarchy)
            {
                ans++;
            }
        }
        return ans;
    }
}

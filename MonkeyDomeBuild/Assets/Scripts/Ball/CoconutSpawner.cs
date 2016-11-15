using UnityEngine;
using System.Collections;

public class CoconutSpawner : MonoBehaviour
{
    public GameObject coconutPrefab;
    public bool gameEnd = false;
    void Start()
    {
        StartCoroutine(SpawnCoconut(Random.Range(1, 3)));
    }
    void Update()
    {


    }
    IEnumerator SpawnCoconut(int spawnAmount)
    {
        for(int i = 0; i < spawnAmount; i++)
        {
            Vector3 newCoconutPos = transform.position;
            newCoconutPos.z = -5f;
            newCoconutPos.x = Random.Range(transform.position.x - 30f, transform.position.x + 30f);
            GameObject tempCoconut = (GameObject)Instantiate(coconutPrefab, newCoconutPos, Quaternion.identity);
            tempCoconut.layer = LayerMask.NameToLayer("UsedCoconut");
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
        yield return new WaitForSeconds(Random.Range(30f, 40f));
        StartCoroutine(SpawnCoconut(Random.Range(2, 4)));
    }
}

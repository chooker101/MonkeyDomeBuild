using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretsManager : MonoBehaviour
{
    public List<Turret> turrets = new List<Turret>();
    private List<TargetQueue> targetQueues = new List<TargetQueue>();
    public GameObject banana;
    public GameObject poo;
    public float tempShootTimeMin;
    public float tempShootTimeMax;
    private float tempShootTime;

    void Start()
    {
        GameObject[] tempTurrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach(GameObject obj in tempTurrets)
        {
            turrets.Add(obj.GetComponent<Turret>());
        }
        tempShootTimeMin = 8f;
        tempShootTimeMax = 10f;
        tempShootTime = Random.Range(tempShootTimeMin, tempShootTimeMax);

    }

    void Update()
    {
        TempTest();
        if (Input.GetKeyDown(KeyCode.U))
        {
            //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            AddFireQueue(GameManager.Instance.gmPlayers[Random.Range(0,(int)GameManager.Instance.TotalNumberofPlayers)], 0);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddFireQueue(GameManager.Instance.gmPlayers[Random.Range(0, (int)GameManager.Instance.TotalNumberofPlayers)], 1);
        }
        //Debug.Log(targetQueues.Count);
        if (targetQueues.Count > 0)
        {
            int availableTurretIndex = 99;
            int defaultIndex = availableTurretIndex;
            if (TurretAvailable(defaultIndex, out availableTurretIndex))
            {
                if (availableTurretIndex != defaultIndex)
                {
                    // Order the available turret to move to location and then open fire
                    turrets[availableTurretIndex].OpenFire(targetQueues[0]);
                    targetQueues.RemoveAt(0);
                }
            }
        }
    }   
    bool TurretAvailable(int defaultIndex,out int turretIndex)
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            if (turrets[i].IsAvailable())
            {
                turretIndex = i;
                return true;
            }
        }
        turretIndex = defaultIndex;
        return false;
    }
    public void AddFireQueue(GameObject target,int _0banana_1poo)
    {       
        switch (_0banana_1poo)
        {
            case 0:
                targetQueues.Add(new TargetQueue(target, banana));
                //Debug.Log("add banana");
                break;
            case 1:
                targetQueues.Add(new TargetQueue(target, poo));
                //Debug.Log("add poo");
                break;
        }

    }

    void TempTest()
    {
        if (Time.time > tempShootTime)
        {
            tempShootTime = Random.Range(tempShootTimeMin, tempShootTimeMax);
            //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            int whatToShoot = Random.Range(0, 1);
            AddFireQueue(GameManager.Instance.gmPlayers[Random.Range(0, (int)GameManager.Instance.TotalNumberofPlayers)], whatToShoot);
        }
    }
}

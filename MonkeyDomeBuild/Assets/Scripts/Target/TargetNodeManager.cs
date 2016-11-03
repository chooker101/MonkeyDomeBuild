using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetNodeManager : MonoBehaviour
{
    private static TargetNodeManager myInstance;
    public GameObject targetPrefab;
    private int numberOfTargets = 0;
    public List<GameObject> targetNodes;
    void Start()
    {
        TargetNode[] tempNodes = FindObjectsOfType<TargetNode>();
        List<TargetNode> tempList = new List<TargetNode>();
        numberOfTargets = tempNodes.Length;
        for(int i = 0; i < tempNodes.Length; i++)
        {
            targetNodes.Add(tempNodes[i].gameObject);
            tempList.Add(tempNodes[i]);
        }
        for(int i = 0; i < numberOfTargets; i++)
        {
            if (tempList.Count > 0)
            {
                int tempIndex = Random.Range(0, tempList.Count);
                tempList[tempIndex].Init();
                tempList.RemoveAt(tempIndex);
            }
            else
            {
                break;
            }
        }        
    }
    public static TargetNodeManager Instance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<TargetNodeManager>();
            }
            return myInstance;
        }
    }
    public GameObject TargetPrefab
    {
        get
        {
            return targetPrefab;
        }
    }
}

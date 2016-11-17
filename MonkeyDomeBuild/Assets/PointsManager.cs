using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsManager : MonoBehaviour {

    public Sprite[] pointSprites;
    public List<Transform> pointObjects = new List<Transform>();
    public List<int> displayQueue = new List<int>();
    public List<int> playerIndex = new List<int>();
    public GameObject pointPrefab;

    public int amount = 10;
    // Use this for initialization
    void Start () {
        for(int i = 0; i < amount; i++)
        {
            GameObject tempPoint = (GameObject)Instantiate(pointPrefab, transform.position, Quaternion.identity);
            tempPoint.transform.SetParent(transform);
            pointObjects.Add(tempPoint.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
        // if queue > 1 run it
        if (displayQueue.Count > 0)
        {
            for (int i = 0; i < displayQueue.Count; i++)
            {
                for(int j = 0; j < pointObjects.Count; j++)
                {
                    if (pointObjects[j].gameObject.activeInHierarchy)
                    {
                        SetSprite(displayQueue[i], playerIndex[i]);
                        pointObjects[j].gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

	}

    void AddQueue(int score, int index)
    {
        displayQueue.Add(score);
        playerIndex.Add(index);
    }

    void SetSprite(int score, int pointObjectIndex)
    {
        // switch to check which sprite to set
        switch (score)
        {
            case 3:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[1];
                break;
            case 5:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[0];
                break;
            case 8:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[3];
                break;
            case 10:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[2];
                break;
            case 15:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[6];
                break;
            case 20:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[5];
                break;
            case -5:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[4];
                break;
            case -10:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[7];
                break;
            default:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = null;
                break;
        }
    }


}

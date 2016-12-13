using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour {

    public Sprite[] pointSprites;
    public List<Transform> pointObjects = new List<Transform>();
    public List<int> displayQueue = new List<int>();
    public List<int> playerIndex = new List<int>();
    public GameObject pointPrefab;

    public int amount = 10;
    bool canDisplay = true;
    public Color gainScoreColor;
    public Color loseScoreColor;
    // Use this for initialization
    void Start () {
        for(int i = 0; i < amount; i++)
        {
            GameObject tempPoint = (GameObject)Instantiate(pointPrefab, transform.position + Vector3.back * 5f, Quaternion.identity);
            tempPoint.transform.SetParent(transform);
            pointObjects.Add(tempPoint.transform);
            tempPoint.GetComponent<PointEffect>().Init();
            tempPoint.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        // if queue > 1 run it
        if (canDisplay)
        {
            if (displayQueue.Count > 0)
            {
                for (int j = 0; j < pointObjects.Count; j++)
                {
                    if (!pointObjects[j].gameObject.activeInHierarchy)
                    {
                        SetSprite(displayQueue[0], j);
                        canDisplay = false;
                        StartCoroutine(PointDelay());
                        pointObjects[j].gameObject.SetActive(true);
                        pointObjects[j].GetComponent<PointEffect>().IsDisplaying = true;
                        displayQueue.RemoveAt(0);
                        playerIndex.RemoveAt(0);
                        break;
                    }
                }
            }
        }

	}

    public void AddQueue(int score, int index)
    {
        displayQueue.Add(score);
        playerIndex.Add(index);
    }
    void SetSprite(int score, int pointObjectIndex)
    {
        // switch to check which sprite to set
        /*
        switch (score)
        {
            case 3:
                //pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[1];
                //pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().color = Color.white;
                pointObjects[pointObjectIndex].transform.localScale = new Vector3(1, 1, 1);
                break;
            case 5:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[0];
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().color = Color.white;
                pointObjects[pointObjectIndex].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                break;
            case 8:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[3];
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().color = Color.white;
                pointObjects[pointObjectIndex].transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                break;
            case 10:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[2];
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().color = Color.white;
                pointObjects[pointObjectIndex].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            case 15:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[6];
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().color = Color.white;
                pointObjects[pointObjectIndex].transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
                break;
            case 20:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[5];
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().color = Color.white;
                pointObjects[pointObjectIndex].transform.localScale = new Vector3(2f, 2f, 2f);
                break;
            case -5:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[4];
                pointObjects[pointObjectIndex].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case -10:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = pointSprites[7];
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().color = Color.red;
                pointObjects[pointObjectIndex].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            default:
                pointObjects[pointObjectIndex].GetComponent<SpriteRenderer>().sprite = null;
                break;
        }
        */
        string t = "";
        if (score > 0)
        {
            t = string.Format("+{0}", score);
            pointObjects[pointObjectIndex].GetComponentInChildren<Text>().color = gainScoreColor;
        }
        else
        {
            t = string.Format("{0}", score);
            pointObjects[pointObjectIndex].GetComponentInChildren<Text>().color = loseScoreColor;
        }
        if (Mathf.Abs(score) >= 20)
        {
            pointObjects[pointObjectIndex].transform.localScale = new Vector3(2f, 2f, 2f);
        }
        else if(Mathf.Abs(score) >= 10)
        {
            pointObjects[pointObjectIndex].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if(Mathf.Abs(score) >= 5)
        {
            pointObjects[pointObjectIndex].transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }
        else
        {
            pointObjects[pointObjectIndex].transform.localScale = new Vector3(1, 1, 1);
        }
        
        pointObjects[pointObjectIndex].GetComponentInChildren<Text>().text = t;
    }

    IEnumerator PointDelay()
    {
        yield return new WaitForSeconds(.2f);
        canDisplay = true;
    }
}

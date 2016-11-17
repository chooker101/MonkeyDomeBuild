using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PointsManager : MonoBehaviour {

    public Sprite[] pointSprites;
    public Transform[] pointObjects;
    public List<int> displayQueue;

	// Use this for initialization
	void Start () {
        pointObjects = GetComponentsInChildren<Transform>(); 
	}
	
	// Update is called once per frame
	void Update () {
	// if queue > 1 run it
	}

    void DisplayQueue()
    {
        // display 
    }

    void AddQueue(int score)
    {
        displayQueue.Add(score);
    }

    void SetSprite()
    {
        // switch to check which sprite to set
    }


}

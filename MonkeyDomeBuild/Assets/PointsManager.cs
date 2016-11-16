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
	
	}

    void DisplayQueue()
    {

    }

    void SetQueue()
    {

    }


}

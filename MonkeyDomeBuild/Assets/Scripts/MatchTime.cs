using UnityEngine;
using System.Collections;

public class MatchTime : MonoBehaviour {

    public static float matchTimeRemaining = 15000;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (matchTimeRemaining > 0) {
            matchTimeRemaining -= Time.deltaTime;
        }

        if (matchTimeRemaining < 0)
        {
            matchTimeRemaining = 0;
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public Text p1Score;
    public Text p2Score;
    public Text p3Score;
    public Text matchTime;
    public Text debugLog;
    public Text shotClock;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        p1Score.text =
            "00"
            ;
        p2Score.text =
            ":D"
            ;
        p3Score.text =
            "c:"
            ;
        matchTime.text = // time of the match left
            "42:69"
            ;


        debugLog.text = // Displays all debug info

            "PLAYER 1 STATS: \n" +
            "Move Force: " + p1.GetComponent<PlayerAction>().moveForce + "\n" +
            "Speed Limit: " + p1.GetComponent<PlayerAction>().speedLimit + "\n" +
            "Mov: " + p1.GetComponent<PlayerAction>().mov + "\n\n" +
            "# Jumps: " + p1.GetComponent<PlayerAction>().stat_jump + "\n" +
            "# Ball Grabs: " + p1.GetComponent<PlayerAction>().stat_ballGrab + "\n" +
            "# Throws: " + p1.GetComponent<PlayerAction>().stat_throw + "\n" +
            "# Targets hit: " + ScoringManager.targetsHit + "\n"
            //"Audience Attitude: " + "\n" +
            //"Audience Target: " + "\n"
            ;
    }
}

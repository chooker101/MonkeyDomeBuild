using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HUD : MonoBehaviour {

    public GameObject player_1;
    public GameObject player_2;
    public GameObject player_3;
    public Text p1_score;
    public Text p2_score;
    public Text p3_score;
    public Text match_time;
    public Text debug_log;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        p1_score.text =
            "00"
            ;
        p2_score.text =
            ":D"
            ;
        p3_score.text =
            "c:"
            ;
        match_time.text =
            "42:69"
            ;


        debug_log.text =

            "PLAYER 1 STATS: \n" +
            "Move Force: " + player_1.GetComponent<PlayerAction>().moveForce + "\n" +
            "Speed Limit: " + player_1.GetComponent<PlayerAction>().speedLimit + "\n" +
            "Mov: " + player_1.GetComponent<PlayerAction>().mov + "\n\n" +
            "# Jumps: " + player_1.GetComponent<PlayerAction>().stat_jump + "\n" +
            "# Throws: " + player_1.GetComponent<PlayerAction>().stat_throw + "\n" +
            "# catches: " + player_1.GetComponent<PlayerAction>().stat_catch + "\n"
            ;
    }
}

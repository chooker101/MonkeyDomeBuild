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

        debug_log.text =

            "Player 1 Move Force: " + player_1.GetComponent<PlayerAction>().moveForce + "\n" +
            "Player 1 Speed Limit: " + player_1.GetComponent<PlayerAction>().speedLimit + "\n" +
            "Player 1 Mov: " + player_1.GetComponent<PlayerAction>().mov + "\n\n" +

            "Player 2 Move Force: " + player_1.GetComponent<PlayerAction>().moveForce + "\n" +
            "Player 2 Speed Limit: " + player_1.GetComponent<PlayerAction>().speedLimit + "\n" +
            "Player 2 Mov: " + player_1.GetComponent<PlayerAction>().mov + "\n\n" +

            "Player 3 Move Force: " + player_3.GetComponent<PlayerAction>().moveForce + "\n" +
            "Player 3 Speed Limit: " + player_3.GetComponent<PlayerAction>().speedLimit + "\n" +
            "Player 3 Mov: " + player_3.GetComponent<PlayerAction>().mov + "\n\n"
            ;
    }
}

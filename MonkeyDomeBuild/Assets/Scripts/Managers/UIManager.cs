﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Text p1Score;
    public Text p2Score;
    public Text p3Score;
    public Text matchTimeText;
    public bool noTime;
    public float matchTime;
    public Text debugLog;
    public Text shotClock;

    private GameObject gameManager;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");


	}
	
	// Update is called once per frame
	void Update ()
	{
        if (noTime == false)
        {
            if (matchTime > 0)
            {
                matchTime -= Time.deltaTime;
            }
            else if (matchTime <= 0)
            {
                matchTime = 0;
                SceneManager.LoadScene("PregameRoom");
            }
        }

        p1Score.text =
            gameManager.GetComponent<ScoringManager>().p1Score.ToString();
            ;
        p2Score.text =
            gameManager.GetComponent<ScoringManager>().p2Score.ToString();
        ;
        p3Score.text =
            gameManager.GetComponent<ScoringManager>().p3Score.ToString();
        ;

        if (noTime == false)
        {
            matchTimeText.text = // time of the match left
       matchTime.ToString("F2");
        }
        else
            matchTimeText.text = "∞";

            debugLog.text = // Displays all debug info
            "PLAYER 1 STATS: \n" +
            "Mov: " + GameManager.Instance.gmPlayers[0].GetComponent<Player>().movement + "\n\n" +
            "# Jumps: " + GameManager.Instance.gmPlayers[0].GetComponent<Player>().stat_jump + "\n" +
            "# Ball Grabs: " + GameManager.Instance.gmPlayers[0].GetComponent<Player>().stat_ballGrab + "\n" +
            "# Throws: " + GameManager.Instance.gmPlayers[0].GetComponent<Player>().stat_throw + "\n\n"
            //"Targets Hit: " + ScoringManager.targetsHit;
            //"Audience Attitude: " + "\n" +
            //"Audience Target: " + "\n"
            ;
    }
}

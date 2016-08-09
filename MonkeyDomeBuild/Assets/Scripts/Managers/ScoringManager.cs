﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WhichPlayer
{
    Player1 = 0,
    Player2 = 1,
    Player3 = 3,
    Player4 = 4,
    Player5 = 5
}

public class ScoringManager : MonoBehaviour
{
    //keep track of ball & players position
    public int p1Score;
    public int p2Score;
    public int p3Score;


    private BallInfo _ball = null;
    public static Dictionary<WhichPlayer, int> playerScores = new Dictionary<WhichPlayer, int>();

    private float minDistanceTravel = 30f;
    private float maxTravelTime = 8f;
    private int catchScore = 5;
    private int passScore = 3;


    void Start()
    {
        _ball = FindObjectOfType<BallInfo>();
        playerScores.Add(WhichPlayer.Player1, 0);
        playerScores.Add(WhichPlayer.Player2, 0);
        playerScores.Add(WhichPlayer.Player3, 0);
        playerScores.Add(WhichPlayer.Player4, 0);
        playerScores.Add(WhichPlayer.Player5, 0);
    }

    void Update()
    {
        p1Score = playerScores[WhichPlayer.Player1];
        p2Score = playerScores[WhichPlayer.Player2];
        p3Score = playerScores[WhichPlayer.Player3];
    }
    WhichPlayer CheckWhichPlayer(int playerIndex)
    {
        WhichPlayer tempPlayer = WhichPlayer.Player1;
        switch (playerIndex)
        {
            case 0:
                tempPlayer = WhichPlayer.Player1;
                break;
            case 1:
                tempPlayer = WhichPlayer.Player2;
                break;
            case 2:
                tempPlayer = WhichPlayer.Player3;
                break;
            case 3:
                tempPlayer = WhichPlayer.Player4;
                break;
            case 4:
                tempPlayer = WhichPlayer.Player5;
                break;
        }
        return tempPlayer;
    }
    void AddScore(WhichPlayer player,int score)
    {
        playerScores[player] += score;
    }
    public void PassingScore(GameObject thrower, GameObject catcher, float distanceTravel,float travelTime)
    {
        //Debug.Log(thrower.name + catcher.name + distanceTravel.ToString());
        if (thrower != null && catcher != null)
        {
            if (thrower.GetInstanceID() != catcher.GetInstanceID())
            {
                if (distanceTravel > minDistanceTravel && travelTime < maxTravelTime)
                {
                    AddScore(CheckWhichPlayer(thrower.GetComponent<Actor>().whichplayer), passScore);
                    AddScore(CheckWhichPlayer(catcher.GetComponent<Actor>().whichplayer), catchScore);
                }
            }
        }
    }

}

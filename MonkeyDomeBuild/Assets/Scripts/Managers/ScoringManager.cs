﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WhichPlayer
{
    Player1 = 0,
    Player2 = 1,
    Player3 = 2,
    Player4 = 3,
    Player5 = 4
}

public class ScoringManager : MonoBehaviour
{
    //keep track of ball & players position
    public int p1Score;
    public int p2Score;
    public int p3Score;
    public int p4Score;
    public int p5Score;

    //private BallInfo _ball = null;
    public static Dictionary<WhichPlayer, int> playerScores = new Dictionary<WhichPlayer, int>();

    public int throwCombo = 0;

    private float longThrowDistance = 70f;
    //private float maxTravelTime = 8f;
    private int longThrowMaxBounce = 8;
    private int minBounce = 3;
    private int maxBounce = 6;

    private int throwComboInc = 3;
    private int catchScore = 3;
    private int perfectCatchScore = 8;
    private int passScore = 5;
    private int longThrowScore = 5;
    private int bounceScore = 3;
    private int catchInAirScore = 3;
    private int monkeyGettingInterceptScore = -10;
    private int innocentMonkeyScore = -5;
    private int gorillaSwitchScore = 5;
    //private int gorillaInterceptScore = 20;
    private int hitTargetScoreT0 = 5;
    private int hitTargetScoreT1 = 10;
    private int hitTargetScoreT2 = 15;
    private int hitTargetScoreT3 = 20;
    private int shotClockExpireScore = -10;

    void Start()
    {
        //_ball = FindObjectOfType<BallInfo>();
        playerScores.Clear();
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
        p4Score = playerScores[WhichPlayer.Player4];
        p5Score = playerScores[WhichPlayer.Player5];
    }
    public int GetScore(int playerIndex)
    {
        if (playerIndex < GameManager.Instance.gmPlayers.Count)
        {
            return playerScores[CheckWhichPlayer(playerIndex)];
        }
        else
        {
            return 0;
        }
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
    void AddScore(int playerIndex,int score)
    {
        GameManager.Instance.gmPlayers[playerIndex].GetComponent<EffectControl>().PlayHappyFace();
        GameManager.Instance.gmTrophyManager.GotPoints(playerIndex, score);
        if (GameManager.Instance.gmAudienceAnimator != null)
        GameManager.Instance.gmAudienceAnimator.AudienceHappy();

        WhichPlayer p = CheckWhichPlayer(playerIndex);
        if (playerScores[p] + score > 0)
        {
            playerScores[p] += score;
        }
        else if (playerScores[p] + score < 0)
        {
            playerScores[p] = 0;
        }

    }

    public void ClearScores()
    {
        playerScores[WhichPlayer.Player1] = 0;
        playerScores[WhichPlayer.Player2] = 0;
        playerScores[WhichPlayer.Player3] = 0;
        playerScores[WhichPlayer.Player4] = 0;
        playerScores[WhichPlayer.Player5] = 0;
    }
    public void PassingScore(GameObject thrower, GameObject catcher, BallInfo ballinfo)
    {
        // float distanceTravel, float travelTime, bool perfectCatch, int numberOfBounce
        float distanceTravel = ballinfo.GetDistanceTravel();
        float travelTime = ballinfo.travelTime;
        bool perfectCatch = ballinfo.IsPerfectCatch(catcher.GetComponent<Actor>());
        int numberOfBounce = ballinfo.numberOfBounce;
        int scoreGetThrower = 0;
        int scoreGetCatcher = 0;
        if (thrower != null && catcher != null)
        {
            if (thrower.GetComponent<Actor>().playerIndex != catcher.GetComponent<Actor>().playerIndex)
            {
                //Debug.Log("catch");
                scoreGetThrower += GetPassScore();
                scoreGetCatcher += GetCatchScore(perfectCatch);
                if (distanceTravel >= longThrowDistance && numberOfBounce <= longThrowMaxBounce)
                {
                    scoreGetThrower += longThrowScore;
                    GameManager.Instance.gmPlayers[thrower.GetComponent<Actor>().playerIndex].GetComponentInChildren<PointsManager>().AddQueue(longThrowScore, thrower.GetComponent<Actor>().playerIndex);
                    //TODO add in BouncePassEvent call here
                    //AddScore(thrower.GetComponent<Actor>().playerIndex, longThrowScore);
                    //Debug.Log("long throw");
                    if (numberOfBounce >= minBounce && numberOfBounce <= maxBounce)
                    {
                        scoreGetThrower += bounceScore;
                        GameManager.Instance.gmPlayers[thrower.GetComponent<Actor>().playerIndex].GetComponentInChildren<PointsManager>().AddQueue(bounceScore, thrower.GetComponent<Actor>().playerIndex);
                        //TODO add in BouncePassEvent call here
                        //AddScore(thrower.GetComponent<Actor>().playerIndex, bounceScore);
                        //Debug.Log("bounce");
                    }
                    if (catcher.GetComponent<Actor>().IsInAir)
                    {
                        scoreGetCatcher += catchInAirScore;
                        GameManager.Instance.gmPlayers[catcher.GetComponent<Actor>().playerIndex].GetComponentInChildren<PointsManager>().AddQueue(catchInAirScore, catcher.GetComponent<Actor>().playerIndex);
                    }
                }
                FindObjectOfType<ScoreVisualizer>().UpdateScore(thrower.GetComponent<Actor>().playerIndex, GetScore(thrower.GetComponent<Actor>().playerIndex), scoreGetThrower, "Successful Throw");
                FindObjectOfType<ScoreVisualizer>().UpdateScore(catcher.GetComponent<Actor>().playerIndex, GetScore(catcher.GetComponent<Actor>().playerIndex), scoreGetCatcher, "Successful Catch");
                AddScore(thrower.GetComponent<Actor>().playerIndex, scoreGetThrower);
                AddScore(catcher.GetComponent<Actor>().playerIndex, scoreGetCatcher);
                // adding point ups
                GameManager.Instance.gmPlayers[thrower.GetComponent<Actor>().playerIndex].GetComponentInChildren<PointsManager>().AddQueue(scoreGetThrower, thrower.GetComponent<Actor>().playerIndex);
				//Debug.Log(catcher.GetComponent<Actor>().playerIndex);
                GameManager.Instance.gmPlayers[catcher.GetComponent<Actor>().playerIndex].GetComponentInChildren<PointsManager>().AddQueue(scoreGetCatcher, catcher.GetComponent<Actor>().playerIndex);
                throwCombo++;
            }
        }
    }
    public void SwitchingScore(GameObject gorilla, GameObject ball)
    {
        //subtract score from other monkeys
        int switchScore = 0;
        for (int i = 0; i < GameManager.Instance.TotalNumberofActors; i++)
        {
            if (GameManager.Instance.gmPlayers[i].GetInstanceID() != gorilla.GetInstanceID())
            {
                if (GameManager.Instance.gmPlayers[i].GetInstanceID() == ball.GetComponent<BallInfo>().GetLastThrowMonkey().GetInstanceID())
                {
                    if (playerScores[CheckWhichPlayer(i)] < Mathf.Abs(monkeyGettingInterceptScore))
                    {
                        switchScore += playerScores[CheckWhichPlayer(i)];
                        FindObjectOfType<ScoreVisualizer>().UpdateScore(i, GetScore(i), playerScores[CheckWhichPlayer(i)], "Being Intercept");
                        if (playerScores[CheckWhichPlayer(i)] > 0)
                        {
                            GameManager.Instance.gmPlayers[i].GetComponentInChildren<PointsManager>().AddQueue(-playerScores[CheckWhichPlayer(i)], i);
                        }
                        playerScores[CheckWhichPlayer(i)] = 0;
                    }
                    else
                    {
                        switchScore += Mathf.Abs(monkeyGettingInterceptScore);
                        FindObjectOfType<ScoreVisualizer>().UpdateScore(i, GetScore(i), monkeyGettingInterceptScore, "Being Intercept");
                        AddScore(i, monkeyGettingInterceptScore);
                        // adding point ups
                        if (playerScores[CheckWhichPlayer(i)] > 0)
                        {
                            GameManager.Instance.gmPlayers[i].GetComponentInChildren<PointsManager>().AddQueue(monkeyGettingInterceptScore, i);
                        }
                    }
                    GameManager.Instance.gmPlayers[i].GetComponent<EffectControl>().PlaySadFace();
                }
                else
                {
                    if (playerScores[CheckWhichPlayer(i)] < Mathf.Abs(innocentMonkeyScore))
                    {
                        switchScore += playerScores[CheckWhichPlayer(i)];
                        FindObjectOfType<ScoreVisualizer>().UpdateScore(i, GetScore(i), playerScores[CheckWhichPlayer(i)], "Being Intercept");
                        if (playerScores[CheckWhichPlayer(i)] > 0)
                        {
                            GameManager.Instance.gmPlayers[i].GetComponentInChildren<PointsManager>().AddQueue(-playerScores[CheckWhichPlayer(i)], i);
                        }
                        playerScores[CheckWhichPlayer(i)] = 0;
                    }
                    else
                    {
                        switchScore += Mathf.Abs(innocentMonkeyScore);
                        FindObjectOfType<ScoreVisualizer>().UpdateScore(i, GetScore(i), innocentMonkeyScore, "Being Intercept");
                        AddScore(i, innocentMonkeyScore);
                        // adding point ups
                        if (playerScores[CheckWhichPlayer(i)] > 0)
                        {
                            GameManager.Instance.gmPlayers[i].GetComponentInChildren<PointsManager>().AddQueue(innocentMonkeyScore, i);
                        }
                    }
                    GameManager.Instance.gmPlayers[i].GetComponent<EffectControl>().PlaySadFace();
                }
            }
        }
        switchScore += gorillaSwitchScore;
        FindObjectOfType<ScoreVisualizer>().UpdateScore(gorilla.GetComponent<Actor>().playerIndex, GetScore(gorilla.GetComponent<Actor>().playerIndex), switchScore, "Intercept");
        AddScore(gorilla.GetComponent<Actor>().playerIndex, switchScore);
        // adding points ups
        GameManager.Instance.gmPlayers[gorilla.GetComponent<Actor>().playerIndex].GetComponentInChildren<PointsManager>().AddQueue(switchScore, gorilla.GetComponent<Actor>().playerIndex);
    }
    int GetPassScore()
    {
        int score = 0;
        score = passScore + throwCombo * throwComboInc;
        return score;
    }
    int GetCatchScore(bool perfectCatch)
    {
        int score = 0;
        if (perfectCatch)
        {
            score = catchScore + throwCombo * throwComboInc + perfectCatchScore;
        }
        else
        {
            score = catchScore + throwCombo * throwComboInc;
        }
        return score;
    }
    public void GorillaInterceptScore(GameObject gorilla, GameObject monkey, GameObject ball)
    {
        //int interceptScore = 0;
        //int monkeyIndex = monkey.GetComponent<Actor>().playerIndex;
        /*if (playerScores[CheckWhichPlayer(monkeyIndex)] < Mathf.Abs(gorillaInterceptScore))
        {
            interceptScore += playerScores[CheckWhichPlayer(monkeyIndex)];
            playerScores[CheckWhichPlayer(monkeyIndex)] = 0;
        }
        else
        {
            interceptScore += Mathf.Abs(gorillaInterceptScore);
            AddScore(monkeyIndex, -gorillaInterceptScore);
        }*/
        //AddScore(gorilla.GetComponent<Actor>().playerIndex, interceptScore);
        throwCombo = 0;
        if (gorilla != monkey)
        {
            //Debug.Log("intercept");
            SwitchingScore(gorilla, ball);
        }
    }

    public void HitTargetScore(BallInfo ball)
    {
		int monkeyIndex = 0;
		if (ball.GetLastThrowMonkey() != null)
		{
			monkeyIndex = ball.GetLastThrowMonkey().GetComponent<Actor>().playerIndex;
		}
        int score = 0;
        switch (GameManager.Instance.gmTargetManager.TargetTier)
        {
            default:
                score = 0;
                break;
            case 0:
                score = hitTargetScoreT0;
                break;
            case 1:
                score = hitTargetScoreT1;
                break;
            case 2:
                score = hitTargetScoreT2;
                break;
            case 3:
                score = hitTargetScoreT3;
                break;
        }
        FindObjectOfType<ScoreVisualizer>().UpdateScore(monkeyIndex, GetScore(monkeyIndex), score, "Hit Target");
        AddScore(monkeyIndex, score);
        // adding up score
        GameManager.Instance.gmPlayers[monkeyIndex].GetComponentInChildren<PointsManager>().AddQueue(score, monkeyIndex);
    }
    public void ShotClockExpireScore(Actor monkey)
    {
        int loseScore = 0;
        if (GetScore(monkey.playerIndex) >= Mathf.Abs(shotClockExpireScore))
        {
            loseScore = shotClockExpireScore;
        }
        else
        {
            loseScore = -GetScore(monkey.playerIndex);
        }
        FindObjectOfType<ScoreVisualizer>().UpdateScore(monkey.playerIndex, GetScore(monkey.playerIndex), loseScore, "Shot Clock Expire");
        AddScore(monkey.playerIndex, loseScore);
        GameManager.Instance.gmPlayers[monkey.playerIndex].GetComponentInChildren<PointsManager>().AddQueue(loseScore, monkey.playerIndex);
    }
    public void ResetCombo()
    {
        throwCombo = 0;
    }
    public void BattleRoyaleCoconutHit(int playerIndex)
    {
        GameManager.Instance.gmPlayers[playerIndex].GetComponentInChildren<PointsManager>().AddQueue(25, playerIndex);
        AddScore(playerIndex, 25);
        FindObjectOfType<ScoreVisualizer>().UpdateScore(playerIndex, GetScore(playerIndex), 25, "Coconut Hit");
    }
    public void BattleRoyaleCoconutKill(int playerIndex)
    {
        GameManager.Instance.gmPlayers[playerIndex].GetComponentInChildren<PointsManager>().AddQueue(50, playerIndex);
        AddScore(playerIndex, 25);
        FindObjectOfType<ScoreVisualizer>().UpdateScore(playerIndex, GetScore(playerIndex), 25, "Coconut Kill");
    }

}

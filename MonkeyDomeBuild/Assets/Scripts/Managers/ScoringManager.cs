using UnityEngine;
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
 

    private float minDistanceTravel = 30f;
    private float longThrowDistance = 70f;
    private float maxTravelTime = 8f;
    private int longThrowMaxBounce = 8;
    private int minBounce = 3;
    private int maxBounce = 6;

    private int catchScore = 5;
    private int perfectCatchScore = 8;
    private int passScore = 3;
    private int longThrowScore = 5;
    private int bounceScore = 3;
    private int catchInAirScore = 3;
    private int monkeyGettingInterceptScore = -10;
    private int innocentMonkeyScore = -5;
    private int gorillaInterceptScore = 20;
    private int hitTargetScoreT0 = 5;
    private int hitTargetScoreT1 = 10;
    private int hitTargetScoreT2 = 15;
    private int hitTargetScoreT3 = 20;

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
    public void PassingScore(GameObject thrower, GameObject catcher, float distanceTravel, float travelTime, bool perfectCatch, int numberOfBounce)
    {
        int scoreGetThrower = 0;
        int scoreGetCatcher = 0;
        if (thrower != null && catcher != null)
        {
            if (thrower.GetInstanceID() != catcher.GetInstanceID())
            {
                if (distanceTravel > minDistanceTravel && travelTime < maxTravelTime)
                {
                    scoreGetThrower += passScore;
                    scoreGetCatcher += perfectCatch ? perfectCatchScore : catchScore;
                }
                if (distanceTravel >= longThrowDistance && numberOfBounce <= longThrowMaxBounce)
                {
                    scoreGetThrower += longThrowScore;
                    //TODO add in BouncePassEvent call here
                    //AddScore(thrower.GetComponent<Actor>().playerIndex, longThrowScore);
                    //Debug.Log("long throw");
                    if (numberOfBounce >= minBounce && numberOfBounce <= maxBounce)
                    {
                        scoreGetThrower += bounceScore;
                        //TODO add in BouncePassEvent call here
                        //AddScore(thrower.GetComponent<Actor>().playerIndex, bounceScore);
                        //Debug.Log("bounce");
                    }
                    if (catcher.GetComponent<Actor>().IsInAir)
                    {
                        scoreGetCatcher += catchInAirScore;
                    }
                    FindObjectOfType<ScoreVisualizer>().UpdateScore(thrower.GetComponent<Actor>().playerIndex, GetScore(thrower.GetComponent<Actor>().playerIndex), scoreGetThrower, "Successful Throw");
                    FindObjectOfType<ScoreVisualizer>().UpdateScore(catcher.GetComponent<Actor>().playerIndex, GetScore(catcher.GetComponent<Actor>().playerIndex), scoreGetCatcher, "Successful Catch");
                    AddScore(thrower.GetComponent<Actor>().playerIndex, scoreGetThrower);
                    AddScore(catcher.GetComponent<Actor>().playerIndex, scoreGetCatcher);
                }
            }
        }
    }
    public void SwitchingScore(GameObject gorilla, GameObject ball)
    {
        //subtract scrore from other monkeys
        int switchScore = 0;
        for(int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
        {
            if (GameManager.Instance.gmPlayers[i].GetInstanceID() != gorilla.GetInstanceID())
            {
                if (GameManager.Instance.gmPlayers[i].GetInstanceID() == ball.GetComponent<BallInfo>().GetLastThrowMonkey().GetInstanceID())
                {
                    if (playerScores[CheckWhichPlayer(i)] < Mathf.Abs(monkeyGettingInterceptScore))
                    {
                        switchScore += playerScores[CheckWhichPlayer(i)];
                        FindObjectOfType<ScoreVisualizer>().UpdateScore(i, GetScore(i), playerScores[CheckWhichPlayer(i)], "Being Intercept");
                        playerScores[CheckWhichPlayer(i)] = 0;
                    }
                    else
                    {
                        switchScore += Mathf.Abs(monkeyGettingInterceptScore);
                        FindObjectOfType<ScoreVisualizer>().UpdateScore(i, GetScore(i), monkeyGettingInterceptScore, "Being Intercept");
                        AddScore(i, monkeyGettingInterceptScore);
                    }
                }
                else
                {
                    if (playerScores[CheckWhichPlayer(i)] < Mathf.Abs(innocentMonkeyScore))
                    {
                        switchScore += playerScores[CheckWhichPlayer(i)];
                        FindObjectOfType<ScoreVisualizer>().UpdateScore(i, GetScore(i), playerScores[CheckWhichPlayer(i)], "Being Intercept");
                        playerScores[CheckWhichPlayer(i)] = 0;
                    }
                    else
                    {
                        switchScore += Mathf.Abs(innocentMonkeyScore);
                        FindObjectOfType<ScoreVisualizer>().UpdateScore(i, GetScore(i), innocentMonkeyScore, "Being Intercept");
                        AddScore(i, innocentMonkeyScore);
                    }
                }
            }
        }
        FindObjectOfType<ScoreVisualizer>().UpdateScore(gorilla.GetComponent<Actor>().playerIndex, GetScore(gorilla.GetComponent<Actor>().playerIndex), switchScore, "Intercept");
        AddScore(gorilla.GetComponent<Actor>().playerIndex, switchScore);
    }
    public void GorillaInterceptScore(GameObject gorilla, GameObject monkey, GameObject ball)
    {
        //Debug.Log("intercetp");
        int interceptScore = 0;
        int monkeyIndex = monkey.GetComponent<Actor>().playerIndex;
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
        SwitchingScore(gorilla, ball);
    }
    public void HitTargetScore(BallInfo ball)
    {
        int monkeyIndex = ball.GetLastThrowMonkey().GetComponent<Actor>().playerIndex;
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
    }

}

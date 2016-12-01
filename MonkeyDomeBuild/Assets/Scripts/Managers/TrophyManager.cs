using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerTrophyStats
{
    public int bananasEaten;
    public int hitByPoop;
    public int perfectCatch;
    public int audienceLosses;
    public int audienceWins;


    public int Points;
    public int Catch;
    public int targetsHit;
    public int callsForBall;
    public int dashTackle;
    public int coconutThrows;



    public PlayerTrophyStats()
    {


        Points = 0;
        Catch = 0;
        coconutThrows = 0;
        targetsHit = 0;
        callsForBall = 0;
        dashTackle = 0;

    }

}

public class TrophyManager : MonoBehaviour
{
    private int maxPlayers = 5;
    // list for each stat, each stat will have a function that will add it

    public int a;
    public int b;
    public int c;
    public int d;
    public int e;
    public int f;

    private static List<PlayerTrophyStats> playerTrophyStats = new List<PlayerTrophyStats>();


    void Start()
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            playerTrophyStats.Add(new PlayerTrophyStats());
        }
    }

    /*
    public void BananasEaten(int playerIndex)
    {
        playerTrophyStats[playerIndex].bananasEaten++;
    }
    public void BeingHitByPoop(int playerIndex)
    {
        playerTrophyStats[playerIndex].hitByPoop++;
    }
    public void AudienceWins(int playerIndex) // need AudienceManager
    {
        playerTrophyStats[playerIndex].audienceWins++;
    }
    public void AudienceLosses(int playerIndex) // need AudienceManager
    {
        playerTrophyStats[playerIndex].audienceLosses++;
    }
    public void PerformPerfectCatch(int playerIndex)
    {
        playerTrophyStats[playerIndex].perfectCatch++;
    }
     
     */


    public void GotPoints(int playerIndex, int points)
    {
        playerTrophyStats[playerIndex].Points += points;
    }
    public void TargetsHit(int playerIndex)
    {
        playerTrophyStats[playerIndex].targetsHit++;
    }
    public void CallsForBall(int playerIndex)
    {
        playerTrophyStats[playerIndex].callsForBall++;
    }
    public void KnockDowns(int playerIndex)
    {
        playerTrophyStats[playerIndex].dashTackle++;
        //ndy
    }
    public void ThrewCoconut(int playerIndex)
    {
        playerTrophyStats[playerIndex].coconutThrows++;
    }
    public void CaughtBall(int playerIndex)
    {
        playerTrophyStats[playerIndex].Catch++;
    }

    public int CatchTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].Catch > tempHighest)
            {
                tempHighest = playerTrophyStats[i].Catch;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

    public int AwardPointsTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].Points > tempHighest)
            {
                tempHighest = playerTrophyStats[i].Points;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }
   

    public int TargetsHitTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].targetsHit > tempHighest)
            {
                tempHighest = playerTrophyStats[i].targetsHit;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

    public int BallCallingTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].callsForBall > tempHighest)
            {
                tempHighest = playerTrophyStats[i].callsForBall;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }
    public int KnockDownTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].dashTackle > tempHighest)
            {
                tempHighest = playerTrophyStats[i].dashTackle;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

    public int CoconutTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].coconutThrows > tempHighest)
            {
                tempHighest = playerTrophyStats[i].coconutThrows;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

    /*


    public int AwardBananasTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].bananasEaten > tempHighest)
            {
                tempHighest = playerTrophyStats[i].bananasEaten;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

    public int PoopTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].hitByPoop > tempHighest)
            {
                tempHighest = playerTrophyStats[i].hitByPoop;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

    public int AudienceWinsTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].audienceWins > tempHighest)
            {
                tempHighest = playerTrophyStats[i].audienceWins;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

    public int AudienceLossesTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].audienceLosses > tempHighest)
            {
                tempHighest = playerTrophyStats[i].audienceLosses;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

         public int PerfectCatchTrophy()
    {
        int winningPlayer = -1;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].perfectCatch > tempHighest)
            {
                tempHighest = playerTrophyStats[i].perfectCatch;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }
    */

    //TODO determine how many trophies will be awarded in the match
    public void CheckallWinners()
    {
        a = AwardPointsTrophy();
        c = TargetsHitTrophy();
        d = BallCallingTrophy();
        b = CatchTrophy();
        e = CoconutTrophy();
        f = KnockDownTrophy();
    }



}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerTrophyStats
{
    public int bananasEaten;
    public int perfectCatch;
    public int targetsHit;
    public int callsForBall;
    public int hitByPoop;
    public int knockDowns;
    public int audienceWins;
    public int audienceLosses;

    public PlayerTrophyStats()
    {
        bananasEaten = 0;
        perfectCatch = 0;
        targetsHit = 0;
        callsForBall = 0;
        hitByPoop = 0;
        knockDowns = 0;
        audienceWins = 0;
        audienceLosses = 0;
    }
}

public class TrophyManager : MonoBehaviour
{
    // list for each stat, each stat will have a function that will add it

    private static List<PlayerTrophyStats> playerTrophyStats = new List<PlayerTrophyStats>();

    void Start()
    {
        for(int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
        {
            playerTrophyStats.Add(new PlayerTrophyStats());
        }
    }
    public void BananasEaten(int playerIndex)
    {
        playerTrophyStats[playerIndex].bananasEaten++;
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
        playerTrophyStats[playerIndex].knockDowns++;
    }
    public void PerformPerfectCatch(int playerIndex)
    {
        playerTrophyStats[playerIndex].perfectCatch++;
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

    public int AwardBananasTrophy(List<PlayerTrophyStats> playerTrophyStats)
    {
        int winningPlayer = 0;
        int tempHighest = 0;

        for(int i = 0; i < playerTrophyStats.Count; i++)
        {
            if(playerTrophyStats[i].bananasEaten > tempHighest)
            {
                tempHighest = playerTrophyStats[i].bananasEaten;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

    public int PerfectCatchTrophy(List<PlayerTrophyStats> playerTrophyStats)
    {
        int winningPlayer = 0;
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

    public int TargetsHitTrophy(List<PlayerTrophyStats> playerTrophyStats)
    {
        int winningPlayer = 0;
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

    public int BallCallingTrophy(List<PlayerTrophyStats> playerTrophyStats)
    {
        int winningPlayer = 0;
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

    public int PoopTrophy(List<PlayerTrophyStats> playerTrophyStats)
    {
        int winningPlayer = 0;
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

    public int KnockDownTrophy(List<PlayerTrophyStats> playerTrophyStats)
    {
        int winningPlayer = 0;
        int tempHighest = 0;

        for (int i = 0; i < playerTrophyStats.Count; i++)
        {
            if (playerTrophyStats[i].knockDowns > tempHighest)
            {
                tempHighest = playerTrophyStats[i].knockDowns;
                winningPlayer = i;
            }
        }
        return winningPlayer;
    }

    public int AudienceWinsTrophy(List<PlayerTrophyStats> playerTrophyStats)
    {
        int winningPlayer = 0;
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

    public int AudienceLossesTrophy(List<PlayerTrophyStats> playerTrophyStats)
    {
        int winningPlayer = 0;
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

    //TODO determine how many trophies will be awarded in the match

}

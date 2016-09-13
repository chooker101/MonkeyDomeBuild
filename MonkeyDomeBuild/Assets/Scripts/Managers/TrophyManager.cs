using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerTrophyStats
{
    public int perfectJumpCatch;
    public int hitByPoop;
    public PlayerTrophyStats()
    {
        perfectJumpCatch = 0;
        hitByPoop = 0;
    }
}

public class TrophyManager : MonoBehaviour
{
    private static List<PlayerTrophyStats> playerTrophyStats = new List<PlayerTrophyStats>();

    void Start()
    {
        for(int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
        {
            playerTrophyStats.Add(new PlayerTrophyStats());
        }
    }
    public void PerformPerfectJumpCatch(int playerIndex)
    {
        playerTrophyStats[playerIndex].perfectJumpCatch++;
    }
    public void BeingHitByPoop(int playerIndex)
    {
        playerTrophyStats[playerIndex].hitByPoop++;
    }
}

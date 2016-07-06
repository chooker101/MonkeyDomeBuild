using UnityEngine;
using System.Collections;

public class ScoringManager : MonoBehaviour {

    /*
     * This script holds a bunch of methods that other scripts should call whenever a score occurs.
     * Every method takes an integer (should be 0,1, or 2) to reference an index of the specific type of score required.
     */

    private const int MINSCORE = 5;
    private const int MIDSCORE = 15;
    private const int MAXSCORE = 30;

    private int[] targetScores = new int[3];
    private int[] catchScores = new int[3];
    private int[] turnoverScores = new int[2];
    private int[] miscScores = new int[3];
    private int[] scoresArray = new int[3] {MINSCORE,MIDSCORE,MAXSCORE};

    /*
     * Call when player hits a target.
     * use targetRank 0 is for touching target without throwing ball
     * use targetRank 1 is for throwing the ball directly at the target
     * use targetRank 2 is for bouncing the ball once and hitting a target
     */
    public int TargetScore(int targetRank)
    {
        for (int i = 0; i < targetScores.Length; i++)
        {
            targetScores[i] = scoresArray[i];
        }
        return targetScores[targetRank];
    }

    /*
     * Call when players execute a pitch & catch. Both players must call the method when a pitch & catch occurs. 
     * use throwRank 0 for a nearby direct pass
     * use throwRank 1 for a faraway direct pass
     * use throwRank 2 for a pass that has bounced once
     */
    public int CatchScore(int throwRank)
    {
        for (int i = 0; i < catchScores.Length; i++)
        {
            catchScores[i] = scoresArray[i] * 2;
        }
        return catchScores[throwRank];
    }

    /*
     * Call when there is a turnover. All players must call this method when a turnover occurs. 
     * use turnoverSide 0 for Gorilla player
     * use turnoverSide 1 for monkey players
     */
    public int TurnoverScore(int turnoverSide)
    {
        for (int i = 0; i < turnoverScores.Length; i++)
        {
            if (i == 0)
            {
                turnoverScores[i] = scoresArray[i] * 3;
            }
            else
            {
                turnoverScores[i] = scoresArray[i] * -2;
            }
        }
        return turnoverScores[turnoverSide];
    }

    /*
     * Call when player performs miscellaneous scoring play.
     * use miscPlay 0 for picking up live ball (monkey)
     * use miscPlay 1 for knocking down monkey (gorilla, call multiple times for knocking down multiple monkeys);
     * use miscPlay 2 for taunting next to gorilla without getting knocked down (monkey)
     */
    public int MiscScore(int miscPlay)
    {
        for (int i = 0; i < miscScores.Length; i++)
        {
            miscScores[i] = scoresArray[i] / 2;
        }
        return miscScores[miscPlay];
    }

}

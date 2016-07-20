using UnityEngine;
using System.Collections;

public class ScoringManager {

    /*
     * This script holds a bunch of methods that other scripts should call whenever a score occurs.
     * Every method takes an integer (should be 0,1,2, or 3) to reference an index of the specific type of score required.
     * All values and calculations are ARBITRARY and are EXPECTED TO CHANGE. 
     * 
     ******* Still under construction/consideration ********
     *      - track each players' score here
     *      - method for returning/get player scores
     *      - methods for subtracting score (ie, not passsing in time for shot clock, gorilla, 
     */


    private const int MINSCORE = 5;
    private const int MIDSCORE = 10;
    private const int MAXSCORE = 30;
    private const int BADSCORE = -5;

    private const int VAL1 = 10;
    private const int VAL2 = 15;
    private const int VAL3 = 20;
    private const int VAL4 = 25;


    private static int[] targetScores = new int[3];
    private static int[] targetTiers = new int[4];
    private static int[] catchScores = new int[4];
    private static int[] turnoverScores = new int[2];
    private static int[] miscScores = new int[3];
    private static int[] gameRuleScores = new int[3];
    private  int[] scoresArray = new int[3] {MINSCORE,MIDSCORE,MAXSCORE};
    private  int[] miscValues = new int[4] { VAL1, VAL2, VAL3, VAL4 };

    public static int targetsHit;

    void Start()
    {
        //populate targetScores
        for (int i = 0; i < targetScores.Length; i++)
        {
            targetScores[i] = scoresArray[i];
        }
        //populate targetTiers
        for (int i = 0; i < targetTiers.Length; i++)
        {
            targetTiers[i] = miscValues[i];
        }

        //populate catchScores
        for (int i = 0; i < catchScores.Length; i++)
        {
            catchScores[i] = scoresArray[i] * 2;
        }

        //populate turnoverScores
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

        //populate gameRuleScores
        // 0 for shot clock gorilla ++ | 1 for target upgrade monkeys ++ | 2 for shot clock monkey -- | 3 for target downgrade gorilla --
        for (int i = 0; i < gameRuleScores.Length; i++)
        {
            if (i < 2)
            {
                gameRuleScores[i] = miscValues[i];
            } else
            {
                gameRuleScores[i] = -miscValues[i] / 2;
            }
        }

        //populate miscScores
        for (int i = 0; i < miscScores.Length; i++)
        {
            miscScores[i] = scoresArray[i] / 2;
        }
    }

    /*
     * Call when player hits a target.
     * use targetTier 0 if in target tier 1
     * use targetTier 1 if in target tier 2
     * use targetTier 2 if in target tier 3
     * use targetTier 3 if in target tier 4
     * use targetRank 0 is for touching target without throwing ball
     * use targetRank 1 is for throwing the ball directly at the target
     * use targetRank 2 is for bouncing the ball once and hitting a target
     */
    public static int TargetScore(int targetTier, int targetRank)
    {
        return targetScores[targetRank] + targetTiers[targetTier];
    }

    /*
     * Call when players execute a pitch & catch. Both players must call the method when a pitch & catch occurs. 
     * use throwRank 0 for a nearby direct pass
     * use throwRank 1 for a faraway direct pass
     * use throwRank 2 for a pass that has bounced once
     * use throwRank 3 for a pass that is not caught by monkey (only catching monkey calls)
     */
    public static int CatchScore(int throwRank)
    {
        return catchScores[throwRank];
    }

    /*
     * Call when there is a turnover. All players must call this method when a turnover occurs. 
     * use turnoverSide 0 for Gorilla player
     * use turnoverSide 1 for monkey players
     *          ******** NOTES **********
     *              - can/should gorilla just call this on his own and 
     *              detract all other players' scores, or does this work as is?
     */
    public static int TurnoverScore(int turnoverSide)
    {
        return turnoverScores[turnoverSide];
    }

    /*
     * Call when specific game action occurs. Various actions are called by various players.
     * use gameAction 0 for shot clock turnover. Gorilla that turns into monkey calls
     * use gameAction 1 for targetTier upgrade. All monkeys call this when targets are upgraded
     * use gameAction 2 for shot clock turnover. Monkey holding ball when clock expires calls
     * use gameAction 3 for targetTier downgrade. Gorilla calls this when targets are downgraded
     */
    public static int GameRuleScore(int gameAction)
    {
        return gameRuleScores[gameAction];
    }

    /*
     * Call when player performs miscellaneous scoring play.
     * use miscPlay 0 for picking up live ball (monkey)
     * use miscPlay 1 for knocking down monkey (gorilla, call multiple times for knocking down multiple monkeys);
     * use miscPlay 2 for taunting next to gorilla without getting knocked down (monkey)
     */
    public static int MiscScore(int miscPlay)
    {
        return miscScores[miscPlay];
    }


}

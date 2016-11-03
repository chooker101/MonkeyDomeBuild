using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreVisualizer : MonoBehaviour
{
    public List<GameObject> infos = new List<GameObject>();
    private List<Text> scoreDisplays = new List<Text>();
    private List<Text> scoreGainLose = new List<Text>();
    private List<Text> actionPerformedTexts = new List<Text>();
    private List<float> counts = new List<float>();
    private List<int> gainLoseScores = new List<int>();

    void Start()
    {
        for(int i = 0; i < infos.Count; i++)
        {
            scoreDisplays.Add(infos[i].transform.FindChild("Score").GetComponent<Text>());
            scoreGainLose.Add(infos[i].transform.FindChild("ScoreGainLose").GetComponent<Text>());
            actionPerformedTexts.Add(infos[i].transform.FindChild("ActionPerformed").GetComponent<Text>());
            scoreDisplays[i].text = "0";
            actionPerformedTexts[i].text = "";
            scoreGainLose[i].text = "";
            counts.Add(0);
        }
    }
    void LateUpdate()
    {
        for(int i = 0; i < counts.Count; i++)
        {
            if (counts[i] != 0)
            {
                if (counts[i] > 0)
                {
                    counts[i] -= Time.deltaTime;
                }
                else
                {
                    counts[i] = 0;
                    scoreDisplays[i].text = GameManager.Instance.gmScoringManager.GetScore(i).ToString();
                    scoreGainLose[i].text = "";
                    actionPerformedTexts[i].text = "";
                }
            }
        }
    }
    public void UpdateScore(int playerIndex,int score,int gainLoseScore,string action)
    {
        scoreDisplays[playerIndex].text = score.ToString();
        scoreGainLose[playerIndex].text = gainLoseScore > 0 ? "+" + gainLoseScore.ToString() : "-" + gainLoseScore.ToString();
        actionPerformedTexts[playerIndex].text = action;
        counts[playerIndex] = 1f;

    }

}

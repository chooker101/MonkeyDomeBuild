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
    private List<Text> playerNumbers = new List<Text>();
    private List<Image> playerHead_monkey = new List<Image>();
    private List<Image> playerHead_gorilla = new List<Image>();
    private List<Image> ballPossession = new List<Image>();

    private List<Image> playerBoardBack = new List<Image>();
    private List<Image> ledBack = new List<Image>();   

    void Start()
    {
        for(int i = 0; i < infos.Count; i++)
        {
            scoreDisplays.Add(infos[i].transform.FindChild("Score").GetComponent<Text>());
            scoreGainLose.Add(infos[i].transform.FindChild("ScoreGainLose").GetComponent<Text>());
            actionPerformedTexts.Add(infos[i].transform.FindChild("ActionPerformed").GetComponent<Text>());
            playerNumbers.Add(infos[i].transform.FindChild("PlayerNumber").GetComponent<Text>());
            playerHead_monkey.Add(infos[i].transform.FindChild("Monkey Head").GetComponent<Image>());
            playerHead_gorilla.Add(infos[i].transform.FindChild("Gorilla Head").GetComponent<Image>());
            ballPossession.Add(infos[i].transform.FindChild("Ball Possession").GetComponent<Image>());
            playerBoardBack.Add(infos[i].transform.FindChild("PlayerBoard Back").GetComponent<Image>());
            ledBack.Add(infos[i].transform.FindChild("LED Back").GetComponent<Image>());
            actionPerformedTexts[i].text = "";
            scoreGainLose[i].text = "";
            counts.Add(0);

            playerHead_monkey[i].color = GameManager.Instance.gmRecordKeeper.GetPlayerColour(i);
            playerHead_gorilla[i].color = GameManager.Instance.gmRecordKeeper.GetPlayerColour(i);
            playerBoardBack[i].color = GameManager.Instance.gmRecordKeeper.GetPlayerColour(i);
            ledBack[i].color = GameManager.Instance.gmRecordKeeper.GetPlayerColour(i);
            scoreDisplays[i].color = GameManager.Instance.gmRecordKeeper.GetPlayerColour(i);

            if(GameManager.Instance.TotalNumberofActors > i)
            {
                playerNumbers[i].text = "P" + (i + 1).ToString();
                scoreDisplays[i].text = "0";

                if (GameManager.Instance.gmPlayerScripts[i].characterType is Gorilla && !playerHead_gorilla[i].gameObject.activeSelf)
                {
                    playerHead_gorilla[i].gameObject.SetActive(true);
                    playerHead_monkey[i].gameObject.SetActive(false);
                }
                else if (GameManager.Instance.gmPlayerScripts[i].characterType is Monkey && !playerHead_monkey[i].gameObject.activeSelf)
                {
                    playerHead_monkey[i].gameObject.SetActive(true);
                    playerHead_gorilla[i].gameObject.SetActive(false);
                }
                if (GameManager.Instance.nextGameModeUI == GameManager.GameMode.Battle_Royal)
                {
                    infos[i].GetComponentInChildren<LifeImgHolder>().Init();
                }
            }
            else
            {
                infos[i].gameObject.SetActive(false);
            }
        }
        transform.localPosition = new Vector3((5 - GameManager.Instance.TotalNumberofActors) * 10, transform.localPosition.y, transform.localPosition.z);
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
        
        for (int i = 0; i< GameManager.Instance.TotalNumberofActors; i++)
        {
            // Changes who's holding the ball on the UI bin
            if (GameManager.Instance.gmPlayers[i].GetComponent<Actor>().IsHoldingBall && !ballPossession[i].gameObject.activeSelf)
            {
                if (GameManager.Instance.gmPlayers[i].GetComponent<Actor>().ballHolding.GetComponent<BallInfo>().IsBall)
                {
                    ballPossession[i].gameObject.SetActive(true);
                }
            }
            else if(!GameManager.Instance.gmPlayers[i].GetComponent<Actor>().IsHoldingBall && ballPossession[i].gameObject.activeSelf)
            {
                ballPossession[i].gameObject.SetActive(false);
            }

            // Displays if a player is a gorilla or a monkey on the UI bin
            if(GameManager.Instance.gmPlayerScripts[i].characterType is Gorilla && !playerHead_gorilla[i].gameObject.activeSelf)
            {
                playerHead_gorilla[i].gameObject.SetActive(true);
                playerHead_monkey[i].gameObject.SetActive(false);
            }
            else if(GameManager.Instance.gmPlayerScripts[i].characterType is Monkey && !playerHead_monkey[i].gameObject.activeSelf)
            {
                playerHead_monkey[i].gameObject.SetActive(true);
                playerHead_gorilla[i].gameObject.SetActive(false);
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
    public void PlayerHitHealthUpdate(int playerindex)
    {
        infos[playerindex].GetComponentInChildren<LifeImgHolder>().LifeDown();
    }

}

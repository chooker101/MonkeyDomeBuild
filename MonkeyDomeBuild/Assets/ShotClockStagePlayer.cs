using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShotClockStagePlayer : MonoBehaviour {
    private Text currentPlayerText;
    private GameObject currentPlayer = null;
    private int index;
     
	// Use this for initialization
	void Start () {
        currentPlayerText = GetComponent<Text>();
        
	}
	
	// Update is called once per frame
	void Update () {
        currentPlayer = GameManager.Instance.gmBalls[0].GetComponent<BallInfo>().GetHoldingMonkey();
        if (currentPlayer != null)
        {
            switch (currentPlayer.GetComponent<Actor>().playerIndex)
            {
                case 0:
                    currentPlayerText.text = "PLAYER 1";
                    break;
                case 1:
                    currentPlayerText.text = "PLAYER 2";
                    break;
                case 2:
                    currentPlayerText.text = "PLAYER 3";
                    break;
                case 3:
                    currentPlayerText.text = "PLAYER 4";
                    break;
                case 4:
                    currentPlayerText.text = "PLAYER 5";
                    break;

            }
            currentPlayerText.color = GameManager.Instance.gmRecordKeeper.colourPlayers[currentPlayer.GetComponent<Actor>().playerIndex].color;
        }
    }
}

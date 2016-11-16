using UnityEngine;
using System.Collections;

public class BallReturn : MonoBehaviour
{
    public GameObject[] balls;
    public BallInfo[] ballsInfo;
    public GameObject pgt;

    private bool[] wasActive;
    // Use this for initialization
    void Start()
    {
        balls = GameObject.FindGameObjectsWithTag("Ball");
        wasActive = new bool[balls.Length];
    }

    // Update is called once per frame
    void Update()
    {
        checkEnable();// Will Enable the ball if the monkeys do not have the same sprite assigned to it, otherwise it will disable
    }

    void checkEnable()
    {
        if(pgt.GetComponent<PreGameTimer>().gameState == GameState.Pregame_PickingColours)
        {
            // Checks the record keeper for what colour the player is, and if the player has the same sprite material as the ball, disables the ball.
            for (int i = 0; i < balls.Length; i++)
            {
                wasActive[i] = balls[i].activeSelf;
                for(int o = 0; o < GameManager.Instance.gmRecordKeeper.colourPlayers.Count; o++)
                {
                    Material ball = balls[i].GetComponent<BallInfo>().mySpriteColour;
                    Material player = GameManager.Instance.gmRecordKeeper.colourPlayers[o];

                    if (ball == player)
                    {
                        balls[i].SetActive(false);
                        break;
                    }
                    else
                    {
                        balls[i].SetActive(true);

                    }
                }
                if (!wasActive[i] && balls[i].activeSelf)
                {
                    balls[i].GetComponent<Transform>().position = transform.position;
                }
            }
        }
        else
        {
            for(int i = 0; i < balls.Length; i++)
            {
                balls[i].SetActive(false);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class BallReturn : MonoBehaviour
{
    public GameObject[] balls;
    public BallInfo[] ballsInfo;

    // Use this for initialization
    void Start()
    {
        balls = GameObject.FindGameObjectsWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        checkEnable();// Will Enable the ball if the monkeys do not have the same sprite assigned to it, otherwise it will disable
    }

    void checkEnable()
    {
        // Checks the record keeper for what colour the player is, and if the player has the same sprite material as the ball, disables the ball.
        for (int i = 0; i < balls.Length; i++)
        {
            for(int o = 0; o < GameManager.Instance.gmRecordKeeper.colourPlayers.Length; o++)
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
                    gameObject.SetActive(true);
                }
            }
        }
    }
}

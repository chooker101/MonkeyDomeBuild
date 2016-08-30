using UnityEngine;
using System.Collections;

public class BallReturn : MonoBehaviour
{
    private RecordKeeper rk_keeper;
    private GameObject[] balls;

    // Use this for initialization
    void Start()
    {
        rk_keeper = FindObjectOfType<RecordKeeper>().GetComponent<RecordKeeper>();
        balls = GameObject.FindGameObjectsWithTag("ball");
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
            for(int o = 0; o < rk_keeper.colourPlayers.Length; o++)
            {
                if (balls[i].GetComponent<BallInfo>().mySpriteColour == rk_keeper.colourPlayers[o])
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

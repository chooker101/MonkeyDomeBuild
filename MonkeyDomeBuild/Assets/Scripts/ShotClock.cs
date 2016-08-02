using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShotClock : MonoBehaviour
{
    Text shotClock = null;
    public GameObject ball;
    void Start()
    {
        shotClock = GetComponent<Text>();
    }


    void Update()
    {
        float time = 8f - ball.GetComponent<BallInfo>().GetCurrentShotClockTime();
        if (time < 0) time = 0f;
        shotClock.text = Mathf.Round(time).ToString();
        //Debug.Log(time);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShotClock : MonoBehaviour
{
    Text shotClock = null;
    void Start()
    {
        shotClock = GetComponent<Text>();      
    }


    void Update()
    {
        if (GameManager.Instance.gmBalls[0] != null)
        {
            float time = 8f - GameManager.Instance.gmBalls[0].GetComponent<BallInfo>().GetCurrentShotClockTime();
            if (time < 0) time = 0f;
            shotClock.text = Mathf.Round(time).ToString();
            //Debug.Log(time);
        }
    }
}

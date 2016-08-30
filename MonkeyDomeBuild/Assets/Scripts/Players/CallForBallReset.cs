using UnityEngine;
using System.Collections;

public class CallForBallReset : MonoBehaviour
{
    public float resetTime = 1f;
    private float resetCount = 0f;
    public GameObject callForBall;
    void Start()
    {
        callForBall.SetActive(false);
    }

    void OnEnable()
    {
        resetCount = resetTime;
    }

    void Update()
    {
        if (callForBall.activeSelf)
        {
            if (resetCount <= 0)
            {
                callForBall.SetActive(false);
            }
            else
            {
                resetCount -= Time.deltaTime;
            }
        }
    }

    public void CallForBall()
    {
        if (!callForBall.activeSelf)
        {
            callForBall.SetActive(true);
            resetCount = resetTime;
        }
    }

    public bool CallForBallActive
    {
        get { return callForBall.activeSelf; }
    }
}

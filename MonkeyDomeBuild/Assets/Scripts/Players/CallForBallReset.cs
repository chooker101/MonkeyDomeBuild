using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CallForBallReset : MonoBehaviour
{
    public float resetTime = 1f;
    private float resetCount = 0f;
    public GameObject callForBall;
    public GameObject hereIAm;
    public GameObject chosenCall;
    private EffectControl effects;

    void Start()
    {
        effects = transform.GetComponentInParent<EffectControl>();
        // Selects the appropriate call to use based on level
        if(SceneManager.GetActiveScene().name == "PregameRoom")
        {
            chosenCall = hereIAm;
        }
        else
        {
            chosenCall = callForBall;
        }

        chosenCall.SetActive(false);
    }

    void OnEnable()
    {
        resetCount = resetTime;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "PregameRoom")
        {
            chosenCall = hereIAm;
        }
        else
        {
            chosenCall = callForBall;
        }

        if (GetComponentInParent<Actor>().characterType is Monkey)
        {
            if (chosenCall.activeSelf)
            {
                if (resetCount <= 0)
                {
                    chosenCall.SetActive(false);
                }
                else
                {
                    resetCount -= Time.deltaTime;
                }
            }
        }
        else
        {
            chosenCall.SetActive(false);
        }

    }

    public void CallForBall()
    {
        if (!chosenCall.activeSelf)
        {
            chosenCall.SetActive(true);
            resetCount = resetTime;
            
            effects.playerLocatorTimer = effects.playerLocatorTimeMax;
        }
    }

    public bool CallForBallActive
    {
        get { return chosenCall.activeSelf; }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShotClock : MonoBehaviour
{
    private bool scalingUp;
    Text shotClock = null;

    private Vector3 originalScale;
    private Vector3 tempScale;
    private bool playShotClocAudio = true;
    void Start()
    {
        shotClock = GetComponent<Text>();   
        scalingUp = true;
        originalScale = shotClock.transform.localScale;

    }


    void Update()
    {
        /*if (GameManager.Instance.gmBalls[0] != null)
        {

        }*/
        //float time = 8f - GameManager.Instance.gmBalls[0].GetComponent<BallInfo>().GetCurrentShotClockTime();
        float time = GameManager.Instance.gmShotClockManager.ShotClockTime - GameManager.Instance.gmShotClockManager.ShotClockCount;
        if (time < 0) time = 0f;
        shotClock.text = time.ToString("F2");
        //Debug.Log(time);

        if (time > 4f)
        {
            playShotClocAudio = true;
            if (!gameObject.CompareTag("StageUI"))
            {
                shotClock.fontStyle = FontStyle.Normal;
            }
            shotClock.color = new Color(0f, 0f, 00f);
        }
        else if (time > 2f && time <= 4f)
        {
            shotClock.color = Color.red; //new Color(200f, 200f, 00f);
            if (!gameObject.CompareTag("StageUI"))
            {
                if (scalingUp)
                {
                    ScaleUpText(3f, 1f);
                }
                else
                {
                    ScaleDownText();
                }
            }
        }
        else
        {
            shotClock.color = Color.white; //new Color(200f, 0f, 00f);
            if (!gameObject.CompareTag("StageUI"))
            {
                if (playShotClocAudio)
                {
                    AudioEffectManager.Instance.PlayAudienceShotClock();
                    playShotClocAudio = false;
                }
                if (scalingUp)
                {
                    shotClock.fontStyle = FontStyle.Bold;
                    ScaleUpText(5f, 5f);
                }
                else
                {
                    ScaleDownText();
                }
            }
        }
    }

     void ScaleUpText(float factor, float time)
    {
        tempScale = shotClock.transform.localScale;

        tempScale.x = Mathf.Lerp(shotClock.transform.localScale.x, factor, time * Time.deltaTime);
        tempScale.y = Mathf.Lerp(shotClock.transform.localScale.y, factor, time * Time.deltaTime);

        shotClock.transform.localScale = tempScale;
        StartCoroutine(ScalingTime());
    }

     void ScaleDownText()
    {
        tempScale = shotClock.transform.localScale;

        tempScale.x = Mathf.Lerp(shotClock.transform.localScale.x, originalScale.x, 5f * Time.deltaTime);
        tempScale.y = Mathf.Lerp(shotClock.transform.localScale.y, originalScale.y, 5f * Time.deltaTime);

        shotClock.transform.localScale = tempScale;
        scalingUp = true;
    }

     IEnumerator ScalingTime()
    {
        yield return new WaitForSeconds(.5f);
        scalingUp = false;
    }
}

﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EffectControl : MonoBehaviour
{
    Color particleColour = Color.white;
    public GameObject particleSetPrefab;
    private PartilcleController controller;
    public Image perfectCatch;
    private float perfectCatchLastTime = 0.6f;
    private float perfectCatchFadeSpeed = 5f;
    private bool perfectCatchEffectEnd = true;

    public Image gorillaStamp;
    private float stampLastTime = 0.7f;
    private float stampTargetScaleMultiplier = 0.5f;
    private bool startStamp = false;
    private Vector3 stampTargetScale;
    private Vector3 stampInitScale;
    private Vector3 stampInitLoc;
    private Vector3 stampTargetLoc;
    private float stampShrinkSpeed = 10f;
    private float stampMoveSpeed = 5f;
    private float stampFadeSpeed = 5f;

    void Start()
    {
        stampInitScale = gorillaStamp.transform.localScale;
        stampTargetScale = new Vector3(1, 1, 1) * stampTargetScaleMultiplier;
        stampInitLoc = gorillaStamp.transform.localPosition;
        stampTargetLoc = new Vector3(0, 0, -60f);
        if (particleSetPrefab != null)
        {
            GameObject temp = Instantiate(particleSetPrefab);
            temp.transform.SetParent(transform);
            temp.transform.localRotation = Quaternion.identity;
            temp.transform.localPosition = new Vector3(0, 0, -0.2f);
            controller = temp.GetComponent<PartilcleController>();
        }

    }
    void Update()
    {
        if (perfectCatchEffectEnd)
        {
            if (Mathf.Abs(perfectCatch.color.a - 1f) > 0.05f)
            {
                Color c = perfectCatch.color;
                c.a = Mathf.Lerp(c.a, 0, Time.deltaTime * perfectCatchFadeSpeed);
            }
            else
            {
                if(perfectCatch.color.a != 0)
                {
                    Color c = perfectCatch.color;
                    c.a = 0;
                    perfectCatch.color = c;
                }
            }         
        }
        if (startStamp)
        {
            gorillaStamp.GetComponent<RectTransform>().localScale = Vector3.LerpUnclamped(gorillaStamp.transform.localScale, stampTargetScale, Time.deltaTime * stampShrinkSpeed);
            gorillaStamp.transform.localPosition = Vector3.LerpUnclamped(gorillaStamp.transform.localPosition, stampTargetLoc, Time.deltaTime * stampMoveSpeed);
        }
        else
        {
            if(Mathf.Abs(gorillaStamp.color.a - 1f) > 0.05f)
            {
                Color c = gorillaStamp.color;
                c.a = Mathf.Lerp(c.a, 0, Time.deltaTime * stampFadeSpeed);
                gorillaStamp.color = c;
            }
            else
            {
                if(gorillaStamp.color.a != 0)
                {
                    Color c = gorillaStamp.color;
                    c.a = 0;
                    gorillaStamp.color = c;
                    gorillaStamp.transform.localScale = stampInitScale;
                    gorillaStamp.transform.localPosition = stampInitLoc;
                }
            }
        }
    }

    public void PlayCatchEffect(GameObject ball)
    {
        if (particleColour != GameManager.Instance.gmRecordKeeper.GetPlayerColour(GetComponent<Actor>().playerIndex))
        {
            particleColour = GameManager.Instance.gmRecordKeeper.GetPlayerColour(GetComponent<Actor>().playerIndex);
        }

        ParticleSystem[] temp = controller.CatchEffect.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in temp)
        {
            ps.startColor = particleColour;
        }
        Vector3 ballPos = ball.transform.position;
        ballPos.z = 0;
        Vector3 pivotPos = controller.transform.position;
        pivotPos.z = 0;
        Quaternion targetAng = Quaternion.FromToRotation(Vector3.right, (ballPos - pivotPos).normalized);
        controller.CatchEffect.rotation = Quaternion.Euler(0, 0, targetAng.eulerAngles.z);
        controller.CatchEffect.GetComponentInChildren<ParticleSystem>().Play();
    }
    public void PlayPerfectCatchEffect(GameObject ball)
    {

        PlayCatchEffect(ball);
        Color c = particleColour;
        c.a = 1f;
        perfectCatch.color = c;
        perfectCatchEffectEnd = false;
        Invoke("ResetPerfectEffect", perfectCatchLastTime);
    }
    void ResetPerfectEffect()
    {
        perfectCatchEffectEnd = true;
        CancelInvoke("ResetPerfectEffect");
    }
    public void PlaySwitchEffect()
    {
        if (particleColour != GameManager.Instance.gmRecordKeeper.GetPlayerColour(GetComponent<Actor>().playerIndex))
        {
            particleColour = GameManager.Instance.gmRecordKeeper.GetPlayerColour(GetComponent<Actor>().playerIndex);
        }
        startStamp = true;
        Color c = particleColour;
        c.a = 1f;
        gorillaStamp.color = c;
        Invoke("ResetSwitchEffect", stampLastTime);
    }
    void ResetSwitchEffect()
    {
        startStamp = false;
        CancelInvoke("ResetSwitchEffect");
    }

}

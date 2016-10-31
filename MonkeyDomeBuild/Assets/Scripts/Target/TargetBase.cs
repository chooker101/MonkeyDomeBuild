﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public enum TargetBaseState
{
    Null,
    RallyStart,
    Hit,
    Miss,
    Pop,
    Prep,
    Warning,
    RallyEnd
}
public class TargetBase : MonoBehaviour
{
    public enum TierStatus
    {
        TierNull,
        TierUp,
        TierStay,
        TierDown
    }
    Target target;
    public TargetBaseState state = TargetBaseState.Null;
    public TierStatus tierStatus = TierStatus.TierNull;
    public Transform slider;
    public Image start;
    public Image end;
    public Image hit;
    public Image miss;
    public Image pop;
    public Image prep;
    public Image warning;
    public Image upTier;
    public Image stayTier;
    public Image downTier;

    float timeCount = 0;
    float defaultTime = 2f;
    private bool changeTier = false;

    private GameObject activatedImg = null;

    void Start()
    {
        changeTier = true;
        //ChangeTargetState(TargetBaseState.RallyStart);
    }


    void Update()
    {
        PanelUpdate();
        TierUpdate();
    }
    void PanelUpdate()
    {
        switch (state)
        {
            case TargetBaseState.RallyStart:
                StartState(2f);
                break;
            case TargetBaseState.Hit:
                HitState();
                break;
            case TargetBaseState.Miss:
                MissState();
                break;
            case TargetBaseState.Pop:
                PopState();
                break;
            case TargetBaseState.Prep:
                PrepState();
                break;
            case TargetBaseState.Warning:
                WarningState();
                break;
            case TargetBaseState.RallyEnd:
                EndState();
                break;
        }
    }
    void TierUpdate()
    {
        if (GameManager.Instance.gmTargetManager.RallyOn)
        {
            if (GameManager.Instance.gmTargetManager.HitSum >= 3)
            {
                if (tierStatus != TierStatus.TierUp)
                {
                    tierStatus = TierStatus.TierUp;
                    changeTier = true;
                }
            }
            else if (GameManager.Instance.gmTargetManager.HitSum >= 2)
            {
                if (tierStatus != TierStatus.TierStay)
                {
                    tierStatus = TierStatus.TierStay;
                    changeTier = true;
                }
            }
            else
            {
                if (tierStatus != TierStatus.TierDown)
                {
                    tierStatus = TierStatus.TierDown;
                    changeTier = true;
                }
            }
        }
        else
        {
            if (tierStatus != TierStatus.TierNull)
            {
                //tierStatus = TierStatus.TierNull;
                //changeTier = true;
            }
        }
        if (changeTier)
        {
            changeTier = false;
            switch (tierStatus)
            {
                case TierStatus.TierNull:
                    upTier.color = Color.grey;
                    stayTier.color = Color.grey;
                    downTier.color = Color.grey;
                    break;
                case TierStatus.TierUp:
                    upTier.color = Color.green;
                    stayTier.color = Color.green;
                    downTier.color = Color.green;
                    break;
                case TierStatus.TierStay:
                    upTier.color = Color.grey;
                    stayTier.color = Color.yellow;
                    downTier.color = Color.yellow;
                    break;
                case TierStatus.TierDown:
                    upTier.color = Color.grey;
                    stayTier.color = Color.grey;
                    downTier.color = Color.red;
                    break;
            }
        }
    }
    public void ChangeTargetState(TargetBaseState state)
    {
        if(this.state != state)
        {
            Vector3 sliderNewScale = slider.localScale;
            sliderNewScale.x = 1;
            slider.localScale = sliderNewScale;
            timeCount = 0;
            this.state = state;
            if (activatedImg != null)
            {
                activatedImg.SetActive(false);
            }
            switch (state)
            {
                case TargetBaseState.RallyStart:
                    activatedImg = start.gameObject;
                    break;
                case TargetBaseState.Hit:
                    activatedImg = hit.gameObject;
                    break;
                case TargetBaseState.Miss:
                    activatedImg = miss.gameObject;
                    break;
                case TargetBaseState.Pop:
                    activatedImg = pop.gameObject;
                    break;
                case TargetBaseState.Prep:
                    activatedImg = prep.gameObject;
                    break;
                case TargetBaseState.Warning:
                    activatedImg = warning.gameObject;
                    break;
                case TargetBaseState.RallyEnd:
                    activatedImg = end.gameObject;
                    break;
            }
        }
    }
    void StartState(float time)
    {
        if (!activatedImg.activeInHierarchy && timeCount == 0)
        {
            activatedImg.SetActive(true);
            timeCount = time;
        }
        if (timeCount <= 0)
        {
            ChangeTargetState(TargetBaseState.Null);
        }
        else
        {
            Vector3 sliderNewScale = slider.localScale;
            sliderNewScale.x = (time - timeCount) / time;
            slider.localScale = sliderNewScale;
            timeCount -= Time.deltaTime;
        }
    }
    void HitState()
    {
        StartState(defaultTime);
    }
    void MissState()
    {
        StartState(defaultTime);
    }
    void PopState()
    {
        if (!activatedImg.activeInHierarchy)
        {
            activatedImg.SetActive(true);
            timeCount = target.InitialLifeTime - target.WarningTime;
        }
        Vector3 sliderNewScale = slider.localScale;
        sliderNewScale.x = (target.InitialLifeTime - target.WarningTime - timeCount) / (target.InitialLifeTime - target.WarningTime);
        timeCount -= Time.deltaTime;
        slider.localScale = sliderNewScale;
    }
    void PrepState()
    {
        if (!activatedImg.activeInHierarchy)
        {
            activatedImg.SetActive(true);
            timeCount = target.PrepTime;
        }
        Vector3 sliderNewScale = slider.localScale;
        sliderNewScale.x = (target.PrepTime - timeCount) / target.PrepTime;
        timeCount -= Time.deltaTime;
        slider.localScale = sliderNewScale;
    }
    void WarningState()
    {
        StartState(target.WarningTime);
    }
    void EndState()
    {
        StartState(3f);
    }
    public Target SetTarget
    {
        set
        {
            target = value;
        }
    }

}